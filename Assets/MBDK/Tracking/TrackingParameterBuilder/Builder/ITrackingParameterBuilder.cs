using System;
using MBDK.Tracking.TrackingParameterBuilder.CustomParameters;
using Firebase.Analytics;

namespace MBDK.Tracking.TrackingParameterBuilder.Builder
{
    public interface ITrackingParameterBuilder : IDisposable
    {
        public Parameter[] GetParameters();
        public void AddParameter(IntTrackingParameter parameter);
        public void AddParameter(LongTrackingParameter parameter);
        public void AddParameter(FloatTrackingParameter parameter);
        public void AddParameter(DoubleTrackingParameter parameter);
        public void AddParameter(StringTrackingParameter parameter);
        public void AddParameter(string parameterName, int parameterValue);
        public void AddParameter(string parameterName, long parameterValue);
        public void AddParameter(string parameterName, float parameterValue);
        public void AddParameter(string parameterName, double parameterValue);
        public void AddParameter(string parameterName, string parameterValue);
        public void ClearParameters();
    }
}
