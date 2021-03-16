using Microsoft.Samples.Kinect.ControlsBasics.DataModel;
using Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
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
    /// Логика взаимодействия для FrameContainer.xaml
    /// </summary>
    public partial class FrameContainer : UserControl
    {
        public static List<UserControl> history = new List<UserControl>();

        public FrameContainer()
        {
            InitializeComponent();            
        }


        public bool CanGoBackFuther()
        {
            return history.Count > 2;
        }
        public void GoBack()
        {
            if (ContentType() == typeof(TimeTableList) && !TimeTable.Instance.IsOut()) {
                TimeTable.Instance.Back();            
            }
            
            MainWindow.Instance.Log("Navigated back to " + history[history.Count - 1].GetType().ToString());

            history.RemoveAt(history.Count - 1);
            content.Content = history[history.Count - 1];
        }
        public void NavigateTo(UserControl innerContent)
        {
            MainWindow.Instance.Log("Navigated to " + innerContent.GetType().ToString());

            content.Content = innerContent;
            history.Add(innerContent);
            MainWindow.Instance.backButton.Visibility = history.Count > 1 ? Visibility.Visible : Visibility.Hidden;

            MainWindow.Instance.setWhiteTheme();
        }

        public object GetContent()
        {
            return content.Content;
        }

        public void OpenBackgroundVideo()
        {
            MainWindow.Instance.Log("Open BackgroundVideo");

            history.RemoveRange(0, history.Count);

            content.Content = new BackgroundVideo();

            MainWindow.Instance.setBlackTheme();
            MainWindow.Instance.backButton.Visibility = Visibility.Hidden;
        }

        public void OpenNightPhoto()
        {
            MainWindow.Instance.Log("Open NightPhoto");

            history.RemoveRange(0, history.Count);

            content.Content = new NightPhoto();

            MainWindow.Instance.setBlackTheme();
            MainWindow.Instance.backButton.Visibility = Visibility.Hidden;
        }

        public Type ContentType()
        {
            if (content.Content != null) {
                return content.Content.GetType();
            } else
            {
                return null;
            }
        }
    }
}
