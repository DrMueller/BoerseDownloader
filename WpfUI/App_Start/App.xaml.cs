using System.Windows;
using Microsoft.Practices.Unity;
using MMU.BoerseDownloader.WpfUI.Views.Shell;

namespace MMU.BoerseDownloader.WpfUI.App_Start
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Bootstrapper.Initialize();
            var viewContainer = Common.Singletons.UnityContainerSingleton.Instance.Resolve<ViewContainer>();
            viewContainer.Show();
        }
    }
}