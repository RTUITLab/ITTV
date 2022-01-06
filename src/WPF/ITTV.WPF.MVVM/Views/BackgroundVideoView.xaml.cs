using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ITTV.WPF.MVVM.Views
{
    public partial class BackgroundVideoView : UserControl
    {
        public BackgroundVideoView()
        {
            InitializeComponent();
            BackgroundVideo.Play();
        }

        private void BackgroundVideo_OnSourceUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void BackgroundVideo_OnMediaEnded(object sender, RoutedEventArgs e)
        {
            BackgroundVideo.Stop();
            
            BackgroundVideo.Play();
        }
    }
}