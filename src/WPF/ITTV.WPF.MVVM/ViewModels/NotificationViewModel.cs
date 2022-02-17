using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Stores;

namespace ITTV.WPF.MVVM.ViewModels
{
    public class NotificationViewModel : ViewModelBase
    {
        public Notification ActiveNotification => _notificationStore.ActiveNotification;
        public bool HasActiveNotification => ActiveNotification != null;
        
        private readonly NotificationStore _notificationStore;

        public NotificationViewModel(NotificationStore notificationStore)
        {
            _notificationStore = notificationStore;

            _notificationStore.NotificationUpdated += OnNotificationUpdated;
        }

        private void OnNotificationUpdated()
        {
            OnPropertyChanged(nameof(ActiveNotification));
            OnPropertyChanged(nameof(HasActiveNotification));
        }
    }
}