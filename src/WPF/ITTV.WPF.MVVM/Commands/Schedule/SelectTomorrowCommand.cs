using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.MVVM.ViewModels;

namespace ITTV.WPF.MVVM.Commands.Schedule
{
    public class SelectTomorrowCommand : CommandBase
    {
        private readonly ScheduleViewModel _scheduleViewModel;

        public SelectTomorrowCommand()
        {
            
        }
        
        public override void Execute(object parameter)
        {
            throw new System.NotImplementedException();
        }
    }
}