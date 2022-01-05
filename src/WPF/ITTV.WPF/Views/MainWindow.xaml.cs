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
using ITTV.WPF.DataModel.Models;
using ITTV.WPF.Helpers;
using ITTV.WPF.Interface.Common;
using ITTV.WPF.Interface.Pages;
using ITTV.WPF.Services;
using Microsoft.Kinect;
using Microsoft.Kinect.Wpf.Controls;

namespace ITTV.WPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow
    {
        public readonly HandOverHelper handHelper;
        private DateTime lastUiOperation = DateTime.Now;

        private List<Body> bodies;
        private readonly List<GestureDetector> gestureDetectorList = new List<GestureDetector>();

        private readonly CreateData _createData;
        private readonly Settings _settings;
        private readonly NewsUpdateThread _newsUpdateThread;
        private readonly MireaTimeManager mireaTimeManager;
        private readonly EggVideo _eggVideo;

        public MainWindow(CreateData createData, Settings settings,  NewsUpdateThread newsUpdateThread, MireaTimeManager mireaTimeManager, EggVideo eggVideo)
        {
            _createData = createData;
            _settings = settings;
            _newsUpdateThread = newsUpdateThread;
            this.mireaTimeManager = mireaTimeManager;
            _eggVideo = eggVideo;

            InitializeComponent();
            ConfigureEggVideo(eggVideo);
            Activate();
            Focus();

            _createData.GetAllVideos();
            _createData.GetNewsFromFile();
            _createData.GetGames();
            _createData.GetAllTimetable();

            _newsUpdateThread.StartUpdating();

            AppDomain.CurrentDomain.UnhandledException += (sender, s) =>
            {
                var e = (Exception) s.ExceptionObject;

                Log(e.ToString());
            };

            if (!_settings.IsAdmin)
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
                    
                    Date.Text = MireaTimeHelper.GetLongDate(dateTime);
                    Time.Text = MireaTimeHelper.GetLongTime(dateTime);
                    Week.Text = MireaTimeHelper.GetWeekOfSemester(dateTime);
                    
                    Para.Text = string.IsNullOrWhiteSpace(Week.Text)? string.Empty : MireaTimeHelper.GetStageOfClasses(dateTime);
                },
                Dispatcher);

            new DispatcherTimer(
                TimeSpan.FromMilliseconds(100),
                DispatcherPriority.Normal,
                (sender, e) => { CheckPersonIsRemoved(); },
                Dispatcher);

            handHelper = new HandOverHelper(kinectRegion, Dispatcher);

            var gesturePath = AllPaths.FileGestureDatabasePath;
            if (File.Exists(gesturePath))
            {
                var maxBodies = kinectRegion.KinectSensor.BodyFrameSource.BodyCount;
                for (var i = 0; i < maxBodies; ++i)
                {
                    var detector = new GestureDetector(kinectRegion.KinectSensor);
                    detector.OnGestureFired += () => { ContentV2.NavigateTo(_eggVideo); };
                    gestureDetectorList.Add(detector);
                }
            }

            ControlsBasicsWindow.Topmost = !_settings.IsAdmin;

            _settings.SettingsUpdated += Settings_SettingsUpdated;

            ContentV2.OpenBackgroundVideo();
        }

        private void ConfigureEggVideo(EggVideo eggVideo)
        {
            var gesturePath = AllPaths.FileGestureDatabasePath;
            var eggVideoFile = AllPaths.FileEggVideoPath;

            if (!File.Exists(gesturePath) || !File.Exists(eggVideoFile)) 
                return;
            
            eggVideo.EggVideoElement.Source = new Uri(eggVideoFile);
            eggVideo.EggVideoElement.Visibility = Visibility.Collapsed;
            eggVideo.EggVideoElement.MediaEnded += (s, e) =>
            {
                ContentV2.OpenBackgroundVideo();
            };
        }

        private void Settings_SettingsUpdated()
        {
            Ui(() =>
            {
                ControlsBasicsWindow.Topmost = ! _settings.IsAdmin;

                if (!_settings.IsAdmin)
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
            _newsUpdateThread.StopUpdating();
            Process.Start(typeof(App).Assembly.GetName().Name);
        }

        private void ReOpenAppInException(object sender, UnhandledExceptionEventArgs e)
        { 
            _newsUpdateThread .StopUpdating();
            Process.Start(typeof(App).Assembly.GetName().Name);
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
            if (ContentV2.CanGoBackFuther())
            {
                ContentV2.GoBack();
            }
            else
            {
                ContentV2.GoBack();
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
                File.AppendAllLines(AllPaths.FileLogsPath, new[] { DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToLongTimeString() + "\t\t" + m });
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
                Ui(() =>
                {
                    if (!mireaTimeManager.IsWorkTimeNow())
                    {
                        if (ContentV2.ContentType() != typeof(NightPhoto))
                        {
                            ContentV2.OpenNightPhoto();
                        }
                    }
                    else
                    {
                        if (DateTime.Now - lastUiOperation > TimeSpan.FromMinutes(1))
                        {
                            if (ContentV2.ContentType() != typeof(BackgroundVideo) && ContentV2.ContentType() != typeof(EggVideo))
                            {
                                if (handHelper.IsHover)
                                {
                                    lastUiOperation += TimeSpan.FromSeconds(10);
                                    return;
                                }
                                ContentV2.OpenBackgroundVideo();
                            }
                        }

                        if (DateTime.Now - lastUiOperation <= TimeSpan.FromSeconds(15)) return;
                        if (ContentV2.ContentType() != typeof(BackgroundVideo)) return;
                        if (ContentV2.GetContent() is BackgroundVideo bv && !bv.IsButtonInvisible() && !_settings.IsAdmin)
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
