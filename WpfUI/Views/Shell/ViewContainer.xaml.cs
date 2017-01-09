using System.ComponentModel;
using System.Windows;
using Microsoft.Practices.Unity;
using MMU.BoerseDownloader.WpfUI.Interfaces;
using MMU.BoerseDownloader.WpfUI.ViewModels.Shell;

namespace MMU.BoerseDownloader.WpfUI.Views.Shell
{
    /// <summary>
    /// Interaction logic for ViewContainer.xaml
    /// </summary>
    public partial class ViewContainer : Window, IClosable
    {
        public ViewContainer()
        {
            DataContext = Common.Singletons.UnityContainerSingleton.Instance.Resolve<ViewModelContainer>();
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            App_End.TearDowner.Teardown();
            base.OnClosing(e);
        }
    }
}