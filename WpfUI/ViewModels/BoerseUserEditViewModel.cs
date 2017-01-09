using MMU.BoerseDownloader.Logics;
using MMU.BoerseDownloader.Model;
using MMU.BoerseDownloader.WpfUI.Commands;
using MMU.BoerseDownloader.WpfUI.Handlers.ExceptionHandling;
using MMU.BoerseDownloader.WpfUI.Handlers.NavigationHandling;
using MMU.BoerseDownloader.WpfUI.Models;
using MMU.BoerseDownloader.WpfUI.ViewModels.Shell;

namespace MMU.BoerseDownloader.WpfUI.ViewModels
{
    public class BoerseUserEditViewModel : ViewModelBase
    {
        private readonly BoerseUserConfigurationLogic _boerseUserConfigurationLogic;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly INavigationHandler _navigationHandler;
        private BoerseUser _boerseUser;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "By Design")]
        public BoerseUserEditViewModel(IExceptionHandler exceptionHandler, INavigationHandler navigationHandler, BoerseUserConfigurationLogic boerseUserConfigurationLogic)
        {
            _exceptionHandler = exceptionHandler;
            _navigationHandler = navigationHandler;
            _boerseUserConfigurationLogic = boerseUserConfigurationLogic;
            LoadUser();
        }

        public ViewModelCommand CancelEdit
        {
            get
            {
                return new ViewModelCommand("Cancel", new RelayCommand(() =>
                {
                    _navigationHandler.NavigateTo<DownloadContextsOverviewViewModel>(ViewModelParameterCollection.Empty);
                }));
            }
        }

        public override string DisplayName => "Configure Boerse-User";

        public string LoginName
        {
            get
            {
                return _boerseUser.LoginName;
            }
            set
            {
                _boerseUser.LoginName = value;
            }
        }

        public string Password
        {
            get
            {
                return _boerseUser.Password;
            }
            set
            {
                _boerseUser.Password = value;
            }
        }

        public ViewModelCommand SaveBoerseUser
        {
            get
            {
                return new ViewModelCommand("Save", new RelayCommand(() =>
                {
                    _exceptionHandler.HandledAction(() =>
                    {
                        _boerseUserConfigurationLogic.Save(_boerseUser);
                        _navigationHandler.NavigateTo<DownloadContextsOverviewViewModel>(ViewModelParameterCollection.Empty);
                    });
                }, CanSaveUser));
            }
        }

        private bool CanSaveUser()
        {
            return !string.IsNullOrEmpty(LoginName) && !string.IsNullOrEmpty(Password);
        }

        private void LoadUser()
        {
            _boerseUser = _boerseUserConfigurationLogic.Load();
            LoginName = _boerseUser.LoginName;
            Password = _boerseUser.Password;
        }
    }
}