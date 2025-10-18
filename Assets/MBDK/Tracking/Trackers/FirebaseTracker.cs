using System.Collections.Generic;
using MBDK.Tracking.TrackingParameterBuilder.Builder;
using MBDK.Tracking.TrackingParameterBuilder.Interfaces;
using Firebase.Analytics;

namespace MBDK.Tracking.Trackers
{
    public class FirebaseTracker : ITracker
    {
        public void LogEvent(string eventName)
        {
            FirebaseAnalytics.LogEvent(eventName);
        }

        public void LogEvent(string eventName, string parameterName, int parameterValue)
        {
            FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
        }

        public void LogEvent(string eventName, string parameterName, long parameterValue)
        {
            FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
        }

        public void LogEvent(string eventName, string parameterName, string parameterValue)
        {
            FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
        }

        public void LogEvent(string eventName, string parameterName, float parameterValue)
        {
            FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
        }

        public void LogEvent(string eventName, string parameterName, double parameterValue)
        {
            FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
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
    }
}
