using System;
using System.Collections.Generic;
using MBDK.Tracking.Configs;
using MBDK.Tracking.Trackers;
using MBDK.Tracking.TrackingParameterBuilder.Builder;

namespace MBDK.Tracking.Manager
{
    public class TrackerManager : ITrackerManager, IDisposable
    {
        private bool isDisposed;
        private readonly List<ITracker> trackers;
        private readonly AdjustConfigScriptableObject adjustConfig;
        
        public TrackerManager(AdjustConfigScriptableObject adjustConfig)
        {
            trackers = new List<ITracker>();
            this.adjustConfig = adjustConfig;
            InitializeTrackers();
        }

        private void InitializeTrackers()
        {
            ITracker firebaseTracker = new FirebaseTracker();
            ITracker adjustTracker = new AdjustTracker(adjustConfig);
            AddTracker(firebaseTracker);
            AddTracker(adjustTracker);
            InjectAllTrackers();
            StartAllTrackers();
        }

        public void InjectAllTrackers()
        {
            for (int i = 0; i < this.trackers.Count; i++)
            {
                this.trackers[i].InjectDependencies(this);
            }
        }

        public void StartAllTrackers()
        {
            for (int i = 0; i < this.trackers.Count; i++)
            {
                this.trackers[i].Start();
            }
        }

        public ITracker GetTracker(TrackerType trackerType)
        {
            for (int i = 0; i < this.trackers.Count; i++)
            {
                if (this.trackers[i].TrackerType == trackerType)
                    return this.trackers[i];
            }
            
            return null;
        }

        public void AddTracker(ITracker tracker)
        {
            this.trackers.Add(tracker);
        }

        public void RemoveTracker(ITracker tracker)
        {
            this.trackers.Remove(tracker);
        }

        public void LogEvent(string eventName)
        {
            for (int i = 0; i < this.trackers.Count; i++)
            {
                this.trackers[i].LogEvent(eventName);
            }
        }

        public void LogEvent(string eventName, ITrackingParameterBuilder trackingParameterBuilder)
        {
            for (int i = 0; i < this.trackers.Count; i++)
            {
                this.trackers[i].LogEvent(eventName, trackingParameterBuilder);
            }
        }

        public void ClearTrackers()
        {
            for (int i = 0; i < this.trackers.Count; i++)
            {
                if (this.trackers[i] is IDisposable disposable)
                    disposable.Dispose();
            }
            
            this.trackers.Clear();
        }

        private void ReleaseUnmanagedResources()
        {
            this.ClearTrackers();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.isDisposed)
                return;
            
            if (disposing)
            {
                ReleaseUnmanagedResources();
            }
            
            this.isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~TrackerManager()
        {
            Dispose(false);
        }
    }
}
