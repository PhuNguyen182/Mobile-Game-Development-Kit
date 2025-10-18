using System;
using System.Collections.Generic;
using MBDK.Tracking.Configs;
using MBDK.Tracking.Trackers;
using MBDK.Tracking.TrackingParameterBuilder.Builder;

namespace MBDK.Tracking.Manager
{
    public class TrackerManager : ITrackerManager, IDisposable
    {
        private bool _isDisposed;
        private readonly List<ITracker> _trackers;
        private readonly AdjustConfigScriptableObject _adjustConfig;
        
        public TrackerManager(AdjustConfigScriptableObject adjustConfig)
        {
            _trackers = new List<ITracker>();
            InitializeTrackers();
        }

        private void InitializeTrackers()
        {
            ITracker firebaseTracker = new FirebaseTracker();
            ITracker adjustTracker = new AdjustTracker(_adjustConfig);
            AddTracker(firebaseTracker);
            AddTracker(adjustTracker);
            InjectAllTrackers();
            StartAllTrackers();
        }

        public void InjectAllTrackers()
        {
            for (int i = 0; i < this._trackers.Count; i++)
            {
                this._trackers[i].InjectDependencies(this);
            }
        }

        public void StartAllTrackers()
        {
            for (int i = 0; i < this._trackers.Count; i++)
            {
                this._trackers[i].Start();
            }
        }

        public ITracker GetTracker(TrackerType trackerType)
        {
            for (int i = 0; i < this._trackers.Count; i++)
            {
                if (this._trackers[i].TrackerType == trackerType)
                    return this._trackers[i];
            }
            
            return null;
        }

        public void AddTracker(ITracker tracker)
        {
            this._trackers.Add(tracker);
        }

        public void RemoveTracker(ITracker tracker)
        {
            this._trackers.Remove(tracker);
        }

        public void LogEvent(string eventName)
        {
            for (int i = 0; i < this._trackers.Count; i++)
            {
                this._trackers[i].LogEvent(eventName);
            }
        }

        public void LogEvent(string eventName, ITrackingParameterBuilder trackingParameterBuilder)
        {
            for (int i = 0; i < this._trackers.Count; i++)
            {
                this._trackers[i].LogEvent(eventName, trackingParameterBuilder);
            }
        }

        public void ClearTrackers()
        {
            for (int i = 0; i < this._trackers.Count; i++)
            {
                if (this._trackers[i] is IDisposable disposable)
                    disposable.Dispose();
            }
            
            this._trackers.Clear();
        }

        private void ReleaseUnmanagedResources()
        {
            this.ClearTrackers();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._isDisposed)
                return;
            
            if (disposing)
            {
                ReleaseUnmanagedResources();
            }
            
            this._isDisposed = true;
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
