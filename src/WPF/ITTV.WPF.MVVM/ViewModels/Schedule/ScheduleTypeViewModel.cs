﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Abstractions.Enums;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.Commands.Schedule;
using ITTV.WPF.MVVM.DTOs;

namespace ITTV.WPF.MVVM.ViewModels.Schedule
{
    public class ScheduleTypeViewModel : ViewModelBase
    {
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
        public ScheduleTypeViewModel(ScheduleManager scheduleManager,
            NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _scheduleManager = scheduleManager;
        }

        public void SetTimeTableData(TimeTableDto timeTableDto)
        {
            _timeTableData = timeTableDto;
        }

        public override void Recalculate()
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
                        _scheduleManager, 
                        timeTable);
                    return new TimeTableQuestionDto(displayName, command);
                });

            SupportedScheduleTypes = new ObservableCollection<TimeTableQuestionDto>(questions);
            SetLoaded();
        }
    }
}