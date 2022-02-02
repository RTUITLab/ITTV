using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.DTOs;
using ITTV.WPF.MVVM.ViewModels;

namespace ITTV.WPF.MVVM.Commands.Schedule
{
    public class SelectGroupCommand : CommandBase
    {
        private readonly NavigationService<GroupsViewModel> _navigationService;

        public SelectGroupCommand(NavigationStore navigationStore, ScheduleManager scheduleManager,
            TimeTableDto timeTableData)
        {
            var groupTypesViewModel = new GroupsViewModel(scheduleManager, navigationStore);
            
            _navigationService = new NavigationService<GroupsViewModel>(navigationStore, groupTypesViewModel);
        }
        
        public override void Execute(object parameter)
        {
            _navigationService.Navigate();
        }
    }
}