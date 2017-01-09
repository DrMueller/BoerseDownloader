using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace MMU.BoerseDownloader.WpfUI.Views
{
    /// <summary>
    /// Interaction logic for DownloadEntriesOverviewView.xaml
    /// </summary>
    public partial class DownloadEntriesOverviewView : UserControl
    {
        public DownloadEntriesOverviewView()
        {
            InitializeComponent();
        }

        private void EntryUrl_Clicked(object sender, RoutedEventArgs e)
        {
            Hyperlink link = (Hyperlink)e.OriginalSource;
            Process.Start(link.NavigateUri.AbsoluteUri);
        }
    }
}