using System;

namespace MBDK.Advertisation
{
    public interface IAdsManager
    {
        public void ToggleBannerAds(bool shouldShowAds);
        public void ShowInterstitialAds(string placement);
        public void ShowRewardedAds(string placement, Action onRewarded = null);
    }
}
