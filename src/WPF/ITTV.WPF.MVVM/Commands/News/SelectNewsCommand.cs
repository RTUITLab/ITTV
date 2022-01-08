using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.ViewModels;

namespace ITTV.WPF.MVVM.Commands.News
{
    public class SelectNewsCommand : CommandBase
    {
        private readonly NavigationService<NewsElementViewModel> _navigationService;
        public SelectNewsCommand(NewsElementViewModel videoViewModel,
            NavigationStore navigationStore)
        {
            _navigationService = new NavigationService<NewsElementViewModel>(navigationStore, videoViewModel);
        }
        
        public override void Execute(object parameter)
        {
            _navigationService.Navigate();
        }
    }
}