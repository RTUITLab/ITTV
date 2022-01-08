using ITTV.WPF.MVVM.ViewModels;

namespace ITTV.WPF.MVVM.Commands.Videos
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