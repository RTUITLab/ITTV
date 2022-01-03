using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ITTV.WPF.DataModel;
using ITTV.WPF.Interface.Common;
using ITTV.WPF.Interface.Pages;
using Microsoft.Kinect;
using Microsoft.Kinect.Wpf.Controls;

namespace ITTV.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow
    {
        private static bool _adminMode = SettingsService.Instance.IsAdmin;

        public readonly HandOverHelper handHelper;

        public static MainWindow Instance;
        private DateTime lastUiOperation = DateTime.Now;

        private List<Body> bodies;
        private readonly List<GestureDetector> gestureDetectorList = new List<GestureDetector>();

        public MainWindow()
        {
            Instance = this;

            InitializeComponent();

            Activate();
            Focus();

            CreateData.Instance.GetAllVideos();
            CreateData.Instance.GetNewsFromFile();
            CreateData.Instance.GetGames();
            CreateData.Instance.GetAllTimetable();

            NewsUpdateThread.Instance.StartUpdating();

            AppDomain.CurrentDomain.UnhandledException += (sender, s) =>
            {
                var e = (Exception) s.ExceptionObject;

                Log(e.ToString());
            };

            if (!_adminMode)
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
            var bodyFrameReader = kinectRegion.KinectSensor.BodyFrameSource.OpenReader();
            bodyFrameReader.FrameArrived += Reader_BodyFrameArrived;

            var timeTimer = new DispatcherTimer(
                TimeSpan.FromSeconds(1),
                DispatcherPriority.Normal,
                (sender, e) =>
                {
                    var dateTime = DateTime.Now;
                    Time.Text = MireaDateTime.GetTime(dateTime);
                    Para.Text = MireaDateTime.GetPara(dateTime);
                    Date.Text = MireaDateTime.GetDay(dateTime);
                    Week.Text = MireaDateTime.GetWeek(dateTime);
                },
                Dispatcher);

            new DispatcherTimer(
                TimeSpan.FromMilliseconds(100),
                DispatcherPriority.Normal,
                (sender, e) => { CheckPersonIsRemoved(); },
                Dispatcher);

            handHelper = new HandOverHelper(kinectRegion, Dispatcher);

            var gesturePath = $@"{AppDomain.CurrentDomain.BaseDirectory}\GesturesDatabase\KinectGesture.gbd";
            if (File.Exists(gesturePath))
            {
                var maxBodies = kinectRegion.KinectSensor.BodyFrameSource.BodyCount;
                for (var i = 0; i < maxBodies; ++i)
                {
                    var detector = new GestureDetector(kinectRegion.KinectSensor);
                    detector.OnGestureFired += () => { content.NavigateTo(new EggVideo()); };
                    gestureDetectorList.Add(detector);
                }
            }

            ControlsBasicsWindow.Topmost = !_adminMode;

            SettingsService.Instance.SettingsUpdated += Settings_SettingsUpdated;

            content.OpenBackgroundVideo();
        }

        private void Settings_SettingsUpdated()
        {
            _adminMode = SettingsService.Instance.IsAdmin;
            
            Ui(() =>
            {
                ControlsBasicsWindow.Topmost = !_adminMode;

                if (!_adminMode)
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
            var dataReceived = false;

            using (var bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    bodies ??= new List<Body>(new Body[bodyFrame.BodyCount].ToList());

                    bodyFrame.GetAndRefreshBodyData(bodies);
                    dataReceived = true;
                }
            }

            if (!dataReceived) return;
            if (bodies == null) return;
            var maxBodies = kinectRegion.KinectSensor.BodyFrameSource.BodyCount;
            for (var i = 0; i < maxBodies; ++i)
            {
                if (i >= bodies.Count)
                    continue;
                var body = bodies[i];
                var trackingId = body.TrackingId;

                if (maxBodies >= gestureDetectorList.Count)
                    continue;
                if (trackingId == gestureDetectorList[i].TrackingId)
                    continue;
                gestureDetectorList[i].TrackingId = trackingId;
                gestureDetectorList[i].IsPaused = trackingId == 0;
            }
        }

        /// <summary>
        /// Handle the back button click.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void GoBack(object sender, RoutedEventArgs e)
        {
            UiInvoked();
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

        public void UiInvoked(DateTime dateTime = default)
        {
            lastUiOperation = dateTime == default ? DateTime.Now : dateTime;
        }

        public void Ui(Action action)
        {
            Dispatcher.Invoke(action);
        }

        public static void Log(string m)
        {
            try
            {
                File.AppendAllLines("./logs.txt", new[] { DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToLongTimeString() + "\t\t" + m });
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void CheckPersonIsRemoved()
        {
            try
            {
                Instance?.Ui(() =>
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
                        if (DateTime.Now - lastUiOperation > TimeSpan.FromMinutes(1))
                        {
                            if (Instance.content.ContentType() != typeof(BackgroundVideo) && Instance.content.ContentType() != typeof(EggVideo))
                            {
                                if (handHelper.IsHover)
                                {
                                    lastUiOperation += TimeSpan.FromSeconds(10);
                                    return;
                                }
                                content.OpenBackgroundVideo();
                            }
                        }

                        if (DateTime.Now - lastUiOperation <= TimeSpan.FromSeconds(15)) return;
                        if (Instance.content.ContentType() != typeof(BackgroundVideo)) return;
                        if (Instance.content.GetContent() is BackgroundVideo bv && !bv.IsButtonInvisible() && !_adminMode)
                        {
                            bv.SetButtonVisibility(Visibility.Collapsed);
                        }
                    }
                });
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void SetBlackTheme()
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

        public void SetWhiteTheme()
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
