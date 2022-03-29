using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Abstractions.Interfaces;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.Commands;
using ITTV.WPF.MVVM.Commands.Schedule;
using ITTV.WPF.MVVM.DTOs;
using Serilog;

namespace ITTV.WPF.MVVM.ViewModels.Schedule
{
    public class CoursesViewModel : ViewModelBase, INavigateBack
    {
        public ICommand NavigateBackCommand { get; }

        public ObservableCollection<TimeTableQuestionDto> SupportedCourses
        {
            get => _supportedCourses;
            set
            {
                if (Equals(value, _supportedCourses))
                    return;

                if (_supportedCourses != null && value != null)
                {
                    if(_supportedCourses.SequenceEqual(value))
                        return;
                }
                _supportedCourses = value;
                
                OnPropertyChanged(nameof(SupportedCourses));
            }
        }
        
        private ObservableCollection<TimeTableQuestionDto> _supportedCourses;
        
        private readonly ScheduleManager _scheduleManager;
        private readonly NavigationStore _navigationStore;
        private readonly NotificationStore _notificationStore;
        private TimeTableDto _timeTableData;
        public CoursesViewModel(ScheduleManager scheduleManager,
            NavigationStore navigationStore,
            NotificationStore notificationStore)
        {
            _scheduleManager = scheduleManager;
            _navigationStore = navigationStore;
            _notificationStore = notificationStore;

            NavigateBackCommand = new NavigateBackCommand(_navigationStore);
        }

        public void SetTimeTableData(TimeTableDto tableDto)
        {
            _timeTableData = tableDto;
        }

        public override void Recalculate()
        {
            try
            {
                if (!_timeTableData.Degree.HasValue)
                    return;
                
                SetUnloaded();
                
                var supportedCourses = _scheduleManager.GetSupportedCoursesForDegree(_timeTableData.Degree.Value);
                var supportedCoursesQuestions = supportedCourses.Select(x =>
                {
                    var title = $"{x}-й курс";
                    var timeTableDto = new TimeTableDto();

                    timeTableDto.Merge(_timeTableData);
                    timeTableDto.SetCourseNumber(x);

                    var command = new SelectCourseCommand(_navigationStore, _notificationStore, _scheduleManager, timeTableDto);

                    return new TimeTableQuestionDto(title, command);
                });

                SupportedCourses = new ObservableCollection<TimeTableQuestionDto>(supportedCoursesQuestions);
                
                SetLoaded();
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "Exception while syncing courses");

                var textException = e.InnerException?.Message ?? e.Message;
                _notificationStore.AddNotification(textException);
            }
        }

    }
}