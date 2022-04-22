using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Models;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using Microsoft.Extensions.Options;

namespace ITTV.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public Settings Settings { get; }
        
        private readonly NavigationStore _navigationStore;
        private readonly UserInterfaceManager _userInterfaceManager;

        public ViewModelBase CurrentViewModel
        {
            get => _navigationStore.CurrentViewModel;
            private set
            {
                if (Equals(_navigationStore.CurrentViewModel, value))
                    return;

                _navigationStore.CurrentViewModel = value;
                
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }
        public FooterViewModel FooterViewModel { get; }
        public NotificationViewModel NotificationViewModel { get; }

        public MainViewModel(NavigationStore navigationStore,
            UserInterfaceManager userInterfaceManager,
            FooterViewModel footerViewModel,
            NotificationViewModel notificationViewModel, 
            IOptions<Settings> settings)
        {
            _navigationStore = navigationStore;
            _userInterfaceManager = userInterfaceManager;

            _userInterfaceManager.OnStateChangedToInactive += NavigateToInactiveMode;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            
            FooterViewModel = footerViewModel;
            NotificationViewModel = notificationViewModel;
            Settings = settings.Value;
        }

        private void NavigateToInactiveMode()
        {
            if (_navigationStore.IsInactiveMode)
                return;
            
            var navigated = _navigationStore.NavigateToInactiveMode();
            if (navigated)
            {
                _userInterfaceManager.ChangeTheme();
            }
        }
        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}