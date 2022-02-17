using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Timers;

namespace ITTV.WPF.Core.Stores
{
    public sealed class NotificationStore : IDisposable
    {
        private static readonly TimeSpan NotificationActiveTime = TimeSpan.FromSeconds(5);
        private readonly Timer _timerForActiveNotification = new(NotificationActiveTime.TotalMilliseconds);
        
        public Notification ActiveNotification;
        private readonly Queue<Notification> _notifications = new();
        
        public event Action NotificationUpdated;

        public NotificationStore()
        {
            _timerForActiveNotification.Elapsed += (_, _) => TryUpdateNotification(unsetAnyway: true);
        }

        public void AddNotification(string message)
        {
            var notification = new Notification(message);
            AddNotification(notification);
        }

        public void AddNotification(Notification notification)
        {
            _notifications.Enqueue(notification);
            TryUpdateNotificationIfEmpty();
        }

        public bool TryUpdateNotificationIfEmpty()
        {
            if (ActiveNotification == null)
            {
                return TryUpdateNotification();
            }

            return false;
        }

        public bool TryUpdateNotification(bool unsetAnyway = false)
        {
            if (unsetAnyway)
                UnSetActiveNotification();
            if (_notifications.Count > 0)
            {
                var notification = _notifications.Dequeue();
                if (notification == null)
                    return false;
            
                ActiveNotification = notification;
            
                NotificationUpdated?.Invoke();

                _timerForActiveNotification.Start();
            
                return true;
            }

            return false;
        }

        private void UnSetActiveNotification()
        {
            ActiveNotification = null;

            _timerForActiveNotification.Stop();
            
            NotificationUpdated?.Invoke();
        }

        public void Dispose()
        {
            _timerForActiveNotification?.Dispose();
        }
    }

    public class Notification
    {
        private const int MaxMessageLength = 100;
        public Notification()
        { }
        public Notification(string message)
        {
            Message = message.Substring(0, Math.Min(message.Length, MaxMessageLength)) + "...";
        }
        [MaxLength(MaxMessageLength)]
        public string Message { get; set; } 
    }
}