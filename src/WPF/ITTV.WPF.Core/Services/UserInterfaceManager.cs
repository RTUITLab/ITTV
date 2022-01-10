using System;

namespace ITTV.WPF.Core.Services
{
    public class UserInterfaceManager
    {
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
    }
}