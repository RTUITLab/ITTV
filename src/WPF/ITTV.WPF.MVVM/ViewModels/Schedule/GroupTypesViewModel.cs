using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.Commands.Schedule;
using ITTV.WPF.MVVM.DTOs;
using Serilog;

namespace ITTV.WPF.MVVM.ViewModels.Schedule
{
    public class GroupTypesViewModel : ViewModelBase
    {
        public ObservableCollection<TimeTableQuestionDto> SupportedGroupTypes
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
        private ObservableCollection<TimeTableQuestionDto> _supportedGroupTypes;
        
        private readonly ScheduleManager _scheduleManager;
        private readonly NavigationStore _navigationStore;
        private readonly NotificationStore _notificationStore;
        
        private TimeTableDto _timeTableData;

        public GroupTypesViewModel(ScheduleManager scheduleManager, 
            NavigationStore navigationStore,
            NotificationStore notificationStore)
        {
            _scheduleManager = scheduleManager;
            _navigationStore = navigationStore;
            _notificationStore = notificationStore;
        }

        public void SetTimeTableData(TimeTableDto tableDto)
        {
            _timeTableData = tableDto;
        }

        public override async void Recalculate()
        {
            try
            {
                if (!_timeTableData.Degree.HasValue || !_timeTableData.CourseNumber.HasValue)
                    return;

                SetUnloaded();

                var supportedGroupTypesApi =
                    await _scheduleManager.GetGroupTypesForCourse(_timeTableData.Degree.Value,
                        _timeTableData.CourseNumber.Value);
                var supportedGroupTypes = supportedGroupTypesApi.Select(x =>
                {
                    var timeTableDto = new TimeTableDto();

                    timeTableDto.Merge(_timeTableData);
                    timeTableDto.SetGroupType(x.Name);

                    var command = new SelectGroupTypesCommand(_navigationStore, _notificationStore, _scheduleManager, timeTableDto);

                    return new TimeTableQuestionDto(x.Name, command);
                });

                SupportedGroupTypes = new ObservableCollection<TimeTableQuestionDto>(supportedGroupTypes);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "Exception while syncing group types");

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