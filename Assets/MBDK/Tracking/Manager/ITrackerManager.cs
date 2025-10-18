using MBDK.Tracking.Trackers;

namespace MBDK.Tracking.Manager
{
    public interface ITrackerManager
    {
        public ITracker GetTracker(TrackerType trackerType);
        public void AddTracker(ITracker tracker);
        public void RemoveTracker(ITracker tracker);
        public void LogEvent(string eventName);
        public void ClearTrackers();
    }
}
