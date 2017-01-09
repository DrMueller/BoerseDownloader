using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Controls;
using mshtml;
using MMU.BoerseDownloader.Model;

namespace MMU.BoerseDownloader.Integration.Infrastructure.Extensions
{
    internal static class WebBrowserExtensions
    {
        // http://stackoverflow.com/questions/1298255/how-do-i-suppress-script-errors-when-using-the-wpf-webbrowser-control 
        internal static void HideScriptErrors(this WebBrowser webBrowser)
        {
            var webBrowserField = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (webBrowserField == null)
            {
                return;
            }

            var objComWebBrowser = webBrowserField.GetValue(webBrowser);
            if (objComWebBrowser == null)
            {
                webBrowser.Loaded += (o, s) => HideScriptErrors(webBrowser); //// In case we are to early
                return;
            }

            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { true });
        }

        internal static void LogInBoerseForum(this WebBrowser webBrowser, BoerseUser boerseUser)
        {
            Thread.Sleep(1000);
            IHTMLDocument2 doc = null;
            Helpers.ThreadingHelper.ExecuteInUiThread(() =>
            {
                doc = (IHTMLDocument2)webBrowser.Document;
            });

            OpenLoginForm(doc);

            var allElements = doc.all.Cast<IHTMLElement>().ToList();
            if (!TryFillingLoginValues(allElements, boerseUser))
            {
                // Let's assume we're already logged in
                return;
            }

            FindAndUseLoginButton(allElements);
        }

        private static void FindAndUseLoginButton(IEnumerable<IHTMLElement> allElements)
        {
            var keepBeingLoggedInCtrl = allElements.First(f => f.id == Constants.BoerseUserControls.CHECKBOX_REMEMBER_LOGIN);
            var parent = keepBeingLoggedInCtrl.parentElement.parentElement;
            IHTMLElement loginButtonElement = null;

            foreach (IHTMLElement childElement in parent.children)
            {
                var valueAttr = childElement.getAttribute("value");
                if (valueAttr != null && valueAttr == "Anmelden")
                {
                    loginButtonElement = childElement;
                    break;
                }
            }

            Thread.Sleep(1000);
            loginButtonElement.click();
        }

        private static bool TryFillingLoginValues(IEnumerable<IHTMLElement> htmlElements, BoerseUser boerseUser)
        {
            var loginCtrl = (IHTMLInputElement)htmlElements.FirstOrDefault(f => f.id == Constants.BoerseUserControls.TEXTBOX_LOGIN);
            if (loginCtrl == null)
            {
                return false;
            }

            var passwordControl = (IHTMLInputElement)htmlElements.First(f => f.id == Constants.BoerseUserControls.TEXTBOX_PASSWORD);
            var keepBeingLoggedInCtrl = (IHTMLInputElement)htmlElements.First(f => f.id == Constants.BoerseUserControls.CHECKBOX_REMEMBER_LOGIN);

            keepBeingLoggedInCtrl.@checked = true;
            loginCtrl.value = boerseUser.LoginName;
            passwordControl.value = boerseUser.Password;

            return true;
        }

        private static void OpenLoginForm(IHTMLDocument2 doc)
        {
            var allElements = doc.all.Cast<IHTMLElement>();
            var loginButton = allElements.FirstOrDefault(f => f.id == "SignupButton");

            // Already Logged in
            if (loginButton == null)
            {
                return;
            }

            loginButton.click();
            Thread.Sleep(1000);
        }
    }
}