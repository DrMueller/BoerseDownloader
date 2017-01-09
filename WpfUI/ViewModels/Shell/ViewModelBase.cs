using System;
using MMU.BoerseDownloader.WpfUI.Models;
using PropertyChanged;

namespace MMU.BoerseDownloader.WpfUI.ViewModels.Shell
{
    [ImplementPropertyChanged]
    public abstract class ViewModelBase
    {
        public abstract string DisplayName { get; }

        public virtual void InjectParameters(ViewModelParameterCollection viewModelParameterCollection)
        {
            throw new NotImplementedException($"ViewModel {GetType().Name} does not accept Parameters.");
        }
    }
}