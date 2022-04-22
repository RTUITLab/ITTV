using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.ViewModels.Videos;

namespace ITTV.WPF.Commands.Videos
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