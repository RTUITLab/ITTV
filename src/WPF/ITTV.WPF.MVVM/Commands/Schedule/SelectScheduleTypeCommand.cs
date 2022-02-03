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
        private readonly ScheduleManager _scheduleManager;
        private readonly TimeTableDto _timeTableData;

        public SelectScheduleTypeCommand(NavigationStore navigationStore, 
            ScheduleManager scheduleManager,
            TimeTableDto timeTableData)
        {
            _navigationStore = navigationStore;
            _scheduleManager = scheduleManager;
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
                var scheduleViewModel = new ScheduleForDayViewModel(_scheduleManager);
                scheduleViewModel.SetTimeTableData(_timeTableData);
                
                var navigationService = new NavigationService<ScheduleForDayViewModel>(_navigationStore, scheduleViewModel);
                navigationService.Navigate();
                
                scheduleViewModel.Recalculate()
                    .ConfigureAwait(false);
            }
        }
    }
}