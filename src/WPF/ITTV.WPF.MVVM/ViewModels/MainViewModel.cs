using ITTV.WPF.MVVM.Stores;

namespace ITTV.WPF.MVVM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

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
            FooterViewModel footerViewModel)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            
            FooterViewModel = footerViewModel;

        }
        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}