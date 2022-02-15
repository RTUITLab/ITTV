using System;
using System.Collections.Generic;
using System.Linq;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Abstractions.Enums;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.Commands.Schedule;
using ITTV.WPF.MVVM.DTOs;

namespace ITTV.WPF.MVVM.ViewModels.Schedule
{
    public class TimeTableViewModel : ViewModelBase
    {
        public LinkedList<TimeTableQuestionDto> SupportedDegrees
        {
            get => _supportedDegrees;
            set
            {
                if (Equals(value, _supportedDegrees))
                    return;
                _supportedDegrees = value;
                
                OnPropertyChanged(nameof(SupportedDegrees));
            }
        }
        
        private LinkedList<TimeTableQuestionDto> _supportedDegrees;

        public TimeTableViewModel(ScheduleManager scheduleManager,
            NavigationStore navigationStore)
        {
            var activeQuestions = scheduleManager.GetSupportedDegrees()
                .Select(x =>
                {
                    var degree = x switch
                    {
                        "Бакалавриат" => DegreeEnum.Bachelor,
                        "Магистратура" => DegreeEnum.Master,
                        _ => throw new ArgumentException("Unsupported degree")
                    };

                    var timeTableData = new TimeTableDto();
                    timeTableData.SetDegree(degree);
                    
                    var command = new SelectDegreeCommand(navigationStore, scheduleManager, timeTableData);

                    return new TimeTableQuestionDto(x, command);
                });

            SupportedDegrees = new LinkedList<TimeTableQuestionDto>(activeQuestions);
        }
    }
}