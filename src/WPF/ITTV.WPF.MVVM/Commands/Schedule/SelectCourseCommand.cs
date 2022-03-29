using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.DTOs;
using ITTV.WPF.MVVM.ViewModels.Schedule;

namespace ITTV.WPF.MVVM.Commands.Schedule
{
    public class SelectCourseCommand : CommandBase
    {
        private readonly NavigationService<GroupTypesViewModel> _navigationService;

        public SelectCourseCommand(NavigationStore navigationStore, 
            NotificationStore notificationStore, 
            ScheduleManager scheduleManager,
            TimeTableDto timeTableData)
        {
            var groupTypesViewModel = new GroupTypesViewModel(scheduleManager, 
                navigationStore, 
                notificationStore);

            groupTypesViewModel.SetTimeTableData(timeTableData);

            _navigationService = new NavigationService<GroupTypesViewModel>(navigationStore, groupTypesViewModel);
        }
        
        public override void Execute(object parameter)
        {
             _navigationService.Navigate();
        }
    }
}