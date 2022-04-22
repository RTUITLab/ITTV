using System;
using System.Collections.ObjectModel;
using System.Linq;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.DTOs;
using Serilog;

namespace ITTV.WPF.ViewModels.Schedule
{
    public class ScheduleForDayViewModel : ViewModelBase
    {
        public ObservableCollection<ScheduleLessonDto> LessonsForSelectedDay
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
        
        private ObservableCollection<ScheduleLessonDto> _lessonsForSelectedDay = new();

        public bool HasClasses => _lessonsForSelectedDay.Any(x => x.Name != null);

        private readonly ScheduleManager _scheduleManager;
        private readonly NotificationStore _notificationStore;
        private TimeTableDto _timeTableData;
        public ScheduleForDayViewModel(ScheduleManager scheduleManager,
            NotificationStore notificationStore)
        {
            _scheduleManager = scheduleManager;
            _notificationStore = notificationStore;
        }

        public void SetTimeTableData(TimeTableDto timeTableDto)
        {
            _timeTableData = timeTableDto;
        }

        public override async void Recalculate()
        {
            try
            {
                SetUnloaded();

                var lessons = await _scheduleManager.GetLessonsForDay(_timeTableData.GroupName,
                    _timeTableData.SelectedScheduleTypeEnum);
                var lessonsDto = lessons
                    .Select((x, i) => new ScheduleLessonDto(i + 1,
                        x.DetailLesson?.ClassRoom,
                        x.DetailLesson?.Name,
                        x.DetailLesson?.Teacher,
                        x.DetailLesson?.Type,
                        x.Time.Start,
                        x.Time.End));

                LessonsForSelectedDay = new ObservableCollection<ScheduleLessonDto>(lessonsDto);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "Exception while syncing schedule for day {@0}", _timeTableData);

                _notificationStore.AddNotification(e);
            }
            finally
            {
                SetLoaded();
            }
        }
    }
}