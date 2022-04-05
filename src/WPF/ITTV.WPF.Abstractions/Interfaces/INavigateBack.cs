using System.Windows.Input;

namespace ITTV.WPF.Abstractions.Interfaces
{
    public interface INavigateBack
    {
        ICommand NavigateBackCommand { get; }
    }
}