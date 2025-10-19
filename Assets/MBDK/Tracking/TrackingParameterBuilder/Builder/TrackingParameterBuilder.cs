using System;
using System.Collections.Generic;
using MBDK.Tracking.TrackingParameterBuilder.CustomParameters;
using MBDK.Tracking.TrackingParameterBuilder.Interfaces;

namespace MBDK.Tracking.TrackingParameterBuilder.Builder
{
    public class TrackingParameterBuilder : ITrackingParameterBuilder, IDisposable
    {
        private readonly List<ITrackerParameter> _parameters = new();

        public int ParameterCount => this._parameters.Count;
        public List<ITrackerParameter> GetParameters() => this._parameters;

        public void AddParameter(ITrackerParameter parameter)
        {
            this._parameters.Add(parameter);
        }

        public void AddParameter(IntTrackingParameter parameter)
        {
            this._parameters.Add(parameter);
        }

        public void AddParameter(LongTrackingParameter parameter)
        {
            this._parameters.Add(parameter);
        }

        public void AddParameter(FloatTrackingParameter parameter)
        {
            this._parameters.Add(parameter);
        }

        public void AddParameter(DoubleTrackingParameter parameter)
        {
            this._parameters.Add(parameter);
        }

        public void AddParameter(StringTrackingParameter parameter)
        {
            this._parameters.Add(parameter);
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
            this._parameters.Clear();
        }

        public void Dispose()
        {
            this.ClearParameters();
        }
    }
}