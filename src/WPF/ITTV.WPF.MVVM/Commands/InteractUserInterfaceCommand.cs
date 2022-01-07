using ITTV.WPF.MVVM.Services;

namespace ITTV.WPF.MVVM.Commands
{
    public class InteractUserInterfaceCommand : CommandBase
    {
        private readonly UserInterfaceManager _userInterfaceManager;
        
        public InteractUserInterfaceCommand(UserInterfaceManager userInterfaceManager)
        {
            _userInterfaceManager = userInterfaceManager;
        }

        public override void Execute(object parameter)
        {
            _userInterfaceManager.UpdateInteractedTime();
        }
    }
}