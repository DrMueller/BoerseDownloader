using System;
using System.Linq;
using System.Threading;
using System.Windows.Navigation;
using HtmlAgilityPack;
using mshtml;
using MMU.BoerseDownloader.Integration.Infrastructure.Extensions;
using MMU.BoerseDownloader.Integration.Infrastructure.Helpers;
using MMU.BoerseDownloader.Integration.UserControls;
using MMU.BoerseDownloader.Model;

namespace MMU.BoerseDownloader.Integration.Logics.Adapters
{
    public class WebBrowserAdapter : IDisposable
    {
        private const int MAX_WAIT_COUNTER = 30;
        private bool _disposed;
        private DownloadContext _downloadContext;
        private string _html;
        private bool _htmlprocessed;
        private bool _loginAlreadyPerformed;
        private WebBrowserUserControl _webBrowserControl;

        public WebBrowserAdapter()
        {
            ThreadingHelper.ExecuteInUiThread(() =>
            {
                _webBrowserControl = new WebBrowserUserControl();
                _webBrowserControl.WebBrowser.HideScriptErrors();
                _webBrowserControl.WebBrowser.LoadCompleted += WebBrowser_LoadCompleted;
                _webBrowserControl.Show();
            });
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal string DownloadThreadHtml(DownloadContext downloadContext, BoerseUser boerseUser)
        {
            _htmlprocessed = false;
            _downloadContext = downloadContext;
            NavigateToThread();
            AssureUserIsLoggedIn(boerseUser);

            return _html;
        }

        private void AssureUserIsLoggedIn(BoerseUser boerseUser)
        {
            if (!_loginAlreadyPerformed)
            {
                var currentCnter = 0;
                var loginWasNeeded = CheckIfLoginNeededAndProcessWorkflow(boerseUser);
                while (loginWasNeeded && (currentCnter < MAX_WAIT_COUNTER))
                {
                    currentCnter++;
                    Thread.Sleep(1000);
                    loginWasNeeded = CheckIfLoginNeededAndProcessWorkflow(boerseUser);
                }

                _loginAlreadyPerformed = true;
            }
        }

        private bool CheckIfLoginNeededAndProcessWorkflow(BoerseUser boerseUser)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(_html);

            var accountUserNameControl = htmlDocument.DocumentNode.Descendants().FirstOrDefault(f => f.GetElementClassName() == "accountUsername");

            if (accountUserNameControl == null)
            {
                _webBrowserControl.WebBrowser.LogInBoerseForum(boerseUser);
                return true;
            }

            return false;
        }

        private void NavigateToThread()
        {
            try
            {
                ThreadingHelper.ExecuteInUiThread(() => _webBrowserControl.WebBrowser.Navigate(_downloadContext.ThreadUrl));
            }
            catch (Exception)
            {
                throw new Exception($"Could not navigate to '{_downloadContext.Name}'.");
            }

            // Assure we're processed
            var currentCnter = 0;
            while (!_htmlprocessed && (currentCnter < MAX_WAIT_COUNTER))
            {
                currentCnter++;
                Thread.Sleep(1000);
            }

            if (!_htmlprocessed || string.IsNullOrEmpty(_html))
            {
                throw new Exception($"Could not load HTML from URL '{_downloadContext.Name}'.");
            }
        }

        private void ReadHtmlFromControl()
        {
            ThreadingHelper.ExecuteInUiThread(() =>
            {
                var doc = (IHTMLDocument2)_webBrowserControl.WebBrowser.Document;
                _html = doc.body.outerHTML;
            });
        }

        private void WebBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            ReadHtmlFromControl();
            _htmlprocessed = true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _webBrowserControl.Close();
                }

                _disposed = true;
            }
        }

        ~WebBrowserAdapter()
        {
            Dispose(false);
        }
    }
}