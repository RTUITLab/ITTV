using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ITTV.WPF.DataModel.Models;

namespace ITTV.WPF.Interface.Pages
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
            if (ContentType() == typeof(TimeTableList))
            {
                TimeTable.Instance.UnChoose();
            }

            MainWindow.Log("Navigated back to " + history[history.Count - 1].GetType().ToString());

            history.RemoveAt(history.Count - 1);
            content.Content = history[history.Count - 1];
        }
        public void NavigateTo(UserControl innerContent)
        {
            MainWindow.Log("Navigated to " + innerContent.GetType().ToString());

            content.Content = innerContent;
            history.Add(innerContent);
            MainWindow.Instance.backButton.Visibility = history.Count > 1 ? Visibility.Visible : Visibility.Hidden;

            MainWindow.Instance.SetWhiteTheme();
        }

        public object GetContent()
        {
            return content.Content;
        }

        public void OpenBackgroundVideo()
        {
            MainWindow.Log("Open BackgroundVideo");

            history.RemoveRange(0, history.Count);

            content.Content = new BackgroundVideo();

            MainWindow.Instance.SetBlackTheme();
            MainWindow.Instance.backButton.Visibility = Visibility.Hidden;
        }

        public void OpenNightPhoto()
        {
            MainWindow.Log("Open NightPhoto");

            history.RemoveRange(0, history.Count);

            content.Content = new NightPhoto();

            MainWindow.Instance.SetBlackTheme();
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
