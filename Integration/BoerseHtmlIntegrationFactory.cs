using Microsoft.Practices.Unity;

namespace MMU.BoerseDownloader.Integration
{
    public class BoerseHtmlIntegrationFactory
    {
        public BoerseHtmlIntegration Create()
        {
            var result = Common.Singletons.UnityContainerSingleton.Instance.Resolve<BoerseHtmlIntegration>();
            return result;
        }
    }
}