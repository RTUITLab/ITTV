using Microsoft.Samples.Kinect.DiscreteGestureBasics;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Microsoft.Samples.Kinect.ControlsBasics.Interface.Pages
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
