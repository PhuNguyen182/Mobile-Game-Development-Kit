using MBDK.Tracking.TrackingParameterBuilder.Interfaces;

namespace MBDK.Tracking.TrackingParameterBuilder.CustomParameters
{
    public readonly struct LongTrackingParameter : ILongTrackingParameter
    {
        private readonly long _parameterValue;
        
        public string ParameterName { get; }

        public LongTrackingParameter(string parameterName, long parameterValue)
        {
            this.ParameterName = parameterName;
            this._parameterValue = parameterValue;
        }
        
        public long GetLongParameterValue() => this._parameterValue;
    }
}