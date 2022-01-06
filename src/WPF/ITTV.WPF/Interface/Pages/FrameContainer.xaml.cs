using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ITTV.WPF.DataModel.Models;
using ITTV.WPF.Views;

namespace ITTV.WPF.Interface.Pages
{
    /// <summary>
    /// Логика взаимодействия для FrameContainer.xaml
    /// </summary>
    public partial class FrameContainer : UserControl
    {
        private static readonly List<UserControl> History = new List<UserControl>();

        private readonly TimeTable _timeTable;
        private readonly BackgroundVideo _backgroundVideo;

        private readonly MainWindow _mainWindow;

        public FrameContainer(TimeTable timeTable, 
            MainWindow mainWindow, BackgroundVideo backgroundVideo)
        {
            _timeTable = timeTable;
            _mainWindow = mainWindow;
            _backgroundVideo = backgroundVideo;

        }

        public FrameContainer()
        {
            InitializeComponent();
        }


        public bool CanGoBackFuther()
        {
            return History.Count > 2;
        }
        public void GoBack()
        {   
            if (ContentType() == typeof(TimeTableList))
            {
                _timeTable.UnChoose();
            }

            //MainWindow.Log("Navigated back to " + History[History.Count - 1].GetType());

            History.RemoveAt(History.Count - 1);
            content.Content = History[History.Count - 1];
        }
        public void NavigateTo(UserControl innerContent)
        {
            //MainWindow.Log("Navigated to " + innerContent.GetType());

            content.Content = innerContent;
            History.Add(innerContent);
            _mainWindow.backButton.Visibility = History.Count > 1 ? Visibility.Visible : Visibility.Hidden;

            _mainWindow.SetWhiteTheme();
        }

        public object GetContent()
        {
            return content.Content;
        }

        public void OpenBackgroundVideo()
        {
            //MainWindow.Log("Open BackgroundVideo");

            History.RemoveRange(0, History.Count);

            content.Content = _backgroundVideo;

            _mainWindow.SetBlackTheme();
            _mainWindow.backButton.Visibility = Visibility.Hidden;
        }

        public void OpenNightPhoto()
        {
            //MainWindow.Log("Open NightPhoto");

            History.RemoveRange(0, History.Count);

            content.Content = new NightPhoto();

            _mainWindow.SetBlackTheme();
            _mainWindow.backButton.Visibility = Visibility.Hidden;
        }

        public Type ContentType()
        {
            return content.Content?.GetType();
        }
    }
}
