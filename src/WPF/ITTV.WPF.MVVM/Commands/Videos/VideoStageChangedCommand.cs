using ITTV.WPF.MVVM.ViewModels;

namespace ITTV.WPF.MVVM.Commands.Videos
{
    public class VideoStageChangedCommand : CommandBase
    {
        private readonly VideoViewModel _videoViewModel;

        public VideoStageChangedCommand(VideoViewModel videoViewModel)
        {
            _videoViewModel = videoViewModel;
        }

        public override void Execute(object parameter)
        {
            _videoViewModel.StartVideoAction();
        }
    }
}