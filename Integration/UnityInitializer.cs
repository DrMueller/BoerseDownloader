using Microsoft.Practices.Unity;
using MMU.BoerseDownloader.Common.Interfaces;
using MMU.BoerseDownloader.Integration.Logics.BoerseLinkThreadHandlers;

namespace MMU.BoerseDownloader.Integration
{
    public class UnityInitializer : IUnityInitializer
    {
        public void InitializeContainer(IUnityContainer unityContainer)
        {
            unityContainer.RegisterType<Logics.Factories.WebBrowserAdapterFactory>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<Logics.Factories.BoerseLinkThreadHandlerFactory>(new ContainerControlledLifetimeManager());

            unityContainer.RegisterType<BoerseLinkThreadHandler, BoerseLinkThreadHandlerHellrazor>(typeof(BoerseLinkThreadHandlerHellrazor).FullName);
            unityContainer.RegisterType<BoerseLinkThreadHandler, BoerseLinkThreadHandlerKristallprinz>(typeof(BoerseLinkThreadHandlerKristallprinz).FullName);
            unityContainer.RegisterType<BoerseLinkThreadHandler, BoerseLinkThreadHandlerSerienJk>(typeof(BoerseLinkThreadHandlerSerienJk).FullName);
            unityContainer.RegisterType<BoerseLinkThreadHandler, BoerseLinkThreadHandlerTardisCore>(typeof(BoerseLinkThreadHandlerTardisCore).FullName);
        }
    }
}