using System;
using System.Timers;
using System.Windows.Input;
using ITTV.WPF.Core.Helpers;
using ITTV.WPF.Core.Models;
using Microsoft.Extensions.Options;

namespace ITTV.WPF.Core.Services
{
    public class UserInterfaceManager : IDisposable
    {
        private readonly Timer _inactiveTimer;
        private readonly Settings _settings;
        
        public event Action OnStateChangedToInactive;

        public UserInterfaceManager(IOptions<Settings> settings)
        {
            _settings = settings.Value;
            
            InputManager.Current.PreProcessInput += CurrentOnPreProcessInput;
            
            _inactiveTimer = new Timer
            {
                Interval = TimeSpan.FromSeconds(1).Seconds * 1000,
                Enabled = true
            };
            _inactiveTimer.Elapsed += InactiveTimerOnElapsed;
        }

        private void InactiveTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now - IdleTimeDetector.GetIdleTimeInfo().LastInputTime > _settings.InactiveModeTime)
            {
                OnStateChangedToInactive?.Invoke();
            }
        }

        private void CurrentOnPreProcessInput(object sender, PreProcessInputEventArgs e)
        {
        }

        public bool IsDarkTheme { get; private set; } = true;

        public event Action ThemeUpdated;

        public void ChangeTheme()
        {
            IsDarkTheme = !IsDarkTheme;
            OnThemeUpdated();
        }

        protected virtual void OnThemeUpdated()
        {
            ThemeUpdated?.Invoke();
        }

        public void Dispose()
        {
            _inactiveTimer?.Dispose();
        }
    }
}