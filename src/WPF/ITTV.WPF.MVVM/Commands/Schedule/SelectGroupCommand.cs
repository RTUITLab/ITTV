using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.DTOs;
using ITTV.WPF.MVVM.ViewModels.Schedule;

namespace ITTV.WPF.MVVM.Commands.Schedule
{
    public class SelectGroupCommand : CommandBase
    {
        private readonly NavigationService<ScheduleTypeViewModel> _navigationService;

        public SelectGroupCommand(NavigationStore navigationStore, 
            NotificationStore notificationStore,
            ScheduleManager scheduleManager,
            TimeTableDto timeTableData)
        {
            var groupsViewModel = new ScheduleTypeViewModel(scheduleManager, navigationStore, notificationStore);
            groupsViewModel.SetTimeTableData(timeTableData);

            _navigationService = new NavigationService<ScheduleTypeViewModel>(navigationStore, groupsViewModel);
        }
        
        public override void Execute(object parameter)
        {
            _navigationService.Navigate();
        }
    }
}