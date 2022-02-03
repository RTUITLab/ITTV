using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.MVVM.DTOs;

namespace ITTV.WPF.MVVM.ViewModels
{
    public class ScheduleForDayViewModel : ViewModelBase
    {
        private readonly TimeTableDto _timeTableData;
        public ScheduleForDayViewModel(TimeTableDto timeTableData)
        {
            _timeTableData = timeTableData;
        }
    }
}