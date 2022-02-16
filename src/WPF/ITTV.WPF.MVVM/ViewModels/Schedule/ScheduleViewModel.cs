using System;
using System.Collections.ObjectModel;
using System.Linq;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Services.ApiClient.Requests.GetScheduleForGroup;
using ITTV.WPF.MVVM.DTOs;

namespace ITTV.WPF.MVVM.ViewModels.Schedule
{
    public class ScheduleViewModel : ViewModelBase
    {
        private readonly ScheduleManager _scheduleManager;
        private TimeTableDto _timeTableData;
        public ScheduleViewModel(ScheduleManager scheduleManager)
        {
            _scheduleManager = scheduleManager;
        }

        public ObservableCollection<OverviewScheduleForDay> OverviewScheduleForDays
        {
            get => _overviewScheduleForDays;
            set
            {
                if (Equals(value, _overviewScheduleForDays))
                    return;
                _overviewScheduleForDays = value;
                
                OnPropertyChanged(nameof(OverviewScheduleForDays));
            }
        }

        private ObservableCollection<OverviewScheduleForDay> _overviewScheduleForDays = new();

        public void SetTimeTable(TimeTableDto timeTableDto)
        {
            _timeTableData = timeTableDto;
        }
        public override async void Recalculate()
        {
            SetUnloaded();
            
            var schedule = await _scheduleManager.GetFullSchedule(_timeTableData.GroupName);

            var days = new[]
            {
                "Понедельник",
                "Вторник",
                "Среда",
                "Четверг",
                "Пятница",
                "Суббота"
            };

            var lessons = days.Select(x => GetOverviewScheduleForDay(schedule, x));

            OverviewScheduleForDays = new ObservableCollection<OverviewScheduleForDay>(lessons);
            SetLoaded();
        }

        private OverviewScheduleForDay GetOverviewScheduleForDay(ApiFullScheduleResponse schedule, string day)
        {
            var (firstWeekLessons, secondWeekLessons) = day switch
            {
                "Понедельник" => (schedule.FirstWeek.Monday, schedule.SecondWeek.Monday),
                "Вторник" => (schedule.FirstWeek.Tuesday, schedule.SecondWeek.Tuesday),
                "Среда" => (schedule.FirstWeek.Wednesday, schedule.SecondWeek.Wednesday),
                "Четверг" => (schedule.FirstWeek.Thursday, schedule.SecondWeek.Thursday),
                "Пятница" => (schedule.FirstWeek.Friday, schedule.SecondWeek.Friday),
                "Суббота" => (schedule.FirstWeek.Saturday, schedule.SecondWeek.Saturday),
                _ => throw new ArgumentException("Unsupported day of week")
            };

            return new OverviewScheduleForDay(day, firstWeekLessons.Select((x, i) =>
            {
                var lessonFromSecondWeek = secondWeekLessons[i];
                return new OverviewScheduleForLesson(i + 1,
                    new ScheduleLessonDto(i + 1,
                        x.DetailLesson?.ClassRoom,
                        x.DetailLesson?.Name,
                        x.DetailLesson?.Teacher,
                        x.DetailLesson?.Type,
                        x.Time?.Start,
                        x.Time?.End),
                    new ScheduleLessonDto(i + 1,
                        lessonFromSecondWeek.DetailLesson?.ClassRoom,
                        lessonFromSecondWeek.DetailLesson?.Name,
                        lessonFromSecondWeek.DetailLesson?.Teacher,
                        lessonFromSecondWeek.DetailLesson?.Type,
                        lessonFromSecondWeek.Time?.Start,
                        lessonFromSecondWeek.Time?.End));
            }).ToArray());
        }

    }
}