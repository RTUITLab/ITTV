using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.DTOs;
using ITTV.WPF.MVVM.ViewModels;

namespace ITTV.WPF.MVVM.Commands.Schedule
{
    public class SelectGroupCommand : CommandBase
    {
        private readonly ScheduleTypeViewModel _scheduleTypeViewModel;
        private readonly NavigationService<ScheduleTypeViewModel> _navigationService;

        public SelectGroupCommand(NavigationStore navigationStore, ScheduleManager scheduleManager,
            TimeTableDto timeTableData)
        {
            var scheduleTypeViewModel = new ScheduleTypeViewModel(navigationStore, scheduleManager);
            scheduleTypeViewModel.SetTimeTableData(timeTableData);

            _scheduleTypeViewModel = scheduleTypeViewModel;
            _navigationService = new NavigationService<ScheduleTypeViewModel>(navigationStore, scheduleTypeViewModel);
        }
        
        public override void Execute(object parameter)
        {
            _navigationService.Navigate();
            _scheduleTypeViewModel.Recalculate();
        }
    }
}