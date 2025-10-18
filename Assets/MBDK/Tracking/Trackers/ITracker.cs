using System.Collections.Generic;
using MBDK.Tracking.Manager;
using MBDK.Tracking.TrackingParameterBuilder.Builder;
using MBDK.Tracking.TrackingParameterBuilder.Interfaces;

namespace MBDK.Tracking.Trackers
{
    public interface ITracker
    {
        public TrackerType TrackerType { get; }
        public void InjectDependencies(ITrackerManager trackerManager);
        public void Start();
        public void LogEvent(string eventName);
        public void LogEvent(string eventName, string parameterName, int parameterValue);
        public void LogEvent(string eventName, string parameterName, long parameterValue);
        public void LogEvent(string eventName, string parameterName, string parameterValue);
        public void LogEvent(string eventName, string parameterName, float parameterValue);
        public void LogEvent(string eventName, string parameterName, double parameterValue);
        public void LogEvent(string eventName, ITrackingParameterBuilder trackingParameterBuilder);
    }
}
