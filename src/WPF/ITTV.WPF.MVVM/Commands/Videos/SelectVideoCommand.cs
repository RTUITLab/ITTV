using ITTV.WPF.MVVM.Services;
using ITTV.WPF.MVVM.Stores;
using ITTV.WPF.MVVM.ViewModels;

namespace ITTV.WPF.MVVM.Commands.Videos
{
    public class SelectVideoCommand : CommandBase
    {
        private readonly NavigationService<VideoViewModel> _navigationService;
        public SelectVideoCommand(VideoViewModel videoViewModel,
            NavigationStore navigationStore)
        {
            _navigationService = new NavigationService<VideoViewModel>(navigationStore, videoViewModel);
        }
        public override void Execute(object parameter)
        {
            _navigationService.Navigate();
        }
    }
}