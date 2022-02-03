using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.Abstractions.Enums;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.DTOs;
using ITTV.WPF.MVVM.ViewModels;

namespace ITTV.WPF.MVVM.Commands.Schedule
{
    public class SelectScheduleTypeCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly TimeTableDto _timeTableData;

        public SelectScheduleTypeCommand(NavigationStore navigationStore, 
            TimeTableDto timeTableData)
        {
            _navigationStore = navigationStore;
            _timeTableData = timeTableData;
        }
        public override void Execute(object parameter)
        {
            if (_timeTableData.SelectedScheduleTypeEnum == SelectedScheduleTypeEnum.FullSchedule)
            {
                var scheduleViewModel = new ScheduleViewModel(_timeTableData);
                var navigationService = new NavigationService<ScheduleViewModel>(_navigationStore, scheduleViewModel);
                navigationService.Navigate();
            }
            else
            {
                var scheduleViewModel = new ScheduleForDayViewModel(_timeTableData);
                var navigationService = new NavigationService<ScheduleForDayViewModel>(_navigationStore, scheduleViewModel);
                navigationService.Navigate();
            }
        }
    }
}