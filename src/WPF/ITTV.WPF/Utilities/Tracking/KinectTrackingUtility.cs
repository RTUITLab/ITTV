using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ITTV.WPF.Commands.EggVideos;
using ITTV.WPF.Core.Helpers;
using ITTV.WPF.Core.Models;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using Microsoft.Extensions.Options;
using Microsoft.Kinect;
using Microsoft.Kinect.Wpf.Controls;
using Serilog;

namespace ITTV.WPF.Utilities.Tracking
{
    public class KinectTrackingUtility
    {
        private readonly UserInterfaceManager _userInterfaceManager;
        private readonly NavigationStore _navigationStore;
        
        private readonly Settings _settings;

        private List<Body> _trackingBodies = new();
        
        private List<KinectGestureDetector> _gestureDetectors = new();
        
        private  KinectRegion _kinectRegion;
        
        public bool IsHoverNow { get; private set; }
        
        public event Action OnHoverStart;
        public event Action OnHoverEnd;

        public KinectTrackingUtility(UserInterfaceManager userInterfaceManager,
            NavigationStore navigationStore,
            IOptions<Settings> settings)
        {
            _userInterfaceManager = userInterfaceManager;
            _navigationStore = navigationStore;

            _settings = settings.Value;
            if (_settings.EggVideoCommands != null)
            {
                KinectGestureDetector.SetGestureCommands(_settings.EggVideoCommands);
            }
        }

        public void SetKinectRegion(KinectRegion kinectRegion)
        { 
            _kinectRegion = kinectRegion;

            if (_settings.EggVideoCommands?.Length > 0)
            {
                var eggVideoCommandsDatabasePath = PathHelper.FileGestureDatabasePath;
                if (!File.Exists(eggVideoCommandsDatabasePath))
                {
                    Log.Logger.Error("File gesture database not found by path {0}!",
                        PathHelper.FileGestureDatabasePath);
                    return;
                }
                
                var eggVideoPath = PathHelper.FileEggVideoPath;
                if (!File.Exists(eggVideoPath))
                {
                    Log.Logger.Error("File egg video not found by path {0}!",
                        PathHelper.FileGestureDatabasePath);
                    return;
                }
                
                _kinectRegion.KinectSensor = KinectSensor.GetDefault();

                var maxBodiesCount = _kinectRegion.KinectSensor.BodyFrameSource.BodyCount;

                _trackingBodies = new List<Body>(new Body[maxBodiesCount]);


                var navigationCommand = new TryNavigateEggVideoCommand(_navigationStore,
                    _userInterfaceManager,
                    _settings);
                
                _gestureDetectors = Enumerable.Range(0, maxBodiesCount)
                    .Select(_ =>
                    {
                        var kinectGestureDetector = new KinectGestureDetector(_kinectRegion.KinectSensor);
                        kinectGestureDetector.OnGestureFired += () => navigationCommand.Execute(null);

                        return kinectGestureDetector;
                    })
                    .ToList();
                
                var bodyFrameReader = _kinectRegion.KinectSensor.BodyFrameSource.OpenReader();
                bodyFrameReader.FrameArrived += BodyFrameReaderOnFrameArrived;
            }
        }

        private void BodyFrameReaderOnFrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            var dataReceived = false;

            using (var bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    _trackingBodies ??= new List<Body>(new Body[bodyFrame.BodyCount].ToList());

                    bodyFrame.GetAndRefreshBodyData(_trackingBodies);
                    dataReceived = true;
                }
            }

            if (dataReceived)
            {
                if (_trackingBodies == null) 
                    return;
                
                _gestureDetectors.ForEach(x =>
                {
                    x.IsPaused = false;
                    x.TrackingId = 0;
                });
                    
                var maxBodies = _kinectRegion.KinectSensor.BodyFrameSource.BodyCount;
                    
                for (var i = 0; i < maxBodies; ++i)
                {
                    if (i >= _trackingBodies.Count) continue;
                    var body = _trackingBodies[i];
                    var trackingId = body.TrackingId;

                    if (maxBodies > _gestureDetectors.Count
                        || trackingId == _gestureDetectors[i].TrackingId) 
                        continue;
                        
                    _gestureDetectors[i].TrackingId = trackingId;
                    _gestureDetectors[i].IsPaused = trackingId == 0;
                }
            }
        }

        public void HandsStatusUpdate()
        {
            if (_kinectRegion == null)
            {
                Log.Logger.Warning("KinectTrackingUtility hands status update skipped because KinectRegion not configured");
                return;
            }
            
            var hoverNow = _kinectRegion?.EngagedBodyTrackingIds?.Count > 0;
            throw new ArgumentException();

            if (hoverNow)
            {
                var dateTimeNow = DateTime.Now;

                _userInterfaceManager.TryUpdateLastActivityTime(dateTimeNow);
                
                if (!IsHoverNow)
                {
                    Log.Logger.Information("Body status changed, is tracking now : {0}", IsHoverNow);
                    OnHoverStart?.Invoke();
                }
            }
            else if (IsHoverNow)
            {
                Log.Logger.Information("Body status changed, is tracking now : {0}", IsHoverNow);
                OnHoverEnd?.Invoke();
            }
            IsHoverNow = hoverNow;
        }
    }
}