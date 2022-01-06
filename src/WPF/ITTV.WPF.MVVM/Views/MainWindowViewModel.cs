﻿using System;
using System.Windows.Threading;
using ITTV.WPF.MVVM.Helpers;
using ITTV.WPF.MVVM.Views.Base;

namespace ITTV.WPF.MVVM.Views
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            Recalc();
            StartTimer();
        }

        public string StageOfClasses
        {
            get => _stageOfClasses;
            private set
            {
                var newValue = string.IsNullOrWhiteSpace(_currentWeekOfSemester) ? null : value;
                
                if (Equals(_stageOfClasses, newValue))
                    return;

                _stageOfClasses = newValue;
                OnPropertyChanged(nameof(StageOfClasses));
            }
        }

        private string _stageOfClasses;

        public string DayLongFormat
        {
            get => _dayLongFormat;
            private set
            {
                if (Equals(_dayLongFormat, value)) 
                    return;
                
                _dayLongFormat = value;
                OnPropertyChanged(nameof(DayLongFormat));
            }
        }

        private string _dayLongFormat;

        public string TimeLongFormat
        {
            get => _timeLongFormat;
            private set
            {
                if (Equals(_timeLongFormat, value))
                    return;
                
                _timeLongFormat = value;
                OnPropertyChanged(nameof(TimeLongFormat));
            }
        }

        private string _timeLongFormat;

        public string CurrentWeekOfSemester
        {
            get => _currentWeekOfSemester;
            private set
            {
                if (Equals(_currentWeekOfSemester, value)) 
                    return;
                
                _currentWeekOfSemester = value;
                OnPropertyChanged(nameof(CurrentWeekOfSemester));
            }
        }

        private string _currentWeekOfSemester;

        private void Recalc()
        {
            var dateTime = DateTime.Now - TimeSpan.FromDays(58);
            
            DayLongFormat = MireaTimeHelper.GetLongDate(dateTime);
            TimeLongFormat = MireaTimeHelper.GetLongTime(dateTime);
            CurrentWeekOfSemester = MireaTimeHelper.GetWeekOfSemester(dateTime);
            StageOfClasses = MireaTimeHelper.GetStageOfClasses(dateTime);
        }

        private void StartTimer()
        {
            var timer = new DispatcherTimer(DispatcherPriority.Background)
            {
                Interval = TimeSpan.FromSeconds(1),
                IsEnabled = true
            };
            
            timer.Tick += (_, _) =>
            {
                Recalc();
            };
        }
    }
}