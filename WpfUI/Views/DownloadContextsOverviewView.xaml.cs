using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace MMU.BoerseDownloader.WpfUI.Views
{
    /// <summary>
    /// Interaction logic for DownloadContectsOverviewView.xaml
    /// </summary>
    public partial class DownloadContextsOverviewView : UserControl
    {
        public DownloadContextsOverviewView()
        {
            InitializeComponent();
        }

        private void ThreadUrl_Clicked(object sender, RoutedEventArgs e)
        {
            Hyperlink link = (Hyperlink)e.OriginalSource;
            Process.Start(link.NavigateUri.AbsoluteUri);
        }
    }
}