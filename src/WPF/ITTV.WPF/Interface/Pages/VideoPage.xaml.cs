using System;
using System.Windows;
using System.Windows.Controls;
using ITTV.WPF.DataModel.Models;
using ITTV.WPF.Views;

namespace ITTV.WPF.Interface.Pages
{
    /// <summary>
    /// Логика взаимодействия для VideoPage.xaml
    /// </summary>
    public partial class VideoPage : UserControl
    {
        private readonly MainWindow _mainWindow;
        private readonly Settings _settings;
        public VideoPage(string videoSource, MainWindow mainWindow, Settings settings)
        {
            _mainWindow = mainWindow;
            _settings = settings;

            InitializeComponent();

            play.Visibility = Visibility.Collapsed;

            Uri uri = new Uri(videoSource, UriKind.Relative);
            Video.Source = uri;
            Video.Volume = _settings.VideoVolume;
            Video.Play();
            Video.MediaOpened += (s, a) => _mainWindow.UiInvoked(DateTime.Now  + Video.NaturalDuration.TimeSpan);

            _settings.SettingsUpdated += Instance_SettingsUpdated;
        }

        private void Instance_SettingsUpdated()
        {
            _mainWindow.Ui(() => { Video.Volume = _settings.VideoVolume; });
            
        }

        private void Video_MediaEnded(object sender, RoutedEventArgs e)
        {
            play.Visibility = Visibility.Visible;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.UiInvoked();
            play.Visibility = Visibility.Collapsed;
            Video.Stop();
            Video.Play();
        }
    }
}
