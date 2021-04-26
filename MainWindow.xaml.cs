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
    using System.IO;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;
    using Microsoft.Kinect;
    using Microsoft.Kinect.Wpf.Controls;
    using Microsoft.Samples.Kinect.ControlsBasics.DataModel;
    using Microsoft.Samples.Kinect.ControlsBasics.Interface.Pages;
    using Microsoft.Samples.Kinect.ControlsBasics.Network.Controll;
    using Microsoft.Samples.Kinect.ControlsBasics.TVSettings;
    using Microsoft.Samples.Kinect.DiscreteGestureBasics;

    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow
    {
        private static bool adminMode = Settings.Instance.IsAdmin;

        public HandOverHelper HandHelper;

        public static MainWindow Instance;
        private DateTime LastUIOperation = DateTime.Now;

        private Body[] bodies = null;
        public List<GestureDetector> gestureDetectorList = new List<GestureDetector>();

        private List<Delegate> cloasingTasks = new List<Delegate>();

        public MainWindow()
        {
            Instance = this;

            this.InitializeComponent();

            CreateData.Instance.GetAllVideos();
            CreateData.Instance.GetNewsFromFile();
            CreateData.Instance.GetGames();
            CreateData.Instance.GetAllTimetable();

            NewsUpdateThread.Instance.StartUpdating();

            AppDomain.CurrentDomain.UnhandledException += (e, s) =>
            {
                Log(s.ToString());
            };

            if (!adminMode)
            {
                AppDomain.CurrentDomain.ProcessExit += ReOpenApp;
                AppDomain.CurrentDomain.UnhandledException += ReOpenAppInException;
                Cursor = Cursors.None;
            }
            else
            {
                Cursor = Cursors.Arrow;
            }

            KinectRegion.SetKinectRegion(this, kinectRegion);

            ((App)Application.Current).KinectRegion = kinectRegion;
            kinectRegion.KinectSensor = KinectSensor.GetDefault();
            BodyFrameReader bodyFrameReader = this.kinectRegion.KinectSensor.BodyFrameSource.OpenReader();
            bodyFrameReader.FrameArrived += this.Reader_BodyFrameArrived;

            DispatcherTimer TimeTimer = new DispatcherTimer(
                TimeSpan.FromSeconds(1),
                DispatcherPriority.Normal,
                (sender, e) =>
                {
                    DateTime dateTime = DateTime.Now; //new DateTime(DateTime.Now.Year, 2, 10, 12, 20, 00);
                    Time.Text = MireaDateTime.Instance.GetTime(dateTime);
                    Para.Text = MireaDateTime.Instance.GetPara(dateTime);
                    Date.Text = MireaDateTime.Instance.GetDay(dateTime);
                    Week.Text = MireaDateTime.Instance.GetWeek(dateTime);
                },
                Dispatcher);

            DispatcherTimer CheckOutTimer = new DispatcherTimer(
                TimeSpan.FromMilliseconds(100),
                DispatcherPriority.Normal,
                (sender, e) => { CheckPersonIsRemoved(); },
                Dispatcher);

            HandHelper = new HandOverHelper(kinectRegion, Dispatcher);

            string GesturePath = $@"{AppDomain.CurrentDomain.BaseDirectory}\GesturesDatabase\KinectGesture.gbd";
            if (File.Exists(GesturePath))
            {
                int maxBodies = this.kinectRegion.KinectSensor.BodyFrameSource.BodyCount;
                for (int i = 0; i < maxBodies; ++i)
                {
                    GestureDetector detector = new GestureDetector(this.kinectRegion.KinectSensor);
                    detector.OnGestureFired += () => { content.NavigateTo(new EggVideo()); };
                    this.gestureDetectorList.Add(detector);
                }
            }

            ControlsBasicsWindow.Topmost = !adminMode;

            Settings.Instance.SettingsUpdated += Settings_SettingsUpdated;

            content.OpenBackgroundVideo();
        }

        private void Settings_SettingsUpdated()
        {
            adminMode = Settings.Instance.IsAdmin;
            UI(() =>
            {
                ControlsBasicsWindow.Topmost = !adminMode;

                if (!adminMode)
                {
                    AppDomain.CurrentDomain.ProcessExit += ReOpenApp;
                    AppDomain.CurrentDomain.UnhandledException += ReOpenAppInException;
                    Cursor = Cursors.None;
                }
                else
                {
                    AppDomain.CurrentDomain.ProcessExit -= ReOpenApp;
                    AppDomain.CurrentDomain.UnhandledException -= ReOpenAppInException;
                    Cursor = Cursors.Arrow;
                }
            });
        }

        private void ReOpenApp(object sender, EventArgs e)
        {
            NewsUpdateThread.Instance.StopUpdating();
            Process.Start("ControlsBasics-WPF.exe");
        }

        private void ReOpenAppInException(object sender, UnhandledExceptionEventArgs e)
        { 
            NewsUpdateThread.Instance.StopUpdating();
            Process.Start("ControlsBasics-WPF.exe");
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Handles the body frame data arriving from the sensor and updates the associated gesture detector object for each body
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            bool dataReceived = false;

            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (bodies == null)
                    {
                        bodies = new Body[bodyFrame.BodyCount];
                    }

                    bodyFrame.GetAndRefreshBodyData(this.bodies);
                    dataReceived = true;
                }
            }

            if (dataReceived)
            {
                if (bodies != null)
                {
                    int maxBodies = this.kinectRegion.KinectSensor.BodyFrameSource.BodyCount;
                    for (int i = 0; i < maxBodies; ++i)
                    {
                        Body body = this.bodies[i];
                        ulong trackingId = body.TrackingId;

                        if (maxBodies < this.gestureDetectorList.Count)
                        {
                            if (trackingId != this.gestureDetectorList[i].TrackingId)
                            {
                                gestureDetectorList[i].TrackingId = trackingId;

                                gestureDetectorList[i].IsPaused = trackingId == 0;
                            }
                        }
                    }
                }
            }
        }





        /// <summary>
        /// Handle the back button click.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void GoBack(object sender, RoutedEventArgs e)
        {
            UIInvoked();
            if (content.CanGoBackFuther())
            {
                content.GoBack();
            }
            else
            {
                content.GoBack();
                backButton.Visibility = Visibility.Hidden;
            }
        }

        public void UIInvoked(DateTime dateTime = default)
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

        public void UI(Action action)
        {
            Dispatcher.Invoke(action);
        }

        public void Log(string m)
        {
            try
            {
                File.AppendAllLines("./logs.txt", new string[] { DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToLongTimeString() + "\t\t" + m });
            }
            catch (Exception e) { }
        }

        private void CheckPersonIsRemoved()
        {
            try
            {
                Instance?.UI(() =>
                {
                    if (!MireaDateTime.Instance.WorkTime())
                    {
                        if (Instance.content.ContentType() != typeof(NightPhoto))
                        {
                            content.OpenNightPhoto();
                        }
                    }
                    else
                    {
                        if (DateTime.Now - LastUIOperation > TimeSpan.FromMinutes(1))
                        {
                            if (Instance.content.ContentType() != typeof(BackgroundVideo) && Instance.content.ContentType() != typeof(EggVideo))
                            {
                                if (HandHelper.IsHover)
                                {
                                    LastUIOperation += TimeSpan.FromSeconds(10);
                                    return;
                                }
                                content.OpenBackgroundVideo();
                            }
                        }
                        if (DateTime.Now - LastUIOperation > TimeSpan.FromSeconds(15))
                        {
                            if (Instance.content.ContentType() == typeof(BackgroundVideo))
                            {
                                BackgroundVideo bv = Instance.content.GetContent() as BackgroundVideo;
                                if (!bv.IsButtonInvisible() && !adminMode)
                                {
                                    bv.SetButtonVisibility(Visibility.Collapsed);
                                }
                            }

                        }
                    }
                });
            }
            catch (Exception) { }
        }

        public void setBlackTheme()
        {
            IIT.Foreground = Brushes.White;
            Lab.Foreground = Brushes.White;
            Time.Foreground = Brushes.White;
            Date.Foreground = Brushes.White;
            Para.Foreground = Brushes.White;
            Week.Foreground = Brushes.White;

            Sep.Fill = Brushes.White;
            ControlsBasicsWindow.Background = Brushes.Black;

            ITLogoImage.Source = FindResource("BlackITLogo") as ImageSource;
            LabLogoImage.Source = FindResource("BlackRTUITLabLogo") as ImageSource;
        }

        public void setWhiteTheme()
        {
            IIT.Foreground = Brushes.Black;
            Lab.Foreground = Brushes.Black;
            Time.Foreground = Brushes.Black;
            Date.Foreground = Brushes.Black;
            Para.Foreground = Brushes.Black;
            Week.Foreground = Brushes.Black;

            Sep.Fill = Brushes.Black;
            ControlsBasicsWindow.Background = Brushes.White;

            ITLogoImage.Source = FindResource("ITLogo") as ImageSource;
            LabLogoImage.Source = FindResource("RTUITLabLogo") as ImageSource;
        }
    }
}
