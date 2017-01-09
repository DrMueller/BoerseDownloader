namespace MMU.BoerseDownloader.WpfUI.App_Start
{
    internal static class Bootstrapper
    {
        internal static void Initialize()
        {
            InitializeUnityContainer();
            ViewModelMappingInitializer.Initialize();
        }

        private static void InitializeUnityContainer()
        {
            Common.Singletons.UnityContainerSingleton.Initialize();
        }
    }
}