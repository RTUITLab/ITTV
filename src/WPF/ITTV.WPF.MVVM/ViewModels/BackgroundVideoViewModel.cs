using System;
using ITTV.WPF.MVVM.Services;

namespace ITTV.WPF.MVVM.ViewModels
{
    public class BackgroundVideoViewModel : ViewModelBase
    {
        private readonly BackgroundVideoPlaylistService _backgroundVideoPlaylistService;
        public BackgroundVideoViewModel(BackgroundVideoPlaylistService backgroundVideoPlaylistService)
        {
            _backgroundVideoPlaylistService = backgroundVideoPlaylistService;
            
            SetNextVideo();
        }

        private void SetNextVideo()
        {
            CurrentVideo = _backgroundVideoPlaylistService.NextVideo();
        }
        
        private Uri _currentVideo;
        public Uri CurrentVideo
        {
            get => _currentVideo;
            set
            {
                if (Equals(_currentVideo, value))
                    return;

                _currentVideo = value;
                OnPropertyChanged(nameof(CurrentVideo));
            }
        }
        
    }
}