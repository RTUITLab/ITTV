using Microsoft.Samples.Kinect.ControlsBasics.DataModel;
using Microsoft.Samples.Kinect.ControlsBasics.TVSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Microsoft.Samples.Kinect.ControlsBasics.Interface.Pages
{
    /// <summary>
    /// Логика взаимодействия для BackgroundVideo.xaml
    /// </summary>
    public partial class BackgroundVideo : UserControl
    {
        private BackgroundVideoPlaylist backgroundVideoPlaylist;

        public BackgroundVideo()
        {
            InitializeComponent();

            backgroundVideoPlaylist = new BackgroundVideoPlaylist();

            Loaded += BackgroundVideo_Loaded;
            this.Unloaded += BackgroundVideo_Unloaded;
            Settings.Instance.SettingsUpdated += Settings_SettingsUpdated;            

            MainWindow.Instance.HandHelper.OnHoverStart += () =>
            {
                try
                {
                    UI(() =>
                    {
                        MainWindow.Instance.UIInvoked();
                        if (IsButtonInvisible())
                        {
                            SetButtonVisibility(Visibility.Visible);
                        }
                    });
                }
                catch (Exception ex)
                {
                    MainWindow.Instance.Log(ex.Message);
                    throw;
                }
            };
        }

        private void Settings_SettingsUpdated()
        {
            BackgroungVideo.Volume = Settings.Instance.VideoVolume;
        }

        private void BackgroundVideo_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Log("BackgroundVideo loaded");

            if (backgroundVideoPlaylist.currentVideo != null)
            {
                BackgroungVideo.Volume = Settings.Instance.VideoVolume;
                BackgroungVideo.Source = backgroundVideoPlaylist.currentVideo;
                BackgroungVideo.MediaEnded += BackgroungVideo_MediaEnded;
                BackgroungVideo.Play();
            }
        }

        private void BackgroundVideo_Unloaded(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Log("BackgroundVideo unloaded method");

            BackgroungVideo.Stop();
        }

        private void BackgroungVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            Uri nextVideo = backgroundVideoPlaylist.nextVideo();

            MainWindow.Instance.Log("Next BackgroundVideo - " + nextVideo.ToString().Substring(nextVideo.ToString().LastIndexOf("/") + 1));
            
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
            MainWindow.Instance.UIInvoked();
            MainWindow.Instance.content.NavigateTo(new Menu());
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
