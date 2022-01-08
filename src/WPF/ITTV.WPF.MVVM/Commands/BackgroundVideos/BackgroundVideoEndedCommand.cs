using ITTV.WPF.MVVM.ViewModels;

namespace ITTV.WPF.MVVM.Commands.BackgroundVideos
{
    public class BackgroundVideoEndedCommand : CommandBase
    {
        private readonly BackgroundVideoViewModel _backgroundVideoViewModel;

        public BackgroundVideoEndedCommand(BackgroundVideoViewModel backgroundVideoViewModel)
        {
            _backgroundVideoViewModel = backgroundVideoViewModel;
        }

        public override void Execute(object parameter)
        {
            _backgroundVideoViewModel.SetNextVideo();
        }
    }
}