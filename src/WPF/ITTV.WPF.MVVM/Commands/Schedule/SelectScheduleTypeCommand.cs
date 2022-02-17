using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.Abstractions.Enums;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.DTOs;
using ITTV.WPF.MVVM.ViewModels;
using ITTV.WPF.MVVM.ViewModels.Schedule;

namespace ITTV.WPF.MVVM.Commands.Schedule
{
    public class SelectScheduleTypeCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly ScheduleManager _scheduleManager;
        private readonly TimeTableDto _timeTableData;
        private readonly NotificationStore _notificationStore;

        public SelectScheduleTypeCommand(NavigationStore navigationStore,
            NotificationStore notificationStore,
            ScheduleManager scheduleManager,
            TimeTableDto timeTableData)
        {
            _navigationStore = navigationStore;
            _notificationStore = notificationStore;
            _scheduleManager = scheduleManager;
            _timeTableData = timeTableData;
        }
        public override void Execute(object parameter)
        {
            if (_timeTableData.SelectedScheduleTypeEnum == SelectedScheduleTypeEnum.FullSchedule)
            {
                var scheduleViewModel = new ScheduleViewModel(_scheduleManager, _notificationStore);
                scheduleViewModel.SetTimeTable(_timeTableData);
                
                var navigationService = new NavigationService<ScheduleViewModel>(_navigationStore, scheduleViewModel);
                navigationService.Navigate();
            }
            else
            {
                var scheduleViewModel = new ScheduleForDayViewModel(_scheduleManager, _notificationStore);
                scheduleViewModel.SetTimeTableData(_timeTableData);
                
                var navigationService = new NavigationService<ScheduleForDayViewModel>(_navigationStore, scheduleViewModel);
                navigationService.Navigate();
            }
        }
    }
}