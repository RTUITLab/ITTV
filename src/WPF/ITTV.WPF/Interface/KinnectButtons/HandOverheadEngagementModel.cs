using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Kinect;
using Microsoft.Kinect.Input;
using Microsoft.Kinect.Toolkit.Input;

namespace ITTV.WPF.Interface.KinnectButtons
{
    public class HandOverheadEngagementModel : IKinectEngagementManager
    {
        private bool _stopped = true;
        private readonly BodyFrameReader _bodyReader;
        private readonly List<Body> _bodies;
        private bool _engagementPeopleHaveChanged;
        private readonly List<BodyHandPair> _handsToEngage;
        private int _engagedPeopleAllowed;

        public HandOverheadEngagementModel(int engagedPeopleAllowed)
        {
            EngagedPeopleAllowed = engagedPeopleAllowed;
            var sensor = KinectSensor.GetDefault();
            _bodyReader = sensor.BodyFrameSource.OpenReader();
            _bodyReader.FrameArrived += BodyReader_FrameArrived;
            sensor.Open();
            _bodies = new List<Body>();
            _handsToEngage = new List<BodyHandPair>();
        }

        public int EngagedPeopleAllowed
        {
            get => _engagedPeopleAllowed;
            set
            {
                if (value > 2 || value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, "This engagement manager requires 0 to 2 people to be set as the EngagedPeopleAllowed");
                }

                _engagedPeopleAllowed = value;
            }
        }

        public bool EngagedBodyHandPairsChanged()
        {
            return _engagementPeopleHaveChanged;
        }

        public IReadOnlyList<BodyHandPair> KinectManualEngagedHands => KinectCoreWindow.KinectManualEngagedHands;

        public void StartManaging()
        {
            _stopped = false;
            _bodyReader.IsPaused = false;
        }

        public void StopManaging()
        {
            _stopped = true;
            _bodyReader.IsPaused = true;
        }

        private void BodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs args)
        {
            var gotData = false;

            using (var frame = args.FrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    frame.GetAndRefreshBodyData(_bodies);
                    gotData = true;
                }
            }

            if (gotData && !_stopped)
            {
                TrackEngagedPlayersViaHandOverHead();
            }
        }

        private static bool IsHandOverhead(JointType jointType, Body body)
        {
            return (body.Joints[jointType].Position.Y >
                    body.Joints[JointType.Head].Position.Y);
        }

        private static bool IsHandBelowHip(JointType jointType, Body body)
        {
            return (body.Joints[jointType].Position.Y <
                    body.Joints[JointType.SpineBase].Position.Y);
        }

        private void TrackEngagedPlayersViaHandOverHead()
        {
            _engagementPeopleHaveChanged = false;
            var currentlyEngagedHands = KinectCoreWindow.KinectManualEngagedHands;
            _handsToEngage.Clear();

            // check to see if anybody who is currently engaged should be disengaged
            foreach (var bodyHandPair in currentlyEngagedHands)
            {
                var bodyTrackingId = bodyHandPair.BodyTrackingId;
                foreach (var body in _bodies.Where(x => x.TrackingId == bodyTrackingId))
                {
                    var engagedHandJoint = bodyHandPair.HandType == HandType.LEFT ? JointType.HandLeft : JointType.HandRight;
                    var toBeDisengaged = IsHandBelowHip(engagedHandJoint, body);

                    if (toBeDisengaged)
                    {
                        _engagementPeopleHaveChanged = true;
                    }
                    else
                    {
                        _handsToEngage.Add(bodyHandPair);
                    }
                }
            }

            // check to see if anybody should be engaged, if not already engaged
            foreach (var body in _bodies)
            {
                if (_handsToEngage.Count >= _engagedPeopleAllowed)
                    continue;
                var alreadyEngaged = false;
                foreach (var bodyHandPair in _handsToEngage)
                {
                    alreadyEngaged = (body.TrackingId == bodyHandPair.BodyTrackingId);
                }

                if (!alreadyEngaged)
                {
                    // check for engagement
                    if (IsHandOverhead(JointType.HandLeft, body))
                    {
                        // engage the left hand
                        _handsToEngage.Add(
                            new BodyHandPair(body.TrackingId, HandType.LEFT));
                        _engagementPeopleHaveChanged = true;
                    }
                    else if (IsHandOverhead(JointType.HandRight, body))
                    {
                        // engage the right hand
                        _handsToEngage.Add(
                            new BodyHandPair(body.TrackingId, HandType.RIGHT));
                        _engagementPeopleHaveChanged = true;
                    }
                }
            }

            if (_engagementPeopleHaveChanged)
            {
                BodyHandPair firstPersonToEngage = null;
                BodyHandPair secondPersonToEngage = null;

                Debug.Assert(_handsToEngage.Count <= 2, "handsToEngage should be <= 2");

                switch (_handsToEngage.Count)
                {
                    case 0:
                        break;
                    case 1:
                        firstPersonToEngage = _handsToEngage[0];
                        break;
                    case 2:
                        firstPersonToEngage = _handsToEngage[0];
                        secondPersonToEngage = _handsToEngage[1];
                        break;
                }

                switch (EngagedPeopleAllowed)
                {
                    case 1:
                        KinectCoreWindow.SetKinectOnePersonManualEngagement(firstPersonToEngage);
                        break;
                    case 2:
                        KinectCoreWindow.SetKinectTwoPersonManualEngagement(firstPersonToEngage, secondPersonToEngage);
                        break;
                }
            }
        }
    }
}
