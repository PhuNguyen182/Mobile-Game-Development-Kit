using System;

namespace MBDK.Advertisement.AdsManager.AdsServices
{
    public interface IAdsService
    {
        public bool IsAdServiceReady { get; }
        public void ToggleBannerAds(bool shouldShowAds);
        public void ShowInterstitialAds(string placement = null);
        public void ShowRewardedAds(string placement = null, Action onReceivedRewardAfterAdShow = null);
    }
}
