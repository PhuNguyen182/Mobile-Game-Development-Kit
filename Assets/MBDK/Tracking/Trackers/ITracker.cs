using System.Collections.Generic;
using MBDK.Tracking.TrackingParameterBuilder.Builder;

namespace MBDK.Tracking.Trackers
{
    public interface ITracker
    {
        public void LogEvent(string eventName);
        public void LogEvent(string eventName, string parameterName, int parameterValue);
        public void LogEvent(string eventName, string parameterName, long parameterValue);
        public void LogEvent(string eventName, string parameterName, string parameterValue);
        public void LogEvent(string eventName, string parameterName, float parameterValue);
        public void LogEvent(string eventName, string parameterName, double parameterValue);
        public void LogEvent(string eventName, ITrackingParameterBuilder trackingParameterBuilder);
    }
}
