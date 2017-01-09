using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace MMU.BoerseDownloader.WpfUI.Infrastructure
{
    public class UiBlockDetector
    {
        static Timer _timer;

        public UiBlockDetector(int maxFreezeTimeInMilliseconds = 200)
        {
            var sw = new Stopwatch();

            new DispatcherTimer(TimeSpan.FromMilliseconds(10), DispatcherPriority.Send, (sender, args) =>
            {
                lock (sw)
                {
                    sw.Restart();
                }
            }, Application.Current.Dispatcher);

            _timer = new Timer(state =>
            {
                lock (sw)
                {
                    if (sw.ElapsedMilliseconds > maxFreezeTimeInMilliseconds)
                    {
                        // Debugger.Break() or set breakpoint here;
                        // Goto Visual Studio --> Debug --> Windows --> Theads 
                        // and checkup where the MainThread is.
                    }
                }
            }, null, TimeSpan.FromMilliseconds(0), TimeSpan.FromMilliseconds(10));
        }
    }
}