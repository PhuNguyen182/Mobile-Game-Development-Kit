using MBDK.Tracking.TrackingParameterBuilder.Interfaces;

namespace MBDK.Tracking.TrackingParameterBuilder.CustomParameters
{
    public readonly struct FloatTrackingParameter : IFloatTrackingParameter
    {
        private readonly float _parameterValue;
        
        public string ParameterName { get; }

        public FloatTrackingParameter(string parameterName, float parameterValue)
        {
            this.ParameterName = parameterName;
            this._parameterValue = parameterValue;
        }
        
        public float GetFloatParameterValue() => this._parameterValue;
    }
}