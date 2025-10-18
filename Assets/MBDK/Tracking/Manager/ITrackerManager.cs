using MBDK.Tracking.Trackers;

namespace MBDK.Tracking.Manager
{
    public interface ITrackerManager
    {
        public void AddTracker(ITracker tracker);
        public void RemoveTracker(ITracker tracker);
    }
}
