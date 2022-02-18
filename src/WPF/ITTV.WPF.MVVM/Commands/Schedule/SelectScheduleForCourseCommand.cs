using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.DTOs;
using ITTV.WPF.MVVM.ViewModels.Schedule;

namespace ITTV.WPF.MVVM.Commands.Schedule
{
    public class SelectScheduleForCourseCommand : CommandBase
    {
        private readonly NavigationService<ScheduleForCourseViewModel> _navigationService;

        public SelectScheduleForCourseCommand(NavigationStore navigationStore, 
            NotificationStore notificationStore,
            TimeTableDto timeTableData)
        {
            var groupViewModel = new ScheduleForCourseViewModel(timeTableData);
            
            _navigationService = new NavigationService<ScheduleForCourseViewModel>(navigationStore, groupViewModel);
        }
        
        public override void Execute(object parameter)
        {
            _navigationService.Navigate();
        }
    }
}