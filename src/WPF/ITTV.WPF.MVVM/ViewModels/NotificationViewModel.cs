using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Models;
using ITTV.WPF.Core.Stores;
using Microsoft.Extensions.Options;

namespace ITTV.WPF.MVVM.ViewModels
{
    public class NotificationViewModel : ViewModelBase
    {
        public Notification ActiveNotification => _notificationStore.ActiveNotification;
        public bool HasActiveNotification => _settings.IsAdminMode && ActiveNotification != null;
        
        private readonly NotificationStore _notificationStore;
        private readonly Settings _settings;

        public NotificationViewModel(NotificationStore notificationStore,
            IOptions<Settings> settings)
        {
            _notificationStore = notificationStore;
            _settings = settings.Value;

            _notificationStore.NotificationUpdated += OnNotificationUpdated;
        }

        private void OnNotificationUpdated()
        {
            OnPropertyChanged(nameof(ActiveNotification));
            OnPropertyChanged(nameof(HasActiveNotification));
        }
    }
}