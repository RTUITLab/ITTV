using System;
using System.Timers;
using ITTV.WPF.Core.Helpers;
using ITTV.WPF.Core.Models;
using Microsoft.Extensions.Options;
using Serilog.Core;

namespace ITTV.WPF.Core.Services
{
    public class  UserInterfaceManager : IDisposable
    {
        public DateTime LastActivityTime { get; private set; } = DateTime.Now;

        private bool _isActiveNow = true;

        public bool IsActiveNow
        {
            get => _isActiveNow;
            private set
            {
                if (Equals(_isActiveNow, value))
                    return;

                _isActiveNow = value;
                OnStateChanged?.Invoke();

                if (_isActiveNow == false)
                {
                    OnStateChangedToInactive?.Invoke();
                }
            }
        }

        private readonly Timer _inactiveTimer;
        private readonly Settings _settings;
        
        public event Action OnStateChangedToInactive;

        public event Action OnStateChanged;

        public UserInterfaceManager(IOptions<Settings> settings)
        {
            _settings = settings.Value;
            
            _inactiveTimer = new Timer
            {
                Interval = TimeSpan.FromSeconds(1).Seconds * 1000,
                Enabled = true
            };
            _inactiveTimer.Elapsed += InactiveTimerOnElapsed;
        }

        private void InactiveTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            var idleLastInputTime = IdleTimeDetector.GetIdleTimeInfo().LastInputTime;
            TryUpdateLastActivityTime(idleLastInputTime);

            if (DateTime.Now - LastActivityTime > _settings.InactiveModeTime 
                && IsActiveNow)
            {
                IsActiveNow = false;
            }
        }

        private bool _isDarkTheme = true;
        public bool IsDarkTheme
        {
            get => _isActiveNow;
            private set
            {
                if (Equals(_isDarkTheme, value))
                    return;
                
                _isDarkTheme = value;
                ThemeUpdated?.Invoke();
            }
        }

        public event Action ThemeUpdated;

        public void ChangeTheme()
        {
            IsDarkTheme = !IsDarkTheme;
        }
        
        public void ChangeThemeToWhite()
        {
            IsDarkTheme = false;
        }
        
        public void ChangeThemeToDark()
        {
            IsDarkTheme = true;
        }

        public bool TryUpdateLastActivityTime(DateTime newLastActivityTime)
        {
            if (LastActivityTime > newLastActivityTime)
                return false;

            LastActivityTime = newLastActivityTime;
            
            if (!IsActiveNow)
            {
                IsActiveNow = true;
            }
            
            return true;
        }

        public void Dispose()
        {
            _inactiveTimer?.Dispose();
        }
    }
}