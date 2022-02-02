using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.MVVM.DTOs;

namespace ITTV.WPF.MVVM.ViewModels
{
    public class ScheduleViewModel : ViewModelBase
    {
        private readonly TimeTableDto _timeTableData;
        public ScheduleViewModel(TimeTableDto timeTableData)
        {
            _timeTableData = timeTableData;
        }
        
    }
}