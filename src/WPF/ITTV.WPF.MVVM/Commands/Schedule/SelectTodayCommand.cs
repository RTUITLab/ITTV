using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.MVVM.ViewModels;

namespace ITTV.WPF.MVVM.Commands.Schedule
{
    public class SelectTodayCommand : CommandBase
    {
        private readonly ScheduleViewModel _scheduleViewModel;
        public SelectTodayCommand()
        {
            
        }
        public override void Execute(object parameter)
        {
            throw new System.NotImplementedException();
        }
    }
}