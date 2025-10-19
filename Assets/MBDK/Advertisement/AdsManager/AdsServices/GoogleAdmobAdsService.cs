using System;
using GoogleMobileAds.Api;
using MBDK.Advertisement.AdsAdapters.Admob;
using MBDK.Advertisement.AdsConfig;

namespace MBDK.Advertisement.AdsManager.AdsServices
{
    public class GoogleAdmobAdsService : IAdsService
    {
        private readonly GoogleAdmobAdsConfig _googleAdmobAdsConfig;
        private GoogleAdmobBannerAds _googleAdmobBannerAds;
    
        public bool IsAdServiceReady { get; private set; }

        public GoogleAdmobAdsService(GoogleAdmobAdsConfig googleAdmobAdsConfig)
        {
            IsAdServiceReady = false;
            this._googleAdmobAdsConfig = googleAdmobAdsConfig;
            MobileAds.Initialize(MobilAdsInitializeCallback);
        }
    
        private void MobilAdsInitializeCallback(InitializationStatus initStatus)
        {
            this.InitializeBannerAds();
            this.IsAdServiceReady = true;
        }

        private void InitializeBannerAds()
        {
            string bannerAdUnitId = this._googleAdmobAdsConfig.GetGoogleAdmobBannerAdUnitId();
            this._googleAdmobBannerAds = new GoogleAdmobBannerAds(bannerAdUnitId);
        }
    
        public void ToggleBannerAds(bool shouldShowAds)
        {
            this._googleAdmobBannerAds.ToggleBannerAds(shouldShowAds);
        }

        public void ShowInterstitialAds(string placement = null) { }

        public void ShowRewardedAds(string placement = null, Action onReceivedRewardAfterAdShow = null) { }
    }
}
