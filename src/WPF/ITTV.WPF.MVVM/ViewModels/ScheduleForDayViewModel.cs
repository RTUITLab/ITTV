using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Services;
using ITTV.WPF.MVVM.DTOs;

namespace ITTV.WPF.MVVM.ViewModels
{
    public class ScheduleForDayViewModel : ViewModelBase
    {
        public LinkedList<ScheduleLessonDto> LessonsForSelectedDay
        {
            get => _lessonsForSelectedDay;
            set
            {
                if (Equals(value, _lessonsForSelectedDay))
                    return;
                _lessonsForSelectedDay = value;
                
                OnPropertyChanged(nameof(HasClasses));
                OnPropertyChanged(nameof(LessonsForSelectedDay));
            }
        }
        
        private LinkedList<ScheduleLessonDto> _lessonsForSelectedDay = new();

        public bool HasClasses => _lessonsForSelectedDay.Any(x => x.Name != null);

        private readonly ScheduleManager _scheduleManager;
        private TimeTableDto _timeTableData;
        public ScheduleForDayViewModel(ScheduleManager scheduleManager)
        {
            _scheduleManager = scheduleManager;
        }

        public void SetTimeTableData(TimeTableDto timeTableDto)
        {
            _timeTableData = timeTableDto;
        }

        public override async Task Recalculate()
        {
            SetUnloaded();
            
            var lessons = await _scheduleManager.GetLessonsForDay(_timeTableData.GroupName, _timeTableData.SelectedScheduleTypeEnum);
            var lessonsDto = lessons
                .Select((x, i) => new ScheduleLessonDto(i + 1,
                    x.DetailLesson?.ClassRoom,
                x.DetailLesson?.Name,
                x.DetailLesson?.Teacher,
                x.DetailLesson?.Type,
                x.Time.Start, 
                x.Time.End));
            
            LessonsForSelectedDay = new LinkedList<ScheduleLessonDto>(lessonsDto);
            SetLoaded();
        }
    }
}