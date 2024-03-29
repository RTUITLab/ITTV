﻿using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.DTOs;
using ITTV.WPF.ViewModels.Schedule;

namespace ITTV.WPF.Commands.Schedule
{
    public class SelectDegreeCommand : CommandBase
    {
        private readonly NavigationService<CoursesViewModel> _navigationService;

        public SelectDegreeCommand(NavigationStore navigationStore, ScheduleManager scheduleManager,
            TimeTableDto timeTableData, NotificationStore notificationStore)
        {
            var courseViewModel = new CoursesViewModel(scheduleManager, navigationStore, notificationStore);
            courseViewModel.SetTimeTableData(timeTableData);
            
            _navigationService = new NavigationService<CoursesViewModel>(navigationStore, courseViewModel);
        }
        
        public override void Execute(object parameter)
        {
            _navigationService.Navigate();
        }
    }
}