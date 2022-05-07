using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.DTOs;
using ITTV.WPF.ViewModels.Schedule;

namespace ITTV.WPF.Commands.Schedule
{
    public class SelectScheduleForCourseCommand : CommandBase
    {
        private readonly NavigationService<ScheduleForCourseViewModel> _navigationService;

        public SelectScheduleForCourseCommand(NavigationStore navigationStore, 
            TimeTableDto timeTableData)
        {
            var groupViewModel = new ScheduleForCourseViewModel(timeTableData);
            groupViewModel.Recalculate();
            
            _navigationService = new NavigationService<ScheduleForCourseViewModel>(navigationStore, groupViewModel);
        }
        
        public override void Execute(object parameter)
        {
            _navigationService.Navigate();
        }
    }
}