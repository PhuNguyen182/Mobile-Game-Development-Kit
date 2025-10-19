using MBDK.Tracking.Trackers;
using MBDK.Tracking.TrackingParameterBuilder.Builder;

namespace MBDK.Tracking.Manager
{
    public interface ITrackerManager
    {
        public ITracker GetTracker(TrackerType trackerType);
        public void InjectAllTrackers();
        public void StartAllTrackers();
        public void AddTracker(ITracker tracker);
        public void RemoveTracker(ITracker tracker);
        public void LogEvent(string eventName);
        public void LogEvent(string eventName, ITrackingParameterBuilder trackingParameterBuilder);
        public void ClearTrackers();
    }
}
