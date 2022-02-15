using ITTV.WPF.Abstractions.Base.Command;
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
            if (!_backgroundVideoViewModel.IsInactiveMode)
            {
                _backgroundVideoViewModel.SetNextVideo();
            }
        }
    }
}