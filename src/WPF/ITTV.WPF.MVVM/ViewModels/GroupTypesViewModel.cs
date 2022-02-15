using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.Commands.Schedule;
using ITTV.WPF.MVVM.DTOs;

namespace ITTV.WPF.MVVM.ViewModels
{
    public class GroupTypesViewModel : ViewModelBase
    {
        public LinkedList<TimeTableQuestionDto> SupportedGroupTypes
        {
            get => _supportedGroupTypes;
            set
            {
                if (Equals(value, _supportedGroupTypes))
                    return;
                _supportedGroupTypes = value;
                
                OnPropertyChanged(nameof(SupportedGroupTypes));
            }
        }
        private LinkedList<TimeTableQuestionDto> _supportedGroupTypes;
        
        private readonly ScheduleManager _scheduleManager;
        private readonly NavigationStore _navigationStore;
        private TimeTableDto _timeTableData;

        public GroupTypesViewModel(ScheduleManager scheduleManager, 
            NavigationStore navigationStore)
        {
            _scheduleManager = scheduleManager;
            _navigationStore = navigationStore;
        }

        public void SetTimeTableData(TimeTableDto tableDto)
        {
            _timeTableData = tableDto;
        }

        public override async Task Recalculate()
        {
            if (!_timeTableData.Degree.HasValue || !_timeTableData.CourseNumber.HasValue)
                return;
            
            SetUnloaded();
            
            var supportedGroupTypesApi = await _scheduleManager.GetGroupTypesForCourse(_timeTableData.Degree.Value, _timeTableData.CourseNumber.Value);
            var supportedGroupTypes = supportedGroupTypesApi.Select(x =>
            {
                var timeTableDto = new TimeTableDto();

                timeTableDto.Merge(_timeTableData);
                timeTableDto.SetGroupType(x.Name);

                var command = new SelectGroupTypesCommand(_navigationStore, _scheduleManager, timeTableDto);

                return new TimeTableQuestionDto(x.Name, command);
            });

            SupportedGroupTypes = new LinkedList<TimeTableQuestionDto>(supportedGroupTypes);
            SetLoaded();
        }
    }
}