using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Stores;

namespace ITTV.WPF.Core.Services
{
    public class NavigationService<TViewModel> where TViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly TViewModel _createdViewModel;

        public NavigationService(NavigationStore navigationStore, TViewModel createdViewModel)
        {
            _navigationStore = navigationStore;
            _createdViewModel = createdViewModel;
        }

        public void Navigate()
        {
            _navigationStore.CurrentViewModel = _createdViewModel;

            _createdViewModel.Recalculate()
                .ConfigureAwait(false);
        }
    }
}