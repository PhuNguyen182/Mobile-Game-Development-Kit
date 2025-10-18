using MBDK.Tracking.TrackingParameterBuilder.Interfaces;

namespace MBDK.Tracking.TrackingParameterBuilder.CustomParameters
{
    public readonly struct IntTrackingParameter : IIntTrackingParameter
    {
        private readonly int _parameterValue;
        
        public string ParameterName { get; }

        public IntTrackingParameter(string parameterName, int parameterValue)
        {
            this.ParameterName = parameterName;
            this._parameterValue = parameterValue;
        }
        
        public int GetIntParameterValue() => this._parameterValue;
    }
}
