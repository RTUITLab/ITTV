using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.ViewModels.Videos;

namespace ITTV.WPF.Commands.Videos
{
    public class VideoEndedCommand : CommandBase
    {
        private readonly VideoViewModel _videoViewModel;

        public VideoEndedCommand(VideoViewModel videoViewModel)
        {
            _videoViewModel = videoViewModel;
        }

        public override void Execute(object parameter)
        {
            _videoViewModel.EnableAction();
        }
    }
}