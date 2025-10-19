using System;

namespace MBDK.Advertisement.AdsManager
{
    public interface IAdsManager
    {
        public void ToggleBannerAds(bool shouldShowAds);
        public void ShowInterstitialAds(string placement = null);
        public void ShowRewardedAds(string placement = null, Action onReceivedRewardAfterAdShow = null);
    }
}
