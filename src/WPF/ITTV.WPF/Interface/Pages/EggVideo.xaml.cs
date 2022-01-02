using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ITTV.WPF.Interface.Pages
{
    /// <summary>
    /// Логика взаимодействия для EggVideo.xaml
    /// </summary>
    public partial class EggVideo : UserControl
    {
        public EggVideo()
        {
            InitializeComponent();

            var gesturePath = AllPaths.FileGestureDatabasePath;
            var eggVideoFile = AllPaths.FileEggVideoPath;

            if (File.Exists(gesturePath) && File.Exists(eggVideoFile))
            {
                EggVideoElement.Source = new Uri(eggVideoFile);
                EggVideoElement.Visibility = Visibility.Collapsed;
                EggVideoElement.MediaEnded += (s, e) =>
                {
                    MainWindow.Instance.content.OpenBackgroundVideo();
                };
            }
        }
    }
}
