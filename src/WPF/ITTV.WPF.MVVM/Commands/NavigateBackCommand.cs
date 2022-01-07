using ITTV.WPF.MVVM.Stores;

namespace ITTV.WPF.MVVM.Commands
{
    public class NavigateBackCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

        public NavigateBackCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            _navigationStore.NavigateBack();
        }
    }
}