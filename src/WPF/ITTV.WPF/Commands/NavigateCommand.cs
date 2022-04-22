using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Services;

namespace ITTV.WPF.Commands
{
    public class NavigateCommand<TViewModel> : CommandBase 
        where TViewModel : ViewModelBase
    {
        private readonly NavigationService<TViewModel> _navigationService;

        public NavigateCommand(NavigationService<TViewModel> navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {
            _navigationService.Navigate();
        }
    }
}