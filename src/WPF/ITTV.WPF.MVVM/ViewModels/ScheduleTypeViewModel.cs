using System;
using System.Collections.Generic;
using System.Linq;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Abstractions.Enums;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.Commands.Schedule;
using ITTV.WPF.MVVM.DTOs;

namespace ITTV.WPF.MVVM.ViewModels
{
    public class ScheduleTypeViewModel : ViewModelBase
    {
        public LinkedList<TimeTableQuestionDto> SupportedScheduleTypes
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
        
        private LinkedList<TimeTableQuestionDto> _supportedScheduleTypes;

        private TimeTableDto _timeTableData;
        private readonly ScheduleManager _scheduleManager;
        private readonly NavigationStore _navigationStore;
        public ScheduleTypeViewModel(NavigationStore navigationStore,
            ScheduleManager scheduleManager)
        {
            _navigationStore = navigationStore;
            _scheduleManager = scheduleManager;
        }

        public void SetTimeTableData(TimeTableDto timeTableDto)
        {
            _timeTableData = timeTableDto;
        }

        public void Recalculate()
        {
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

                    var command = new SelectScheduleTypeCommand(_navigationStore, timeTable);
                    return new TimeTableQuestionDto(displayName, command);
                });

            SupportedScheduleTypes = new LinkedList<TimeTableQuestionDto>(questions);
        }
    }
}