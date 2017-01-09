using System.Windows;
using System.Windows.Controls;

namespace MMU.BoerseDownloader.WpfUI.UserControls
{
    /// <summary>
    /// Interaction logic for DownloadEntriesFilterUserControl.xaml
    /// </summary>
    public partial class DownloadEntriesFilterUserControl : UserControl
    {
        public static readonly DependencyProperty ShowVisitedDownloadEntriesProperty = DependencyProperty.Register("ShowVisitedDownloadEntries", typeof(bool), typeof(DownloadEntriesFilterUserControl));

        public DownloadEntriesFilterUserControl()
        {
            InitializeComponent();
        }

        public bool ShowVisitedDownloadEntries { get; set; }
    }
}