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

            string GesturePath = $@"{AppDomain.CurrentDomain.BaseDirectory}\GesturesDatabase\KinectGesture.gbd";
            if (File.Exists(GesturePath))
            {
                var eggVideoFile = $@"{AppDomain.CurrentDomain.BaseDirectory}\vgbtechs\kinectrequired.mp4";
                if (File.Exists(eggVideoFile))
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
}
