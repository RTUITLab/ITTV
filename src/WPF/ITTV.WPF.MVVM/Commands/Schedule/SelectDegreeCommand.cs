using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.DTOs;
using ITTV.WPF.MVVM.ViewModels;

namespace ITTV.WPF.MVVM.Commands.Schedule
{
    public class SelectDegreeCommand : CommandBase
    {
        private readonly NavigationService<CoursesViewModel> _navigationService;

        public SelectDegreeCommand(NavigationStore navigationStore, ScheduleManager scheduleManager,
            TimeTableDto timeTableData)
        {
            var courseViewModel = new CoursesViewModel(scheduleManager, navigationStore);
            courseViewModel.SetTimeTableData(timeTableData);
            
            _navigationService = new NavigationService<CoursesViewModel>(navigationStore, courseViewModel);
        }
        
        public override void Execute(object parameter)
        {
            _navigationService.Navigate();
        }
    }
}