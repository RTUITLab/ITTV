using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.Core.Models;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.ViewModels;

namespace ITTV.WPF.MVVM.Commands.EggVideos
{
    public class TryNavigateEggVideoCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly NavigationService<EggVideoViewModel> _eggVideoNavigationService;

        public TryNavigateEggVideoCommand(NavigationStore navigationStore, Settings settings)
        {
            _navigationStore = navigationStore;

            var eggVideoViewModel = new EggVideoViewModel(navigationStore, settings);
            _eggVideoNavigationService = new NavigationService<EggVideoViewModel>(navigationStore, eggVideoViewModel);
        }

        public override void Execute(object parameter)
        {
            if (_navigationStore.CurrentViewModel is not EggVideoViewModel)
                _eggVideoNavigationService.Navigate();
        }
    }
}