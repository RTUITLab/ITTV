using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.DTOs;
using ITTV.WPF.MVVM.ViewModels;

namespace ITTV.WPF.MVVM.Commands.Schedule
{
    public class SelectCourseCommand : CommandBase
    {
        private readonly GroupsViewModel _viewModel;
        private readonly NavigationService<GroupsViewModel> _navigationService;

        public SelectCourseCommand(NavigationStore navigationStore, ScheduleManager scheduleManager,
            TimeTableDto timeTableData)
        {
            var groupsViewModel = new GroupsViewModel(scheduleManager, navigationStore);

            groupsViewModel.SetTimeTableData(timeTableData);

            _navigationService = new NavigationService<GroupsViewModel>(navigationStore, groupsViewModel);
            _viewModel = groupsViewModel;
        }
        
        public override void Execute(object parameter)
        {
             _navigationService.Navigate();
        }
    }
}