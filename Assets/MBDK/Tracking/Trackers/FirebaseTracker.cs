using System.Collections.Generic;
using MBDK.Tracking.TrackingParameterBuilder.Builder;
using MBDK.Tracking.TrackingParameterBuilder.Interfaces;
using Firebase.Analytics;
using MBDK.Tracking.Manager;
using UnityEngine;

namespace MBDK.Tracking.Trackers
{
    public class FirebaseTracker : ITracker
    {
        private const string TrackerTag = "FirebaseTracker";
        
        public TrackerType TrackerType => TrackerType.Firebase;

        public void InjectDependencies(ITrackerManager trackerManager)
        {
            
        }

        public void Start()
        {
            
        }

        public void LogEvent(string eventName)
        {
            FirebaseAnalytics.LogEvent(eventName);
            Debug.Log($"[{TrackerTag}] Tracked event: {eventName}");
        }

        public void LogEvent(string eventName, string parameterName, int parameterValue)
        {
            FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
            Debug.Log($"[{TrackerTag}] Tracked event: {eventName}, parameter: {parameterName}, value: {parameterValue}");
        }

        public void LogEvent(string eventName, string parameterName, long parameterValue)
        {
            FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
            Debug.Log($"[{TrackerTag}] Tracked event: {eventName}, parameter: {parameterName}, value: {parameterValue}");
        }

        public void LogEvent(string eventName, string parameterName, string parameterValue)
        {
            FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
            Debug.Log($"[{TrackerTag}] Tracked event: {eventName}, parameter: {parameterName}, value: {parameterValue}");
        }

        public void LogEvent(string eventName, string parameterName, float parameterValue)
        {
            FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
            Debug.Log($"[{TrackerTag}] Tracked event: {eventName}, parameter: {parameterName}, value: {parameterValue}");
        }

        public void LogEvent(string eventName, string parameterName, double parameterValue)
        {
            FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
            Debug.Log($"[{TrackerTag}] Tracked event: {eventName}, parameter: {parameterName}, value: {parameterValue}");
        }

        public void LogEvent(string eventName, ITrackingParameterBuilder trackingParameterBuilder)
        {
            Parameter[] parameters = trackingParameterBuilder.GetParameters();
            FirebaseAnalytics.LogEvent(eventName, parameters);
            trackingParameterBuilder.Dispose();
        }

        public void SetUserProperty(string propertyName, string propertyValue)
        {
            FirebaseAnalytics.SetUserProperty(propertyName, propertyValue);
            Debug.Log($"[{TrackerTag}] Set user property: {propertyName}, value: {propertyValue}");
        }
    }
}
