using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Helpers;
using ITTV.WPF.Core.Stores;
using Serilog;

namespace ITTV.WPF.MVVM.ViewModels.Videos
{
    public class VideosViewModel : ViewModelBase
    {
        private ObservableCollection<VideoViewModel> _videos = new();
        public ObservableCollection<VideoViewModel> Videos
        {
            get => _videos;
            set
            {
                if (Equals(_videos, value))
                    return;

                if (_videos != null && value != null)
                {
                    if (_videos.SequenceEqual(value))
                        return;
                }

                _videos = value;
                OnPropertyChanged(nameof(Videos));
            }
        }


        private readonly NavigationStore _navigationStore;
        private readonly NotificationStore _notificationStore;
        
        public VideosViewModel(NavigationStore navigationStore,
            NotificationStore notificationStore)
        {
            _navigationStore = navigationStore;
            _notificationStore = notificationStore;
        }

        public override void Recalculate()
        {
            try
            {
                SetUnloaded();

                var path = PathHelper.GetDirectoryVideosPath;
                var videos = VideoHelper.GetSupportedVideoUris(path)
                    .Select(x => new VideoViewModel(Path.GetFileNameWithoutExtension(x.OriginalString),
                        x,
                        _navigationStore));

                Videos = new ObservableCollection<VideoViewModel>(videos);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "Exception while syncing videos");

                var textException = e.InnerException?.Message ?? e.Message;
                _notificationStore.AddNotification(textException);
            }
            finally
            {
                SetLoaded();
            }
        }
    }
}