using System;
using System.Windows.Input;
using ITTV.WPF.MVVM.Commands.BackgroundVideos;
using ITTV.WPF.MVVM.Models;
using ITTV.WPF.MVVM.Services;
using Microsoft.Extensions.Options;

namespace ITTV.WPF.MVVM.ViewModels
{
    public class BackgroundVideoViewModel : ViewModelBase
    {
        private readonly BackgroundVideoPlaylistService _backgroundVideoPlaylistService;
        private readonly Settings _settings;

        public ICommand BackgroundVideoEndedEndedCommand { get; }
        public BackgroundVideoViewModel(BackgroundVideoPlaylistService backgroundVideoPlaylistService,
            IOptions<Settings> settings)
        {
            _backgroundVideoPlaylistService = backgroundVideoPlaylistService;
            _settings = settings.Value;

            BackgroundVideoEndedEndedCommand = new BackgroundVideoEndedCommand(this);
            Setup();
        }

        private void Setup()
        {
            if (!_backgroundVideoPlaylistService.ContainsAnyVideos()) 
                return;
            
            CurrentVideo = _backgroundVideoPlaylistService.NextVideo();
            Volume = _settings.VideoVolume;
        }
        private Uri _currentVideo;
        public Uri CurrentVideo
        {
            get => _currentVideo;
            set
            {
                _currentVideo = value;
                OnPropertyChanged(nameof(CurrentVideo));
            }
        }

        private double _volume;

        public double Volume
        {
            get => _volume;
            set
            {
                if (Equals(_volume, value))
                    return;
                
                _volume = value;
                OnPropertyChanged(nameof(Volume));
            }
        }
        public void SetNextVideo()
        {
            CurrentVideo = _backgroundVideoPlaylistService.NextVideo();
        }
    }
}