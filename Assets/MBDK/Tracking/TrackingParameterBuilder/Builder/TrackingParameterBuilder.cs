using System.Collections.Generic;
using MBDK.Tracking.TrackingParameterBuilder.CustomParameters;
using Firebase.Analytics;

namespace MBDK.Tracking.TrackingParameterBuilder.Builder
{
    public class TrackingParameterBuilder : ITrackingParameterBuilder
    {
        private readonly List<IntTrackingParameter> _intParameters = new();
        private readonly List<LongTrackingParameter> _longParameters = new();
        private readonly List<FloatTrackingParameter> _floatParameters = new();
        private readonly List<DoubleTrackingParameter> _doubleParameters = new();
        private readonly List<StringTrackingParameter> _stringParameters = new();

        private int ParameterCount =>
            this._intParameters.Count + this._longParameters.Count + this._floatParameters.Count +
            this._doubleParameters.Count + this._stringParameters.Count;

        public void AddParameter(IntTrackingParameter parameter)
        {
            this._intParameters.Add(parameter);
        }

        public void AddParameter(LongTrackingParameter parameter)
        {
            this._longParameters.Add(parameter);
        }

        public void AddParameter(FloatTrackingParameter parameter)
        {
            this._floatParameters.Add(parameter);
        }

        public void AddParameter(DoubleTrackingParameter parameter)
        {
            this._doubleParameters.Add(parameter);
        }

        public void AddParameter(StringTrackingParameter parameter)
        {
            this._stringParameters.Add(parameter);
        }

        public void AddParameter(string parameterName, int parameterValue)
        {
            IntTrackingParameter parameter = new IntTrackingParameter(parameterName, parameterValue);
            this.AddParameter(parameter);
        }

        public void AddParameter(string parameterName, long parameterValue)
        {
            LongTrackingParameter parameter = new LongTrackingParameter(parameterName, parameterValue);
            this.AddParameter(parameter);
        }

        public void AddParameter(string parameterName, float parameterValue)
        {
            FloatTrackingParameter parameter = new FloatTrackingParameter(parameterName, parameterValue);
            this.AddParameter(parameter);
        }

        public void AddParameter(string parameterName, double parameterValue)
        {
            DoubleTrackingParameter parameter = new DoubleTrackingParameter(parameterName, parameterValue);
            this.AddParameter(parameter);
        }

        public void AddParameter(string parameterName, string parameterValue)
        {
            StringTrackingParameter parameter = new StringTrackingParameter(parameterName, parameterValue);
            this.AddParameter(parameter);
        }

        public void ClearParameters()
        {
            this._intParameters.Clear();
            this._longParameters.Clear();
            this._floatParameters.Clear();
            this._doubleParameters.Clear();
            this._stringParameters.Clear();
        }

        public Parameter[] GetParameters()
        {
            int index = 0;
            Parameter[] parameters = new Parameter[ParameterCount];
            
            for (int i = 0; i < this._intParameters.Count; i++)
            {
                var parameterName = this._intParameters[i].ParameterName;
                var parameterValue = this._intParameters[i].GetIntParameterValue();
                Parameter parameter = new Parameter(parameterName, parameterValue);
                parameters[index] = parameter;
                index += 1;
            }
            
            for (int i = 0; i < this._longParameters.Count; i++)
            {
                var parameterName = this._longParameters[i].ParameterName;
                var parameterValue = this._longParameters[i].GetLongParameterValue();
                Parameter parameter = new Parameter(parameterName, parameterValue);
                parameters[index] = parameter;
                index += 1;
            }
            
            for (int i = 0; i < this._floatParameters.Count; i++)
            {
                var parameterName = this._floatParameters[i].ParameterName;
                var parameterValue = this._floatParameters[i].GetFloatParameterValue();
                Parameter parameter = new Parameter(parameterName, parameterValue);
                parameters[index] = parameter;
                index += 1;
            }
            
            for (int i = 0; i < this._doubleParameters.Count; i++)
            {
                var parameterName = this._doubleParameters[i].ParameterName;
                var parameterValue = this._doubleParameters[i].GetDoubleParameterValue();
                Parameter parameter = new Parameter(parameterName, parameterValue);
                parameters[index] = parameter;
                index += 1;
            }
            
            for (int i = 0; i < this._stringParameters.Count; i++)
            {
                var parameterName = this._stringParameters[i].ParameterName;
                var parameterValue = this._stringParameters[i].GetStringParameterValue();
                Parameter parameter = new Parameter(parameterName, parameterValue);
                parameters[index] = parameter;
                index += 1;
            }
            
            return parameters;
        }

        public void Dispose()
        {
            this.ClearParameters();
        }
    }
}