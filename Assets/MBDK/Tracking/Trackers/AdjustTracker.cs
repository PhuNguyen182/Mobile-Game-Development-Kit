using System;
using AdjustSdk;
using MBDK.Tracking.Configs;
using MBDK.Tracking.Manager;
using MBDK.Tracking.TrackingParameterBuilder.Builder;

namespace MBDK.Tracking.Trackers
{
    public class AdjustTracker : ITracker, IDisposable
    {
        private bool _isDisposed;
        private readonly AdjustConfig _adjustConfig;
        
        public event Action<string> OnGetAdIdEvent;
        public event Action<string> DeferredDeeplinkTrigger;
        public event Action<AdjustAttribution> OnGetAdjustAttribution;
        public event Action<AdjustAttribution> OnAdjustAttributionChanged;

        public AdjustTracker(AdjustConfigScriptableObject config)
        {
            this._adjustConfig = new AdjustConfig(config.appToken, config.environment, config.allowSuppressLogLevel)
            {
                LogLevel = config.logLevel,
                DefaultTracker = config.defaultTracker,
                ExternalDeviceId = config.externalDeviceId,
                IsCoppaComplianceEnabled = config.isCoppaComplianceEnabled,
                IsSendingInBackgroundEnabled = config.isSendingInBackgroundEnabled,
                IsCostDataInAttributionEnabled = config.isCostDataInAttributionEnabled,
                IsDeviceIdsReadingOnceEnabled = config.isDeviceIdsReadingOnceEnabled,
                IsDeferredDeeplinkOpeningEnabled = config.isDeferredDeeplinkOpeningEnabled,
                IsAppTrackingTransparencyUsageEnabled = config.isAppTrackingTransparencyUsageEnabled,
                IsFirstSessionDelayEnabled = config.isFirstSessionDelayEnabled
            };

            this._adjustConfig.DeferredDeeplinkDelegate += this.OnDeferredDeeplinkTrigger;
            this._adjustConfig.AttributionChangedDelegate += OnAttributionChanged;

            if (config.shouldUseDeeplinkUrlStrategyDomain)
            {
                this._adjustConfig.SetUrlStrategy(config.urlStrategyDomains, config.shouldUseSubdomains,
                    config.isDataResidency);
            }
        }

        public TrackerType TrackerType => TrackerType.Adjust;

        public void InjectDependencies(ITrackerManager trackerManager)
        {
            
        }

        public void Start()
        {
            Adjust.InitSdk(this._adjustConfig);
            Adjust.GetAdid(OnGetAdId);
            Adjust.GetAttribution(OnGetAdjustAttribute);
        }

        public void LogEvent(string eventToken)
        {
            AdjustEvent adjustEvent = new AdjustEvent(eventToken);
            Adjust.TrackEvent(adjustEvent);
        }

        public void LogEvent(string eventName, string parameterName, int parameterValue) { }

        public void LogEvent(string eventName, string parameterName, long parameterValue) { }

        public void LogEvent(string eventName, string parameterName, string parameterValue) { }

        public void LogEvent(string eventName, string parameterName, float parameterValue) { }

        public void LogEvent(string eventName, string parameterName, double parameterValue) { }

        public void LogEvent(string eventName, ITrackingParameterBuilder trackingParameterBuilder) { }

        public void LogRevenue(string source, double amount, string currency)
        {
            AdjustAdRevenue adjustAdRevenue = new AdjustAdRevenue(source);
            adjustAdRevenue.SetRevenue(amount, currency);
            Adjust.TrackAdRevenue(adjustAdRevenue);
        }

        private void OnGetAdId(string adId)
        {
            this.OnGetAdIdEvent?.Invoke(adId);
        }
        
        private void OnDeferredDeeplinkTrigger(string value)
        {
            this.DeferredDeeplinkTrigger?.Invoke(value);
        }

        private void OnGetAdjustAttribute(AdjustAttribution attribution)
        {
            this.OnGetAdjustAttribution?.Invoke(attribution);
        }
        
        private void OnAttributionChanged(AdjustAttribution attribution)
        {
            this.OnAdjustAttributionChanged?.Invoke(attribution);
        }
        
        private void ReleaseUnmanagedResources()
        {
            this.OnGetAdIdEvent = null;
            this.DeferredDeeplinkTrigger = null;
            this.OnGetAdjustAttribution = null;
            this.OnAdjustAttributionChanged = null;
            this._adjustConfig.DeferredDeeplinkDelegate = null;
            this._adjustConfig.AttributionChangedDelegate = null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._isDisposed)
                return;
            
            if (disposing)
            {
                ReleaseUnmanagedResources();    
            }
            
            this._isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AdjustTracker()
        {
            Dispose(false);
        }
    }
}
