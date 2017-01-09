using Microsoft.Practices.Unity;
using MMU.BoerseDownloader.WpfUI.Models;

namespace MMU.BoerseDownloader.WpfUI.ViewModels.Shell
{
    public class ViewModelFactory
    {
        private readonly IUnityContainer _unityContainer;

        public ViewModelFactory(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        internal ViewModelBase CreateViewModel<T>(ViewModelParameterCollection viewModelParameterCollection)
            where T : ViewModelBase
        {
            var result = _unityContainer.Resolve<T>();
            if (viewModelParameterCollection.HasValue)
            {
                result.InjectParameters(viewModelParameterCollection);
            }

            return result;
        }
    }
}