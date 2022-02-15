﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.Commands.Schedule;
using ITTV.WPF.MVVM.DTOs;

namespace ITTV.WPF.MVVM.ViewModels.Schedule
{
    public class CoursesViewModel : ViewModelBase
    {
        public LinkedList<TimeTableQuestionDto> SupportedCourses
        {
            get => _supportedCourses;
            set
            {
                if (Equals(value, _supportedCourses))
                    return;
                _supportedCourses = value;
                
                OnPropertyChanged(nameof(SupportedCourses));
            }
        }
        
        private LinkedList<TimeTableQuestionDto> _supportedCourses;
        
        private readonly ScheduleManager _scheduleManager;
        private readonly NavigationStore _navigationStore;
        private TimeTableDto _timeTableData;
        public CoursesViewModel(ScheduleManager scheduleManager,
            NavigationStore navigationStore)
        {
            _scheduleManager = scheduleManager;
            _navigationStore = navigationStore;
        }

        public void SetTimeTableData(TimeTableDto tableDto)
        {
            _timeTableData = tableDto;
        }

        public override Task Recalculate()
        {
            if (!_timeTableData.Degree.HasValue)
                return Task.CompletedTask;
            
            SetUnloaded();
            
            var supportedCourses = _scheduleManager.GetSupportedCoursesForDegree(_timeTableData.Degree.Value);
            var supportedCoursesQuestions = supportedCourses.Select(x =>
            {
                var title = $"{x}-й курс";
                var timeTableDto = new TimeTableDto();

                timeTableDto.Merge(_timeTableData);
                timeTableDto.SetCourseNumber(x);

                var command = new SelectCourseCommand(_navigationStore, _scheduleManager, timeTableDto);

                return new TimeTableQuestionDto(title, command);
            });

            SupportedCourses = new LinkedList<TimeTableQuestionDto>(supportedCoursesQuestions);
            
            SetLoaded();
            
            return Task.CompletedTask;
        }
    }
}