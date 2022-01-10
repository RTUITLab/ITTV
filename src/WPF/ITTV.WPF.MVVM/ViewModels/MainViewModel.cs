using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;

namespace ITTV.WPF.MVVM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
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
        
        public MainViewModel(NavigationStore navigationStore,
            UserInterfaceManager userInterfaceManager,
            FooterViewModel footerViewModel)
        {
            _navigationStore = navigationStore;
            _userInterfaceManager = userInterfaceManager;

            _userInterfaceManager.OnStateChangedToInactive += NavigateToInactiveMode;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            
            FooterViewModel = footerViewModel;
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