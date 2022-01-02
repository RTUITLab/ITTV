using System;
using Microsoft.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;

namespace ITTV.WPF.Interface.Common
{
    public sealed class GestureDetector : IDisposable
    {
        private const string GestureDatabase = @"GesturesDatabase\KinectGesture.gbd";

        private VisualGestureBuilderFrameSource vgbFrameSource;

        private VisualGestureBuilderFrameReader vgbFrameReader;

        public event Action OnGestureFired;
        
        public GestureDetector(KinectSensor kinectSensor)
        {
            if (kinectSensor == null)
            {
                throw new ArgumentNullException(nameof(kinectSensor));
            }

            vgbFrameSource = new VisualGestureBuilderFrameSource(kinectSensor, 0);
            vgbFrameSource.TrackingIdLost += Source_TrackingIdLost;

            vgbFrameReader = vgbFrameSource.OpenReader();
            if (vgbFrameReader != null)
            {
                vgbFrameReader.IsPaused = false;
                vgbFrameReader.FrameArrived += Reader_GestureFrameArrived;
            }

            using var database = new VisualGestureBuilderDatabase(GestureDatabase);
            foreach (var gesture in database.AvailableGestures)
            {
                vgbFrameSource.AddGesture(gesture);
            }
        }
        
        public ulong TrackingId
        {
            get => vgbFrameSource.TrackingId;

            set
            {
                if (vgbFrameSource.TrackingId != value)
                {
                    vgbFrameSource.TrackingId = value;
                }
            }
        }
        
        public bool IsPaused
        {
            set
            {
                if (vgbFrameReader.IsPaused != value)
                {
                    vgbFrameReader.IsPaused = value;
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
                if (vgbFrameReader != null)
                {
                    vgbFrameReader.FrameArrived -= Reader_GestureFrameArrived;
                    vgbFrameReader.Dispose();
                    vgbFrameReader = null;
                }

                if (vgbFrameSource != null)
                {
                    vgbFrameSource.TrackingIdLost -= Source_TrackingIdLost;
                    vgbFrameSource.Dispose();
                    vgbFrameSource = null;
                }
            }
        }
        private static readonly string[] GestureNames = { "HUI1", "HUI2", "HUI3" };


        private DateTime lastCoolGesture;
        private int currentWantedGesture;

        private Gesture lastGesture;
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
                    foreach (var gesture in vgbFrameSource.Gestures)
                    {
                        discreteResults.TryGetValue(gesture, out var result);

                        if (result == null)
                            continue;
                        if (result.Detected)
                        {
                            localGesture = gesture;
                        }
                        // update the GestureResultView object with new gesture result values
                        //this.GestureResultView.UpdateGestureResult(true, result.Detected, result.Confidence);

                    }
                }
            }
           
            if (currentWantedGesture == GestureNames.Length)
            {
                OnGestureFired?.Invoke();
                currentWantedGesture = 0;
                lastGesture = null;
                return;
            }
            if (GestureNames[currentWantedGesture] == localGesture?.Name)
            {
                lastGesture = localGesture;
                lastCoolGesture = DateTime.UtcNow;
                currentWantedGesture++;
                return;
            }
            if (lastGesture != null && DateTime.UtcNow - lastCoolGesture > TimeSpan.FromSeconds(5))
            {
                currentWantedGesture = 0;
                lastGesture = null;
            }
        }

        /// <summary>
        /// Handles the TrackingIdLost event for the VisualGestureBuilderSource object
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Source_TrackingIdLost(object sender, TrackingIdLostEventArgs e)
        {
            // update the GestureResultView object to show the 'Not Tracked' image in the UI
            //this.GestureResultView.UpdateGestureResult(false, false, 0.0f);
        }
    }
}
