using ITTV.WPF.MVVM.Stores;

namespace ITTV.WPF.MVVM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public FooterViewModel FooterViewModel { get; }
        public BackgroundVideoViewModel BackgroundVideoViewModel { get; }


        private readonly MenuViewModel _menuViewModel;

        private readonly NavigationStore _navigationStore;
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public MainViewModel(NavigationStore navigationStore,
            BackgroundVideoViewModel backgroundVideoViewModel,
            MenuViewModel menuViewModel, 
            FooterViewModel footerViewModel)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            
            
            BackgroundVideoViewModel = backgroundVideoViewModel;
            FooterViewModel = footerViewModel;

            _menuViewModel = menuViewModel;
        }
        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        

        
    }
}