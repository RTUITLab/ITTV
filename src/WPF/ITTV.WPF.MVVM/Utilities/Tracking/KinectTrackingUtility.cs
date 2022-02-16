using System;
using ITTV.WPF.Core.Services;
using Microsoft.Kinect.Wpf.Controls;
using Serilog;

namespace ITTV.WPF.MVVM.Utilities.Tracking
{
    public class KinectTrackingUtility
    {
        private readonly UserInterfaceManager _userInterfaceManager;
        private  KinectRegion _kinectRegion;
        
        public bool IsHoverNow { get; private set; }
        
        public event Action OnHoverStart;
        public event Action OnHoverEnd;

        public KinectTrackingUtility(UserInterfaceManager userInterfaceManager)
        {
            _userInterfaceManager = userInterfaceManager;
        }

        public void SetKinectRegion(KinectRegion kinectRegion)
            => _kinectRegion = kinectRegion;
        public void HandsStatusUpdate()
        {
            if (_kinectRegion == null)
            {
                Log.Logger.Warning("KinectTrackingUtility hands status update skipped");
                return;
            }
            
            var hoverNow = _kinectRegion?.EngagedBodyTrackingIds?.Count > 0;

            if (hoverNow)
            {
                var dateTimeNow = DateTime.Now;

                _userInterfaceManager.TryUpdateLastActivityTime(dateTimeNow);
                
                if (!IsHoverNow)
                {
                    Log.Logger.Warning("Body status changed");
                    OnHoverStart?.Invoke();
                }
            }
            else if (IsHoverNow)
            {
                Log.Logger.Warning("Body status changed");
                OnHoverEnd?.Invoke();
            }
            IsHoverNow = hoverNow;
        }
    }
}