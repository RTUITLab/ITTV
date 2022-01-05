using System;
using System.Windows;
using System.Windows.Controls;
using ITTV.WPF.DataModel.Models;

namespace ITTV.WPF.Interface.Pages
{
    /// <summary>
    /// Логика взаимодействия для VideoPage.xaml
    /// </summary>
    public partial class VideoPage : UserControl
    {
        public VideoPage(string VideoSource)
        {
            InitializeComponent();

            play.Visibility = Visibility.Collapsed;

            Uri uri = new Uri(VideoSource, UriKind.Relative);
            Video.Source = uri;
            Video.Volume = Settings.Instance.VideoVolume;
            Video.Play();
            Video.MediaOpened += (s, a) => MainWindow.Instance.UiInvoked(DateTime.Now  + Video.NaturalDuration.TimeSpan);

            Settings.Instance.SettingsUpdated += Instance_SettingsUpdated;
        }

        private void Instance_SettingsUpdated()
        {
            MainWindow.Instance.Ui(() => { Video.Volume = Settings.Instance.VideoVolume; });
            
        }

        private void Video_MediaEnded(object sender, RoutedEventArgs e)
        {
            play.Visibility = Visibility.Visible;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.UiInvoked();
            play.Visibility = Visibility.Collapsed;
            Video.Stop();
            Video.Play();
        }
    }
}
