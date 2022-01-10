using System;
using System.Timers;
using System.Windows.Input;
using ITTV.WPF.Core.Models;
using Microsoft.Extensions.Options;

namespace ITTV.WPF.Core.Services
{
    public class UserInterfaceManager : IDisposable
    {
        private readonly Timer _timer;
        private readonly Settings _settings;
        private DateTime _lastActivity = DateTime.Now;
        public event Action OnStateChangedToInactive;

        public UserInterfaceManager(IOptions<Settings> settings)
        {
            _settings = settings.Value;
            
            InputManager.Current.PreProcessInput += CurrentOnPreProcessInput;
            
            _timer = new Timer
            {
                Interval = TimeSpan.FromSeconds(1).Seconds * 1000,
                Enabled = true
            };
            _timer.Elapsed += TimerOnElapsed;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now - _lastActivity > _settings.InactiveModeTime)
            {
                OnStateChangedToInactive?.Invoke();
            }
        }

        private void CurrentOnPreProcessInput(object sender, PreProcessInputEventArgs e)
        {
            _lastActivity = DateTime.Now;
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
            _timer?.Dispose();
        }
    }
}