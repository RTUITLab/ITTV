using System;
using System.IO;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Helpers;
using ITTV.WPF.MVVM.DTOs;

namespace ITTV.WPF.MVVM.ViewModels.Schedule
{
    public class ScheduleForCourseViewModel : ViewModelBase
    {
        private readonly TimeTableDto _timeTableDto;
        public ScheduleForCourseViewModel(TimeTableDto timeTableDto)
        {
            _timeTableDto = timeTableDto;
        }

        public override void Recalculate()
        {
            if (_timeTableDto.Degree == default 
                || _timeTableDto.CourseNumber == default)
                return;

            var path = PathHelper.GetFileScheduleImageForCourse(_timeTableDto.Degree, _timeTableDto.CourseNumber);
            
            var fileExist = File.Exists(path);
            if (fileExist)
            {
                var uri = new Uri(path);
                FilePath = uri;
            }
        }

        public Uri FilePath 
        { 
            get => _filePath;
            private set
            {
                if (Equals(value, _filePath))
                    return;

                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
            } 
        }
        private Uri _filePath;
    }
}