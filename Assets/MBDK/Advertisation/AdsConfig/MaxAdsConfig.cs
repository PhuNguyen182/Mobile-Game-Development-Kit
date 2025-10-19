using UnityEngine;

[CreateAssetMenu(fileName = "MaxAdsConfig", menuName = "Advertising/Configs/MaxAdsConfig")]
public class MaxAdsConfig : ScriptableObject
{
    [SerializeField] public string maxSdkKey;
    
    [Header("Android")]
    [SerializeField] private string androidMaxBannerUnitId;
    [SerializeField] private string androidMaxInterstitialUnitId;
    [SerializeField] private string androidMaxRewardedUnitId;
    
    [Header("iOS")]
    [SerializeField] private string iOSMaxBannerUnitId;
    [SerializeField] private string iOSMaxInterstitialUnitId;
    [SerializeField] private string iOSMaxRewardedUnitId;

    public string GetBannerAdUnityId()
    {
#if UNITY_ANDROID
        return this.androidMaxBannerUnitId;
#elif UNITY_IOS
        return this.iOSMaxBannerUnitId;
#endif
        return null;
    }

    public string GetInterstitialAdUnitId()
    {
#if UNITY_ANDROID
        return this.androidMaxRewardedUnitId;
#elif UNITY_IOS
        return this.iOSMaxInterstitialUnitId;
#endif
        return null;
    
    }
    
    public string GetRewardedAdUnitId()
    {
#if UNITY_ANDROID
        return this.androidMaxInterstitialUnitId;
#elif UNITY_IOS
        return this.iOSMaxRewardedUnitId;
#endif
        return null;
    
    }
}
