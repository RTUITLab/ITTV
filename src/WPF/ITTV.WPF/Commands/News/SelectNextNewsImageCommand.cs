using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.ViewModels.News;

namespace ITTV.WPF.Commands.News
{
    public class SelectNextNewsImageCommand : CommandBase
    {
        private readonly NewsElementViewModel _newsElementViewModel;
        public SelectNextNewsImageCommand(NewsElementViewModel newsElementViewModel)
        {
            _newsElementViewModel = newsElementViewModel;
        }
        
        public override void Execute(object parameter)
        {
            _newsElementViewModel.SelectNextImage();
        }
    }
}