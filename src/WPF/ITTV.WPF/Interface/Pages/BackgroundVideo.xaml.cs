using System;
using System.Windows;
using System.Windows.Controls;
using ITTV.WPF.DataModel;
using ITTV.WPF.DataModel.Models;
using ITTV.WPF.Views;

namespace ITTV.WPF.Interface.Pages
{
    /// <summary>
    /// Логика взаимодействия для BackgroundVideo.xaml
    /// </summary>
    public partial class BackgroundVideo : UserControl
    {
        private readonly MainWindow _mainWindow;
        private readonly Menu _menu;
        
        private BackgroundVideoPlaylist backgroundVideoPlaylist;

        private readonly Settings _settings;
        private readonly TimeTable _timeTable;

        public BackgroundVideo(Settings settings,
            TimeTable timeTable, 
            MainWindow mainWindow, 
            Menu menu)
        {
            _settings = settings;
            _timeTable = timeTable;
            _mainWindow = mainWindow;
            _menu = menu;

            InitializeComponent();

            backgroundVideoPlaylist = new BackgroundVideoPlaylist(settings);

            Loaded += BackgroundVideo_Loaded;
            this.Unloaded += BackgroundVideo_Unloaded;
            settings.SettingsUpdated += Settings_SettingsUpdated;

            timeTable.CloseTimeTable();

            _mainWindow.handManager.OnHoverStart += () =>
            {
                try
                {
                    UI(() =>
                    {
                        _mainWindow.UiInvoked();
                        if (IsButtonInvisible())
                        {
                            SetButtonVisibility(Visibility.Visible);
                        }
                    });
                }
                catch (Exception ex)
                {
                    MainWindow.Log(ex.Message);
                    throw;
                }
            };
        }

        private void Settings_SettingsUpdated()
        {
            _mainWindow.Ui(() => {
                BackgroungVideo.Volume = _settings.VideoVolume;
                backgroundVideoPlaylist = new BackgroundVideoPlaylist(_settings);
            });
        }

        private void BackgroundVideo_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow.Log("BackgroundVideo loaded");

            if (backgroundVideoPlaylist.currentVideo != null)
            {
                BackgroungVideo.Volume = _settings.VideoVolume;
                BackgroungVideo.Source = backgroundVideoPlaylist.currentVideo;
                BackgroungVideo.MediaEnded += BackgroungVideo_MediaEnded;
                BackgroungVideo.Play();
            }
        }

        private void BackgroundVideo_Unloaded(object sender, RoutedEventArgs e)
        {
            MainWindow.Log("BackgroundVideo unloaded method");

            BackgroungVideo.Stop();
        }

        private void BackgroungVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            Uri nextVideo = backgroundVideoPlaylist.NextVideo();

            MainWindow.Log("Next BackgroundVideo - " + nextVideo.ToString().Substring(nextVideo.ToString().LastIndexOf("/") + 1));
            
            BackgroungVideo.Stop();
            BackgroungVideo.Source = nextVideo;
            BackgroungVideo.Play();
        }

        private void UI(Action action)
        {
            Dispatcher.Invoke(action);
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.UiInvoked();
            _mainWindow.ContentV2.NavigateTo(_menu);
        }

        public bool IsButtonInvisible()
        {
            return MenuButton.Visibility == Visibility.Collapsed;
        }

        public void SetButtonVisibility(Visibility visibility)
        {
            MenuButton.Visibility = visibility;
        }
    }
}
