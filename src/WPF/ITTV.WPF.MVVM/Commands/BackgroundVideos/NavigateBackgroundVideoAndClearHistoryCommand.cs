using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.Core.Stores;

namespace ITTV.WPF.MVVM.Commands.BackgroundVideos
{
    public class NavigateBackgroundVideoAndClearHistoryCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

        public NavigateBackgroundVideoAndClearHistoryCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            _navigationStore.NavigateToInactiveMode();
        }
    }
}