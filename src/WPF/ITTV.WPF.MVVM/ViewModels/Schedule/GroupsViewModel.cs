using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.Commands.Schedule;
using ITTV.WPF.MVVM.DTOs;

namespace ITTV.WPF.MVVM.ViewModels.Schedule
{
    public class GroupsViewModel : ViewModelBase
    {
        public LinkedList<TimeTableQuestionDto> GroupsForCourse
        {
            get => _groupsForCourse;
            set
            {
                if (Equals(value, _groupsForCourse))
                    return;
                _groupsForCourse = value;
                
                OnPropertyChanged(nameof(GroupsForCourse));
            }
        }
        private LinkedList<TimeTableQuestionDto> _groupsForCourse;
        
        private readonly ScheduleManager _scheduleManager;
        private readonly NavigationStore _navigationStore;
        private TimeTableDto _timeTableData;

        public GroupsViewModel(ScheduleManager scheduleManager, NavigationStore navigationStore)
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
            if (!_timeTableData.Degree.HasValue ||
                !_timeTableData.CourseNumber.HasValue || 
                _timeTableData.GroupType is null)
                return;
            SetUnloaded();
            
            var groups = await _scheduleManager.GetGroups(_timeTableData.Degree.Value, _timeTableData.CourseNumber.Value, _timeTableData.GroupType);
            var groupsQuestions = groups.Select(x =>
            {
                var timeTableDto = new TimeTableDto();

                timeTableDto.Merge(_timeTableData);
                timeTableDto.SetGroupName(x);

                var command = new SelectGroupCommand(_navigationStore, _scheduleManager, timeTableDto);

                return new TimeTableQuestionDto(x, command);
            });

            GroupsForCourse = new LinkedList<TimeTableQuestionDto>(groupsQuestions);
            SetLoaded();
        }
    }
}