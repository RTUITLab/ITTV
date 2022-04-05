using System;
using System.Collections.ObjectModel;
using System.Linq;
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
    public class GroupsViewModel : ViewModelBase, INavigateBack
    {
        public ICommand NavigateBackCommand { get; }

        public ObservableCollection<TimeTableQuestionDto> GroupsForCourse
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
        private ObservableCollection<TimeTableQuestionDto> _groupsForCourse;
        
        private readonly ScheduleManager _scheduleManager;
        private readonly NavigationStore _navigationStore;
        private readonly NotificationStore _notificationStore;
        
        private TimeTableDto _timeTableData;

        public GroupsViewModel(ScheduleManager scheduleManager,
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

        public override async void Recalculate()
        {
            try
            {
                if (!_timeTableData.Degree.HasValue ||
                    !_timeTableData.CourseNumber.HasValue ||
                    _timeTableData.GroupType is null)
                    return;
                
                SetUnloaded();

                var groups = await _scheduleManager.GetGroups(_timeTableData.Degree.Value,
                    _timeTableData.CourseNumber.Value, _timeTableData.GroupType);
                var groupsQuestions = groups.Select(x =>
                {
                    var timeTableDto = new TimeTableDto();

                    timeTableDto.Merge(_timeTableData);
                    timeTableDto.SetGroupName(x);

                    var command = new SelectGroupCommand(_navigationStore, _notificationStore, _scheduleManager, timeTableDto);

                    return new TimeTableQuestionDto(x, command);
                });

                GroupsForCourse = new ObservableCollection<TimeTableQuestionDto>(groupsQuestions);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "Exception while syncing groups");

                var textException = e.InnerException?.Message ?? e.Message;
                _notificationStore.AddNotification(textException);
            }
            finally
            {
                SetLoaded();
            }
        }

    }
}