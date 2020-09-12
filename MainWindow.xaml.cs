//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.ControlsBasics
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using System.Windows.Automation.Provider;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Microsoft.Kinect;
    using Microsoft.Kinect.Wpf.Controls;
    using Microsoft.Samples.Kinect.ControlsBasics.DataModel;

    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow
    {
        public static List<string> history = new List<string>();
        public static ContentControl var_navigationRegion;

        private static bool needCheckTime;
        private bool adminMode = false;

        private static DateTime LastUIOperation;
        private static MainWindow instance = default;
        private static Timer timer;
        private static HandOverHelper handHelper;
        static MainWindow()
        {
            timer = new Timer(CheckPersonIsRemoved, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(100));
            needCheckTime = bool.Parse(System.IO.File.ReadAllText("time.txt"));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class. 
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();


            AppDomain.CurrentDomain.ProcessExit += (e, s) => { Process.Start("ControlsBasics-WPF.exe"); };
            AppDomain.CurrentDomain.UnhandledException += (e, s) => { Process.Start("ControlsBasics-WPF.exe"); App.Current.Shutdown(); };


            instance = this;

            CheckTime();

            var_navigationRegion = navigationRegion;

            CreateData.GetAllVideos();
            CreateData.GetNewsFromSite();
            CreateData.GetGames();
            CreateData.GetAllTimetable();

            KinectRegion.SetKinectRegion(this, kinectRegion);

            App app = ((App)Application.Current);
            app.KinectRegion = kinectRegion;

            handHelper = new HandOverHelper(kinectRegion, Dispatcher);

            // Use the default sensor
            this.kinectRegion.KinectSensor = KinectSensor.GetDefault();

            //// Add in display content
            var sampleDataSource = SampleDataSource.GetGroup("Menu");
            history.Add("Menu");
            this.itemsControl.ItemTemplate = (DataTemplate)this.FindResource(sampleDataSource.TypeGroup + "Template");
            this.itemsControl.ItemsSource = sampleDataSource;

            // Open a Main video when nowbody use system
            BodyFrameReader bodyFrameReader = this.kinectRegion.KinectSensor.BodyFrameSource.OpenReader();

            handHelper.OnHoverStart += () =>
            {
                Log("Lenya start 1");
                try
                {
                    UI(() =>
                    {
                        if (instance.BackgroungVideo.Visibility == Visibility.Visible)
                        {
                            UIInvoked();
                            MenuButton.Visibility = Visibility.Visible;
                        }
                    });
                }
                catch (Exception ex)
                {
                    Log(ex.Message);
                    throw;
                }

                Log("Lenya start 2");
            };

            handHelper.OnHoverKeyboardCheck += () =>
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.K))
                {
                    adminMode = !adminMode;
                    handHelper.adminMode = adminMode;
                    Cursor =  adminMode ?  Cursors.Arrow : Cursors.None;
                }
            };

            if (SampleDataSource.GetItem("Video-Main") != null)
            {
                BackgroungVideo.Source = new Uri(SampleDataSource.GetItem("Video-Main").Parametrs[0].ToString());
                BackgroungVideo.MediaEnded += BackgroungVideo_MediaEnded;
                BackgroungVideo.Play();
            }
            
        }

        private void CheckTime()
        {
            if (needCheckTime && (DateTime.Now.Hour >= 22 || DateTime.Now.Hour < 8))
            {
                BackgroungImage.Visibility = Visibility.Visible;
                BackgroungVideo.Volume = 0;
            }
            else
            {
                BackgroungImage.Visibility = Visibility.Collapsed;
                BackgroungVideo.Volume = 1;
            }
        }

        private void BackgroungVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            BackgroungVideo.Stop();
            BackgroungVideo.Play();
        }


        /// <summary>
        /// Handle a button click from the wrap panel.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            UIInvoked();
            var button = (Button)e.OriginalSource;
            SampleDataItem sampleDataItem = button.DataContext as SampleDataItem;

            if (sampleDataItem != null)
            {
                if (sampleDataItem.Task == SampleDataItem.TaskType.Page && sampleDataItem.NavigationPage != null)
                {
                    if (sampleDataItem.Title == "Новости")
                    {
                        CreateData.GetNewsFromSite();
                    }
                    else if (sampleDataItem.Title == "Видео")
                    {
                        CreateData.GetAllVideos();
                    }
                    else if (sampleDataItem.Title == "Игры")
                    {
                        CreateData.GetGames();
                    } else if (sampleDataItem.Title == "Расписание")
                    {
                        CreateData.GetAllTimetable();
                    }

                    history.Add(sampleDataItem.UniqueId);
                    backButton.Visibility = System.Windows.Visibility.Visible;
                    navigationRegion.Content = Activator.CreateInstance(sampleDataItem.NavigationPage, sampleDataItem.Parametrs);
                }
                if (sampleDataItem.Task == SampleDataItem.TaskType.ChangeGroup && sampleDataItem.NewGroup != null)
                {
                    history.Add(sampleDataItem.NewGroup);
                    backButton.Visibility = System.Windows.Visibility.Visible;
                    var type = SampleDataSource.GetGroup(sampleDataItem.NewGroup).TypeGroup;
                    this.itemsControl.ItemTemplate = (DataTemplate)this.FindResource(type + "Template");
                    this.itemsControl.ItemsSource = SampleDataSource.GetGroup(sampleDataItem.NewGroup);
                }
                if (sampleDataItem.Task == SampleDataItem.TaskType.Execute && sampleDataItem.Parametrs != null)
                {
                    Process.Start((string)sampleDataItem.Parametrs[0]);

                    ButtonAutomationPeer peer = new ButtonAutomationPeer(backButton);
                    IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                    invokeProv.Invoke();
                }
            }
            else
            {
                var selectionDisplay = new SelectionDisplay(button.Content as string);
                this.kinectRegionGrid.Children.Add(selectionDisplay);

                // Selection dialog covers the entire interact-able area, so the current press interaction
                // should be completed. Otherwise hover capture will allow the button to be clicked again within
                // the same interaction (even whilst no longer visible).
                selectionDisplay.Focus();

                // Since the selection dialog covers the entire interact-able area, we should also complete
                // the current interaction of all other pointers.  This prevents other users interacting with elements
                // that are no longer visible.
                this.kinectRegion.InputPointerManager.CompleteGestures();

                e.Handled = true;
            }
        }

        /// <summary>
        /// Handle the back button click.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void GoBack(object sender, RoutedEventArgs e)
        {
            //if (navigationRegion.Content != kinectRegionGrid)
            //{
            //    navigationRegion.Content = this.kinectRegionGrid;
            //} 
            //else
            //{
            UIInvoked();
            history.RemoveAt(history.Count - 1);
            SampleDataItem sampleDataItem = SampleDataSource.GetItem(history[history.Count - 1]);

            if (sampleDataItem == null)
            {
                navigationRegion.Content = this.kinectRegionGrid;
                var type = SampleDataSource.GetGroup(history.Last()).TypeGroup;
                this.itemsControl.ItemTemplate = (DataTemplate)this.FindResource(type + "Template");
                this.itemsControl.ItemsSource = SampleDataSource.GetGroup(history[history.Count - 1]);
            }
            else if (sampleDataItem.Task == SampleDataItem.TaskType.Page && sampleDataItem.NavigationPage != null)
            {
                navigationRegion.Content = Activator.CreateInstance(sampleDataItem.NavigationPage, sampleDataItem.Parametrs);
            }
            else if (sampleDataItem.Task == SampleDataItem.TaskType.ChangeGroup && sampleDataItem.NewGroup != null)
            {
                var type_b = SampleDataSource.GetGroup(history[history.Count - 1]).TypeGroup;
                this.itemsControl.ItemTemplate = (DataTemplate)this.FindResource(type_b + "Template");
                this.itemsControl.ItemsSource = SampleDataSource.GetGroup(sampleDataItem.NewGroup);
            }
            if (history[history.Count - 1] == "Menu")
            {
                backButton.Visibility = System.Windows.Visibility.Hidden;
            }
            //}
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            UIInvoked();
            BackgroungVideo.Stop();
            BackgroungVideo.Visibility = Visibility.Collapsed;
            MenuButton.Visibility = Visibility.Collapsed;
        }


        private static void CheckPersonIsRemoved(object state)
        {
            try
            {
                instance?.UI(() => instance.CheckTime());

                if (DateTime.Now - LastUIOperation > TimeSpan.FromMinutes(1))
                {
                    instance?.UI(() =>
                    {
                        if (instance.BackgroungVideo.Visibility != Visibility.Visible)
                        {
                            if (handHelper.IsHover)
                            {
                                LastUIOperation += TimeSpan.FromSeconds(10);
                                return;
                            }
                            instance.MenuButton.Visibility = Visibility.Collapsed;
                            instance.BackgroungVideo.Visibility = Visibility.Visible;
                            instance.BackgroungVideo.Play();
                        }
                    });
                }
                if (DateTime.Now - LastUIOperation > TimeSpan.FromSeconds(15))
                {
                    instance?.UI(() =>
                    {
                        if (instance.MenuButton.Visibility == Visibility.Visible)
                        {
                            instance.MenuButton.Visibility = Visibility.Collapsed;
                        }
                    });
                } 
            }
            catch (Exception)
            {
            }
        }

        public static void UIInvoked(DateTime dateTime = default)
        {
            if (dateTime == default)
            {
                LastUIOperation = DateTime.Now;
            }
            else
            {
                LastUIOperation = dateTime;
            }
        }

        private void UI(Action action)
        {
            Dispatcher.Invoke(action);
        }

        public static void Log(string m)
        {
            System.IO.File.AppendAllLines("./logs.txt", new string[] { m });
        }

        private void scrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            UIInvoked();
        }
    }
}
