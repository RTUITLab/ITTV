using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.ViewModels;

namespace ITTV.WPF.Commands.BackgroundVideos
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
            if (!_backgroundVideoViewModel.IsInactiveWorkMode)
            {
                _backgroundVideoViewModel.SetNextVideo();
            }
        }
    }
}