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

namespace Microsoft.Samples.Kinect.ControlsBasics.Pages
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
            Video.Volume = Settings.Settings.Volume;
            Video.Play();
            Video.MediaOpened += (s, a) => MainWindow.UIInvoked(DateTime.Now  + Video.NaturalDuration.TimeSpan);
        }

        private void Video_MediaEnded(object sender, RoutedEventArgs e)
        {
            play.Visibility = Visibility.Visible;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.UIInvoked();
            play.Visibility = Visibility.Collapsed;
            Video.Stop();
            Video.Play();
        }
    }
}
