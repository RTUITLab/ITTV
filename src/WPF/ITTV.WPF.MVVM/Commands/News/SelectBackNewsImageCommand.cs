using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.MVVM.ViewModels.News;

namespace ITTV.WPF.MVVM.Commands.News
{
    public class SelectBackNewsImageCommand : CommandBase
    {
        private readonly NewsElementViewModel _newsElementViewModel;
        public SelectBackNewsImageCommand(NewsElementViewModel newsElementViewModel)
        {
            _newsElementViewModel = newsElementViewModel;
        }
        public override void Execute(object parameter)
        {
            _newsElementViewModel.SelectBackImage();
        }
    }
}