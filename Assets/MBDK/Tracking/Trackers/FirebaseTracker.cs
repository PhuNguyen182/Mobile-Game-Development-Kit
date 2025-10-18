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

        public void InjectDependencies(ITrackerManager trackerManager)
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
            int parameterCount = trackingParameterBuilder.ParameterCount;
            Parameter[] parameters = new Parameter[parameterCount];
            
            List<ITrackerParameter> trackerParameters = trackingParameterBuilder.GetParameters();
            for (int i = 0; i < trackerParameters.Count; i++)
            {
                parameters[i] = ConvertToFirebaseParameter(trackerParameters[i]);
            }
            
            FirebaseAnalytics.LogEvent(eventName, parameters);
            
            Debug.Log($"[{TrackerTag}] Tracked event: {eventName} with below parameters:");
            for (int i = 0; i < parameters.Length; i++)
            {
                string parameterName = trackerParameters[i].ParameterName;
                string parameterValue = GetParameterValue(trackerParameters[i]);
                Debug.Log(
                    $"[{TrackerTag}] Tracked event: {eventName}, parameter: {parameterName}, value: {parameterValue}");
            }
        }

        private static Parameter ConvertToFirebaseParameter(ITrackerParameter trackerParameter)
        {
            return trackerParameter switch
            {
                IIntTrackingParameter intParameter => new Parameter(intParameter.ParameterName,
                    intParameter.GetIntParameterValue()),
                ILongTrackingParameter longParameter => new Parameter(longParameter.ParameterName,
                    longParameter.GetLongParameterValue()),
                IFloatTrackingParameter floatParameter => new Parameter(floatParameter.ParameterName,
                    floatParameter.GetFloatParameterValue()),
                IDoubleTrackingParameter doubleParameter => new Parameter(doubleParameter.ParameterName,
                    doubleParameter.GetDoubleParameterValue()),
                IStringTrackingParameter stringParameter => new Parameter(stringParameter.ParameterName,
                    stringParameter.GetStringParameterValue()),
                _ => null
            };
        }

        private string GetParameterValue(ITrackerParameter trackerParameter)
        {
            return trackerParameter switch
            {
                IIntTrackingParameter intParameter => $"{intParameter.GetIntParameterValue()}",
                ILongTrackingParameter longParameter => $"{longParameter.GetLongParameterValue()}",
                IFloatTrackingParameter floatParameter => $"{floatParameter.GetFloatParameterValue()}",
                IDoubleTrackingParameter doubleParameter => $"{doubleParameter.GetDoubleParameterValue()}",
                IStringTrackingParameter stringParameter => stringParameter.GetStringParameterValue(),
                _ => null
            };
        }
    }
}
