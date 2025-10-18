using MBDK.Tracking.TrackingParameterBuilder.Interfaces;

namespace MBDK.Tracking.TrackingParameterBuilder.CustomParameters
{
    public readonly struct StringTrackingParameter : IStringTrackingParameter
    {
        private readonly string _parameterValue;
        
        public string ParameterName { get; }

        public StringTrackingParameter(string parameterName, string parameterValue)
        {
            this.ParameterName = parameterName;
            this._parameterValue = parameterValue;
        }
        
        public string GetStringParameterValue() => this._parameterValue;
    }
}