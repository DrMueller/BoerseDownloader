using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;
using MMU.BoerseDownloader.Common.Interfaces;
using MMU.BoerseDownloader.WpfUI.ViewModels.Shell;

namespace MMU.BoerseDownloader.WpfUI.App_Start
{
    internal class UiContainerInitializer : IUnityInitializer
    {
        public void InitializeContainer(IUnityContainer unityContainer)
        {
            InitializeViewModelsByReflection(unityContainer);
            InitializeHandlers(unityContainer);
        }

        private static IEnumerable<Type> GetAllViewModelTypes()
        {
            var viewModelBaseType = typeof(ViewModelBase);

            var assembly = typeof(ViewModelBase).GetTypeInfo().Assembly;
            var result = assembly.GetTypes().Where(f => viewModelBaseType.IsAssignableFrom(f) && !f.GetTypeInfo().IsAbstract);

            return result.ToList();
        }

        private void InitializeHandlers(IUnityContainer unityContainer)
        {
            unityContainer.RegisterType<Handlers.ExceptionHandling.IExceptionHandler, Handlers.ExceptionHandling.Implementation.ExceptionHandler>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<Handlers.ExceptionHandling.IExceptionHandlerConfiguration, Handlers.ExceptionHandling.Implementation.ExceptionHandlerConfiguration>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<Handlers.ExceptionHandling.IExceptionLogger, Handlers.ExceptionHandling.Implementation.ExceptionLogger>(new ContainerControlledLifetimeManager());

            unityContainer.RegisterType<Handlers.InformationHandling.IInformationHandler, Handlers.InformationHandling.Implementation.InformationHandler>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<Handlers.InformationHandling.IInformationHandlerConfiguration, Handlers.InformationHandling.Implementation.InformationHandlerConfiguration>(new ContainerControlledLifetimeManager());

            unityContainer.RegisterType<Handlers.NavigationHandling.INavigationHandler, Handlers.NavigationHandling.Implementation.NavigationHandler>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<Handlers.NavigationHandling.INavigationHandlerConfiguration, Handlers.NavigationHandling.Implementation.NavigationHandlerConfiguration>(new ContainerControlledLifetimeManager());
        }

        private static void InitializeViewModelsByReflection(IUnityContainer unityContainer)
        {
            var viewModelTypes = GetAllViewModelTypes();
            foreach (var vm in viewModelTypes)
            {
                unityContainer.RegisterType(vm);
            }
        }
    }
}