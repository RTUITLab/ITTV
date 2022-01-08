using System;

namespace ITTV.WPF.MVVM.Services
{
    public class UserInterfaceManager
    {
        public bool IsDarkTheme { get; private set; }

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