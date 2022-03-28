using System;
using System.IO;
using System.Windows.Input;
using System.Windows.Threading;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Helpers;
using ITTV.WPF.Core.Models;
using ITTV.WPF.Core.Services;
using ITTV.WPF.MVVM.Commands;
using ITTV.WPF.MVVM.Commands.BackgroundVideos;
using Microsoft.Extensions.Options;

namespace ITTV.WPF.MVVM.ViewModels
{
    public class BackgroundVideoViewModel : ViewModelBase
    {
        private readonly BackgroundVideoPlaylistService _backgroundVideoPlaylistService;
        private readonly UserInterfaceManager _userInterfaceManager;
        private readonly Settings _settings;
        
        private DispatcherTimer _timer;


        public ICommand BackgroundVideoEndedCommand { get; }
        public ICommand ShowMenuCommand { get; }
        public ICommand ChangeThemeCommand { get; }

        public BackgroundVideoViewModel(BackgroundVideoPlaylistService backgroundVideoPlaylistService,
            IOptions<Settings> settings,
            UserInterfaceManager userInterfaceManager,
            NavigateCommand<MenuViewModel> showMenuCommand)
        {
            BackgroundVideoEndedCommand = new BackgroundVideoEndedCommand(this);
            ShowMenuCommand = showMenuCommand;
            ChangeThemeCommand = new ChangeThemeCommand(userInterfaceManager);

            _backgroundVideoPlaylistService = backgroundVideoPlaylistService;
            
            _userInterfaceManager = userInterfaceManager;
            _userInterfaceManager.OnStateChanged += () => OnPropertyChanged(nameof(IsActiveStatus));
            
            _settings = settings.Value;
            
            StartTimer();
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
            get => _settings.VideoVolume;
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
        
        private void StartTimer()
        {
            _timer = new DispatcherTimer(DispatcherPriority.Background)
            {
                Interval = TimeSpan.FromSeconds(1),
                IsEnabled = true
            };
            
            _timer.Tick += (_, _) =>
            {
                var isInactiveMode = _settings.NeedCheckTime 
                                     && (_settings.StartWorkTime > DateTime.Now.TimeOfDay
                                         || _settings.EndWorkTime < DateTime.Now.TimeOfDay);
                IsInactiveWorkMode = isInactiveMode;

            };
        }

        public bool IsInactiveWorkMode
        {
            get => _isInactiveWorkMode;
            set
            {
                if (Equals(_isInactiveWorkMode, value))
                {
                    if (!(_isInactiveWorkMode && CurrentVideo == default))
                        return;
                }
                
                _isInactiveWorkMode = value;

                if (_isInactiveWorkMode)
                {
                    CurrentVideo = new Uri(PathHelper.FileInactiveImageGerb);
                }
                else
                {
                    Setup();
                }
                
                OnPropertyChanged(nameof(IsInactiveWorkMode));
            }
        }

        public bool IsActiveStatus => _userInterfaceManager.IsActiveNow && !_isInactiveWorkMode;
    
        private bool _isInactiveWorkMode;
    }
}