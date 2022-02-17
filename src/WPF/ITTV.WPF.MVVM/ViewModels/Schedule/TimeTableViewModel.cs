using System;
using System.Collections.ObjectModel;
using System.Linq;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Abstractions.Enums;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.Commands.Schedule;
using ITTV.WPF.MVVM.DTOs;
using Serilog;

namespace ITTV.WPF.MVVM.ViewModels.Schedule
{
    public class TimeTableViewModel : ViewModelBase
    {
        public ObservableCollection<TimeTableQuestionDto> SupportedDegrees
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
        
        private ObservableCollection<TimeTableQuestionDto> _supportedDegrees;

        public TimeTableViewModel(ScheduleManager scheduleManager,
            NavigationStore navigationStore,
             NotificationStore notificationStore)
        {
            try
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
                        
                        var command = new SelectDegreeCommand(navigationStore, scheduleManager, timeTableData, notificationStore);

                        return new TimeTableQuestionDto(x, command);
                    });

                SupportedDegrees = new ObservableCollection<TimeTableQuestionDto>(activeQuestions);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "Exception while getting degrees");

                var textException = e.InnerException?.Message ?? e.Message;
                notificationStore.AddNotification(textException);
            }
        }
    }
}