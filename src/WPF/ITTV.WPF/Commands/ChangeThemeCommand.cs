using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.Core.Services;

namespace ITTV.WPF.Commands
{
    public class ChangeThemeCommand : CommandBase
    {
        private readonly UserInterfaceManager _userInterfaceManager;

        public ChangeThemeCommand(UserInterfaceManager userInterfaceManager)
        {
            _userInterfaceManager = userInterfaceManager;
        }

        public override void Execute(object parameter)
        {
            _userInterfaceManager.ChangeTheme();
        }
    }
}