using System;
using MBDK.Advertisement.AdsConfig;
using MBDK.Advertisement.AdsManager.AdsServices;
using MBDK.Advertisement.AdsConfig.Enums;

namespace MBDK.Advertisement.AdsManager
{
    public class AdsManager : IAdsManager
    {
        private readonly AdsServiceConfigData _adsServiceConfigData;
        private readonly AdsCooldownController _adsCooldownController;
        private readonly IAdsService _maxAdsService;
        private readonly IAdsService _googleAdsService;

        public AdsManager(AdsServiceConfigData adsServiceConfigData, MaxAdsConfig maxAdsConfig,
            GoogleAdmobAdsConfig googleAdmobAdsConfig)
        {
            this._adsServiceConfigData = adsServiceConfigData;
            this._adsCooldownController = new AdsCooldownController();
            this._adsCooldownController.SetCooldownDuration(this._adsServiceConfigData.interstitialAdsCooldown);
            this._maxAdsService = new MaxAdsService(maxAdsConfig);
            this._googleAdsService = new GoogleAdmobAdsService(googleAdmobAdsConfig);
        }

        public void ToggleBannerAds(bool shouldShowAds)
        {
            switch (_adsServiceConfigData.bannerAdsServiceType)
            {
                case AdsServiceType.Max:
                    this._maxAdsService.ToggleBannerAds(shouldShowAds);
                    break;
                case AdsServiceType.Google:
                    this._googleAdsService.ToggleBannerAds(shouldShowAds);
                    break;
            }
        }

        public void ShowInterstitialAds(string placement = null)
        {
            if (!this._adsCooldownController.CanShowAds())
                return;
            
            switch (_adsServiceConfigData.bannerAdsServiceType)
            {
                case AdsServiceType.Max:
                    this._maxAdsService.ShowInterstitialAds(placement);
                    break;
                case AdsServiceType.Google:
                    this._googleAdsService.ShowInterstitialAds(placement);
                    break;
            }
            
            this._adsCooldownController.MarkAdsShown();
        }

        public void ShowRewardedAds(string placement = null, Action onReceivedRewardAfterAdShow = null)
        {
            switch (_adsServiceConfigData.bannerAdsServiceType)
            {
                case AdsServiceType.Max:
                    this._maxAdsService.ShowRewardedAds(placement, onReceivedRewardAfterAdShow);
                    break;
                case AdsServiceType.Google:
                    this._googleAdsService.ShowRewardedAds(placement, onReceivedRewardAfterAdShow);
                    break;
            }
        }
    }
}
