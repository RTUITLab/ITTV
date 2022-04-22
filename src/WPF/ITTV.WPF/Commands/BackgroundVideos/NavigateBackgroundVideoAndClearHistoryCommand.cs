using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;

namespace ITTV.WPF.Commands.BackgroundVideos
{
    public class NavigateBackgroundVideoAndClearHistoryCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly UserInterfaceManager _userInterfaceManager;

        public NavigateBackgroundVideoAndClearHistoryCommand(NavigationStore navigationStore, UserInterfaceManager userInterfaceManager)
        {
            _navigationStore = navigationStore;
            _userInterfaceManager = userInterfaceManager;
        }

        public override void Execute(object parameter)
        {
            _navigationStore.NavigateToInactiveMode();
            _userInterfaceManager.ChangeTheme();
        }
    }
}