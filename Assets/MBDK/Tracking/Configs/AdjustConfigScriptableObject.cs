using System.Collections.Generic;
using AdjustSdk;
using UnityEngine;

namespace MBDK.Tracking.Configs
{
    [CreateAssetMenu(menuName = "Tracking/Configs/Adjust Config")]
    public class AdjustConfigScriptableObject : ScriptableObject
    {
        [SerializeField] public string appToken;
        [SerializeField] public AdjustEnvironment environment;
        [SerializeField] public AdjustLogLevel logLevel;
        [SerializeField] public string defaultTracker;
        [SerializeField] public string externalDeviceId;
        [SerializeField] public bool isCoppaComplianceEnabled;
        [SerializeField] public bool isSendingInBackgroundEnabled;
        [SerializeField] public bool isCostDataInAttributionEnabled;
        [SerializeField] public bool isDeviceIdsReadingOnceEnabled;
        [SerializeField] public bool isDeferredDeeplinkOpeningEnabled;
        [SerializeField] public bool isAppTrackingTransparencyUsageEnabled;
        [SerializeField] public bool isFirstSessionDelayEnabled;
        [SerializeField] public bool allowSuppressLogLevel;
        
        [Header("Deeplink Options")]
        [SerializeField] public bool shouldUseDeeplinkUrlStrategyDomain;
        [SerializeField] public List<string> urlStrategyDomains;
        [SerializeField] public bool shouldUseSubdomains;
        [SerializeField] public bool isDataResidency;
    }
}
