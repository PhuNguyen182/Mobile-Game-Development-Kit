using System.Collections.Generic;
using MBDK.Tracking.TrackingParameterBuilder.Interfaces;

namespace MBDK.Tracking.TrackingParameterBuilder.Builder
{
    public interface ITrackingParameterBuilder
    {
        public int ParameterCount { get; }
        
        public List<ITrackerParameter> GetParameters();
        public void AddParameter(ITrackerParameter parameter);
        public void RemoveParameter(ITrackerParameter parameter);
        public void ClearParameters();
    }
}
