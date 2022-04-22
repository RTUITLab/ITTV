using System;
using ITTV.WPF.Core.Helpers;
using Microsoft.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;

namespace ITTV.WPF.Utilities.Tracking
{
    public class KinectGestureDetector : IDisposable
    {
        public event Action OnGestureFired;

        private static string[] _gestureCommands;
        /// <summary> Gesture frame source which should be tied to a body tracking ID </summary>
        private VisualGestureBuilderFrameSource _vgbFrameSource;
        /// <summary> Gesture frame reader which will handle gesture events coming from the sensor </summary>
        private VisualGestureBuilderFrameReader _vgbFrameReader;
        
        public static void SetGestureCommands(string[] gestureCommands)
            => _gestureCommands = gestureCommands;

        /// <summary>
        /// Initializes a new instance of the GestureDetector class along with the gesture frame source and reader
        /// </summary>
        /// <param name="kinectSensor">Active sensor to initialize the VisualGestureBuilderFrameSource object with</param>
        public KinectGestureDetector(KinectSensor kinectSensor)
        {
            if (_gestureCommands == null)
                throw new ArgumentException(nameof(_gestureCommands));
            
            if (kinectSensor == null)
            {
                throw new ArgumentNullException(nameof(kinectSensor));
            }


            // create the vgb source. The associated body tracking ID will be set when a valid body frame arrives from the sensor.
            _vgbFrameSource = new VisualGestureBuilderFrameSource(kinectSensor, 0);
            _vgbFrameSource.TrackingIdLost += Source_TrackingIdLost;

            // open the reader for the vgb frames
            _vgbFrameReader = _vgbFrameSource.OpenReader();
            if (_vgbFrameReader != null)
            {
                _vgbFrameReader.IsPaused = false;
                _vgbFrameReader.FrameArrived += Reader_GestureFrameArrived;
            }

            using var database = new VisualGestureBuilderDatabase(PathHelper.FileGestureDatabasePath);
            // we could load all available gestures in the database with a call to vgbFrameSource.AddGestures(database.AvailableGestures), 
            // but for this program, we only want to track one discrete gesture from the database, so we'll load it by name
            foreach (var gesture in database.AvailableGestures)
            {
                _vgbFrameSource.AddGesture(gesture);
            }
        }

        /// <summary>
        /// Gets or sets the body tracking ID associated with the current detector
        /// The tracking ID can change whenever a body comes in/out of scope
        /// </summary>
        public ulong TrackingId
        {
            get => _vgbFrameSource.TrackingId;

            set
            {
                if (_vgbFrameSource.TrackingId != value)
                {
                    _vgbFrameSource.TrackingId = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the detector is currently paused
        /// If the body tracking ID associated with the detector is not valid, then the detector should be paused
        /// </summary>
        public bool IsPaused
        {
            get => _vgbFrameReader.IsPaused;

            set
            {
                if (_vgbFrameReader.IsPaused != value)
                {
                    _vgbFrameReader.IsPaused = value;
                }
            }
        }

        /// <summary>
        /// Disposes all unmanaged resources for the class
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the VisualGestureBuilderFrameSource and VisualGestureBuilderFrameReader objects
        /// </summary>
        /// <param name="disposing">True if Dispose was called directly, false if the GC handles the disposing</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_vgbFrameReader != null)
                {
                    _vgbFrameReader.FrameArrived -= Reader_GestureFrameArrived;
                    _vgbFrameReader.Dispose();
                    _vgbFrameReader = null;
                }

                if (_vgbFrameSource != null)
                {
                    _vgbFrameSource.TrackingIdLost -= Source_TrackingIdLost;
                    _vgbFrameSource.Dispose();
                    _vgbFrameSource = null;
                }
            }
        }

        private DateTime _lastCoolGesture;
        private int _currentWantedGesture;

        private Gesture _lastGesture;
        
        /// <summary>
        /// Handles gesture detection results arriving from the sensor for the associated body tracking Id
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_GestureFrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
        {
            Gesture localGesture = null;
            var frameReference = e.FrameReference;
            using (var frame = frameReference.AcquireFrame())
            {
                // get the discrete gesture results which arrived with the latest frame
                var discreteResults = frame?.DiscreteGestureResults;

                if (discreteResults != null)
                {
                    // we only have one gesture in this source object, but you can get multiple gestures
                    foreach (var gesture in _vgbFrameSource.Gestures)
                    {
                        discreteResults.TryGetValue(gesture, out var result);

                        if (result is {Detected: true})
                        {
                            localGesture = gesture;
                        }

                    }
                }
            }
            
            if (_gestureCommands[_currentWantedGesture] == localGesture?.Name)
            {
                _lastGesture = localGesture;
                _lastCoolGesture = DateTime.UtcNow;
                _currentWantedGesture++;

                if (_currentWantedGesture == _gestureCommands.Length)
                {
                    OnGestureFired?.Invoke();
                    _currentWantedGesture = 0;
                    _lastGesture = null;
                    return;
                }
            }

            if (_lastGesture == null || DateTime.UtcNow - _lastCoolGesture <= TimeSpan.FromSeconds(5)) 
                return;
            
            _currentWantedGesture = 0;
            _lastGesture = null;
        }

        /// <summary>
        /// Handles the TrackingIdLost event for the VisualGestureBuilderSource object
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Source_TrackingIdLost(object sender, TrackingIdLostEventArgs e)
        { }
    }
}