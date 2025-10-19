using MBDK.Advertisement.AdsConfig.Enums;
using UnityEngine;

namespace MBDK.Advertisement.AdsConfig
{
    [CreateAssetMenu(fileName = "GoogleAdmobAdsConfig", menuName = "Advertising/Configs/GoogleAdmobAdsConfig")]
    public class GoogleAdmobAdsConfig : ScriptableObject
    {
        [SerializeField] private AdmobAdsType adsType;
    
        [Header("Test Android Id")]
        [SerializeField] private string testAndroidBannerAdUnitId;
        [SerializeField] private string testAndroidInterstitialAdUnitId;
        [SerializeField] private string testAndroidRewardedAdUnitId;
    
        [Header("Test iOS Id")]
        [SerializeField] private string testIOSBannerAdUnitId;
        [SerializeField] private string testIOSInterstitialAdUnitId;
        [SerializeField] private string testIOSRewardedAdUnitId;
    
        [Header("Android")]
        [SerializeField] private string androidAppId;
        [SerializeField] private string androidBannerAdUnitId;
        [SerializeField] private string androidInterstitialAdUnitId;
        [SerializeField] private string androidRewardedAdUnitId;
    
        [Header("iOS")]
        [SerializeField] private string iOSAppId;
        [SerializeField] private string iOSBannerAdUnitId;
        [SerializeField] private string iOSInterstitialAdUnitId;
        [SerializeField] private string iOSRewardedAdUnitId;

        public string GetGoogleAdmobAppId()
        {
#if UNITY_ANDROID
        return this.androidAppId;
#elif UNITY_IOS
        return this.iOSAppId;
#endif
            return null;
        }

        public string GetGoogleAdmobBannerAdUnitId()
        {
            return this.adsType == AdmobAdsType.Test
                ? this.GetTestGoogleAdmobBannerAdUnitId()
                : this.GetRealGoogleAdmobBannerAdUnitId();
        }

        public string GetGoogleAdmobInterstitialAdUnitId()
        {
            return this.adsType == AdmobAdsType.Test
                ? this.GetTestGoogleAdmobInterstitialAdUnitId()
                : this.GetRealGoogleAdmobInterstitialAdUnitId();
        }

        public string GetGoogleAdmobRewardedAdUnitId()
        {
            return this.adsType == AdmobAdsType.Test
                ? this.GetTestGoogleAdmobRewardedAdUnitId()
                : this.GetRealGoogleAdmobRewardedAdUnitId();
        }

        private string GetTestGoogleAdmobBannerAdUnitId()
        {
#if UNITY_ANDROID
        return this.testAndroidBannerAdUnitId;
#elif UNITY_IOS
        return this.testIOSBannerAdUnitId;
#endif
            return null;
        }
    
        private string GetRealGoogleAdmobBannerAdUnitId()
        {
#if UNITY_ANDROID
        return this.androidBannerAdUnitId;
#elif UNITY_IOS
        return this.iOSBannerAdUnitId;
#endif
            return null;
        }

        private string GetTestGoogleAdmobInterstitialAdUnitId()
        {
#if UNITY_ANDROID
        return this.testAndroidInterstitialAdUnitId;
#elif UNITY_IOS
        return this.testIOSInterstitialAdUnitId;
#endif
            return null;
        }
    
        private string GetRealGoogleAdmobInterstitialAdUnitId()
        {
#if UNITY_ANDROID
        return this.androidInterstitialAdUnitId;
#elif UNITY_IOS
        return this.iOSInterstitialAdUnitId;
#endif
            return null;
        }

        private string GetTestGoogleAdmobRewardedAdUnitId()
        {
#if UNITY_ANDROID
        return this.testAndroidRewardedAdUnitId;
#elif UNITY_IOS
        return this.testIOSRewardedAdUnitId;
#endif
            return null;
        }
    
        private string GetRealGoogleAdmobRewardedAdUnitId()
        {
#if UNITY_ANDROID
        return this.androidRewardedAdUnitId;
#elif UNITY_IOS
        return this.iOSRewardedAdUnitId;
#endif
            return null;
        }
    }

}