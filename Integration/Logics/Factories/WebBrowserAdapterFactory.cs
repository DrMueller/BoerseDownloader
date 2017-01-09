using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using MMU.BoerseDownloader.Integration.Logics.Adapters;
using MMU.BoerseDownloader.Model;

namespace MMU.BoerseDownloader.Integration.Logics.Factories
{
    public class WebBrowserAdapterFactory : IDisposable
    {
        private readonly ConcurrentDictionary<string, WebBrowserAdapter> _adapterCache = new ConcurrentDictionary<string, WebBrowserAdapter>();
        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal WebBrowserAdapter Create(DownloadContext downloadContext)
        {
            var result = GetCreateChachedAdapter(downloadContext);
            return result;
        }

        private WebBrowserAdapter GetCreateChachedAdapter(DownloadContext downloadContext)
        {
            WebBrowserAdapter result;
            if (!_adapterCache.TryGetValue(downloadContext.Name, out result))
            {
                result = Common.Singletons.UnityContainerSingleton.Instance.Resolve<WebBrowserAdapter>();
                _adapterCache.TryAdd(downloadContext.Name, result);
            }

            return result;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _adapterCache.Select(f => f.Value).ForEach(f => f.Dispose());
                }

                //// Unmanaged resources are released here.
                _disposed = true;
            }
        }

        ~WebBrowserAdapterFactory()
        {
            Dispose(false);
        }
    }
}