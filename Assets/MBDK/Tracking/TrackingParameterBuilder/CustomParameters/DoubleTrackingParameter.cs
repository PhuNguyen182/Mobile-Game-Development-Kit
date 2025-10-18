using MBDK.Tracking.TrackingParameterBuilder.Interfaces;

namespace MBDK.Tracking.TrackingParameterBuilder.CustomParameters
{
    public readonly struct DoubleTrackingParameter : IDoubleTrackingParameter
    {
        private readonly double _parameterValue;
        
        public string ParameterName { get; }

        public DoubleTrackingParameter(string parameterName, double parameterValue)
        {
            this.ParameterName = parameterName;
            this._parameterValue = parameterValue;
        }
        
        public double GetDoubleParameterValue() => this._parameterValue;
    }
}