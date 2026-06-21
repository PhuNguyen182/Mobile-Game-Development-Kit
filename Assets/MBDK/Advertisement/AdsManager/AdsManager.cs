using System;
using MBDK.Advertisement.AdsConfig;
using MBDK.Advertisement.AdsManager.AdsServices;
using MBDK.Advertisement.AdsConfig.Enums;

namespace MBDK.Advertisement.AdsManager
{
    public class AdsManager : IAdsManager
    {
        private readonly AdsServiceConfigData adsServiceConfigData;
        private readonly AdsCooldownController adsCooldownController;
        private readonly IAdsService maxAdsService;
        private readonly IAdsService googleAdsService;

        public AdsManager(AdsServiceConfigData adsServiceConfigData, MaxAdsConfig maxAdsConfig,
            GoogleAdmobAdsConfig googleAdmobAdsConfig)
        {
            this.adsServiceConfigData = adsServiceConfigData;
            this.adsCooldownController = new AdsCooldownController();
            this.adsCooldownController.SetCooldownDuration(this.adsServiceConfigData.interstitialAdsCooldown);
            this.maxAdsService = new MaxAdsService(maxAdsConfig);
            this.googleAdsService = new GoogleAdmobAdsService(googleAdmobAdsConfig);
        }

        public void ToggleBannerAds(bool shouldShowAds)
        {
            switch (adsServiceConfigData.bannerAdsServiceType)
            {
                case AdsServiceType.AppLovin:
                    this.maxAdsService.ToggleBannerAds(shouldShowAds);
                    break;
                case AdsServiceType.Admob:
                    this.googleAdsService.ToggleBannerAds(shouldShowAds);
                    break;
            }
        }

        public void ShowInterstitialAds(string placement = null)
        {
            if (!this.adsCooldownController.CanShowAds())
                return;
            
            switch (adsServiceConfigData.interstitialAdsServiceType)
            {
                case AdsServiceType.AppLovin:
                    this.maxAdsService.ShowInterstitialAds(placement);
                    break;
                case AdsServiceType.Admob:
                    this.googleAdsService.ShowInterstitialAds(placement);
                    break;
            }
            
            this.adsCooldownController.MarkAdsShown();
        }

        public void ShowRewardedAds(string placement = null, Action onReceivedRewardAfterAdShow = null)
        {
            switch (adsServiceConfigData.rewardedAdsServiceType)
            {
                case AdsServiceType.AppLovin:
                    this.maxAdsService.ShowRewardedAds(placement, onReceivedRewardAfterAdShow);
                    break;
                case AdsServiceType.Admob:
                    this.googleAdsService.ShowRewardedAds(placement, onReceivedRewardAfterAdShow);
                    break;
            }
        }
    }
}
