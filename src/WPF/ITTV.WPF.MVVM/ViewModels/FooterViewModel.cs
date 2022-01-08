using System;
using System.Windows.Input;
using System.Windows.Threading;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Helpers;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.Commands;

namespace ITTV.WPF.MVVM.ViewModels
{
    public class FooterViewModel : ViewModelBase
    {
        private readonly UserInterfaceManager _userInterfaceManager;
        private readonly NavigationStore _navigationStore;

        private DispatcherTimer _timer;

        public bool IsDarkTheme => _userInterfaceManager.IsDarkTheme;

        public FooterViewModel(NavigationStore navigationStore,
            UserInterfaceManager userInterfaceManager)
        {
            _navigationStore = navigationStore;
            _navigationStore.HistoryViewModelsUpdated += OnHistoryViewModelsUpdated;
            
            _userInterfaceManager = userInterfaceManager;
            _userInterfaceManager.ThemeUpdated += OnThemeUpdated;
            
            NavigateBackCommand = new NavigateBackCommand(_navigationStore);
            
            Recalc();
            StartTimer();
        }
        public ICommand NavigateBackCommand { get; }
        private bool _canNavigateBack;
        public bool CanNavigateBack
        {
            get => _canNavigateBack;
            set
            {
                if (Equals(_canNavigateBack, value))
                    return;
                
                _canNavigateBack = value;
                OnPropertyChanged(nameof(CanNavigateBack));
            }
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
            var dateTime = DateTime.Now;
            
            DayLongFormat = MireaTimeHelper.GetLongDate(dateTime);
            TimeLongFormat = MireaTimeHelper.GetLongTime(dateTime);
            CurrentWeekOfSemester = MireaTimeHelper.GetWeekOfSemester(dateTime);
            StageOfClasses = MireaTimeHelper.GetStageOfClasses(dateTime);
        }

        private void StartTimer()
        {
            _timer = new DispatcherTimer(DispatcherPriority.Background)
            {
                Interval = TimeSpan.FromSeconds(1),
                IsEnabled = true
            };
            
            _timer.Tick += (_, _) =>
            {
                Recalc();
            };
        }

        private void OnHistoryViewModelsUpdated()
        {
            CanNavigateBack = _navigationStore.CanNavigateBack();
        }

        private void OnThemeUpdated()
        {
            OnPropertyChanged(nameof(IsDarkTheme));
        }

        public override void Dispose()
        {
            base.Dispose();
            
            _timer.Stop();
        }
    }
}