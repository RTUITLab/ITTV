﻿using ITTV.WPF.Abstractions.Base.Command;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.DTOs;
using ITTV.WPF.MVVM.ViewModels.Schedule;

namespace ITTV.WPF.MVVM.Commands.Schedule
{
    public class SelectGroupTypesCommand : CommandBase
    {
        private readonly NavigationService<GroupsViewModel> _navigationService;

        public SelectGroupTypesCommand(NavigationStore navigationStore, 
            NotificationStore notificationStore,
            ScheduleManager scheduleManager,
            TimeTableDto timeTableData)
        {
            var groupViewModel = new GroupsViewModel(scheduleManager, navigationStore, notificationStore);
            groupViewModel.SetTimeTableData(timeTableData);
            
            _navigationService = new NavigationService<GroupsViewModel>(navigationStore, groupViewModel);
        }
        
        public override void Execute(object parameter)
        {
            _navigationService.Navigate();
        }
    }
}