using Microsoft.Practices.Unity;

namespace MMU.BoerseDownloader.Common.Interfaces
{
    public interface IUnityInitializer
    {
        void InitializeContainer(IUnityContainer unityContainer);
    }
}