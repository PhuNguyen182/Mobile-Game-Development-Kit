using System;
using System.Collections.Generic;
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

        public void RemoveParameter(ITrackerParameter parameter)
        {
            this._parameters.Remove(parameter);
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