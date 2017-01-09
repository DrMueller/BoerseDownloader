using System;
using System.Windows;

namespace MMU.BoerseDownloader.Integration.Infrastructure.Helpers
{
    internal static class ThreadingHelper
    {
        internal static void ExecuteInUiThread(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }
    }
}