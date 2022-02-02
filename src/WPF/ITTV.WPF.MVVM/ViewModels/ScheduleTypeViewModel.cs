using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.MVVM.Commands.Schedule;
using ITTV.WPF.MVVM.DTOs;

namespace ITTV.WPF.MVVM.ViewModels
{
    public class ScheduleTypeViewModel : ViewModelBase
    {
        public CommandBase SelectTodayCommand { get; }
        public CommandBase SelectTomorrowCommand { get; }
        public CommandBase SelectFullScheduleCommand { get; }

        private readonly TimeTableDto _timeTableData;
        
        public ScheduleTypeViewModel(TimeTableDto timeTableData)
        {
            _timeTableData = timeTableData;

            SelectTodayCommand = new SelectTodayCommand();
            SelectTomorrowCommand = new SelectTomorrowCommand();
            SelectFullScheduleCommand = new SelectFullScheduleCommand();
        }
    }
}