using System;
using System.Timers;
using ITTV.WPF.Core.Helpers;
using ITTV.WPF.Core.Models;
using Microsoft.Extensions.Options;

namespace ITTV.WPF.Core.Services
{
    public class UserInterfaceManager : IDisposable
    {
        public DateTime LastActivityTime { get; private set; }
        
        private readonly Timer _inactiveTimer;
        private readonly Settings _settings;
        
        public event Action OnStateChangedToInactive;

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
            
            if (DateTime.Now - LastActivityTime > _settings.InactiveModeTime)
            {
                OnStateChangedToInactive?.Invoke();
            }
        }

        public bool IsDarkTheme { get; private set; } = true;

        public event Action ThemeUpdated;

        public void ChangeTheme()
        {
            IsDarkTheme = !IsDarkTheme;
            OnThemeUpdated();
        }

        public bool TryUpdateLastActivityTime(DateTime newLastActivityTime)
        {
            if (LastActivityTime > newLastActivityTime)
                return false;

            LastActivityTime = newLastActivityTime;
            return true;
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