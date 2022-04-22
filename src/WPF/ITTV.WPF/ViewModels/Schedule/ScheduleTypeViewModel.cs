using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Abstractions.Enums;
using ITTV.WPF.Abstractions.Interfaces;
using ITTV.WPF.Commands;
using ITTV.WPF.Commands.Schedule;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.DTOs;
using Serilog;

namespace ITTV.WPF.ViewModels.Schedule
{
    public class ScheduleTypeViewModel : ViewModelBase, INavigateBack
    {
        public ICommand NavigateBackCommand { get; }

        public ObservableCollection<TimeTableQuestionDto> SupportedScheduleTypes
        {
            get => _supportedScheduleTypes;
            set
            {
                if (Equals(value, _supportedScheduleTypes))
                    return;
                _supportedScheduleTypes = value;
                
                OnPropertyChanged(nameof(SupportedScheduleTypes));
            }
        }
        
        private ObservableCollection<TimeTableQuestionDto> _supportedScheduleTypes;

        private TimeTableDto _timeTableData;
        private readonly ScheduleManager _scheduleManager;
        private readonly NavigationStore _navigationStore;
        private readonly NotificationStore _notificationStore;
        public ScheduleTypeViewModel(ScheduleManager scheduleManager,
            NavigationStore navigationStore,
            NotificationStore notificationStore)
        {
            _navigationStore = navigationStore;
            _notificationStore = notificationStore;
            _scheduleManager = scheduleManager;

            NavigateBackCommand = new NavigateBackCommand(_navigationStore);
        }

        public void SetTimeTableData(TimeTableDto timeTableDto)
        {
            _timeTableData = timeTableDto;
        }

        public override void Recalculate()
        {
            try
            {
                SetUnloaded();

                var questions = _scheduleManager.GetSupportedScheduleTypes()
                    .Select(x =>
                    {
                        var displayName = x switch
                        {
                            SelectedScheduleTypeEnum.Today => "Сегодня",
                            SelectedScheduleTypeEnum.Tomorrow => "Завтра",
                            SelectedScheduleTypeEnum.FullSchedule => "Общее",
                            _ => throw new ArgumentOutOfRangeException(nameof(x), x, "Unsupported schedule type")
                        };

                        var timeTable = new TimeTableDto();
                        timeTable.Merge(_timeTableData);
                        timeTable.SetSelectedScheduleEnum(x);

                        var command = new SelectScheduleTypeCommand(_navigationStore,
                            _notificationStore,
                            _scheduleManager,
                            timeTable);
                        return new TimeTableQuestionDto(displayName, command);
                    });

                SupportedScheduleTypes = new ObservableCollection<TimeTableQuestionDto>(questions);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "Exception while syncing schedule type {@0}", _timeTableData);

                _notificationStore.AddNotification(e);
            }
            finally
            {
                SetLoaded();
            }
        }

    }
}