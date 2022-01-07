using System;

namespace ITTV.WPF.MVVM.Services
{
    public class UserInterfaceManager
    {
        private DateTime _lastInterfaceInteractTime = DateTime.Now;

        public UserInterfaceManager()
        { }

        public void UpdateInteractedTime()
            => _lastInterfaceInteractTime = DateTime.Now;
        
    }
}