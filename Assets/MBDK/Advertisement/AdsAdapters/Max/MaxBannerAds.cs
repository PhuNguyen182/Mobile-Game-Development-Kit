using UnityEngine;

namespace MBDK.Advertisement.AdsAdapters.Max
{
    public class MaxBannerAds
    {
        private const string MaxBannerLogTag = "MaxBannerAds";

        private readonly string _bannerUnitId;

        public MaxBannerAds(string bannerUnitId)
        {
            this._bannerUnitId = bannerUnitId;
            MaxSdkBase.AdViewConfiguration adViewConfiguration =
                new MaxSdkBase.AdViewConfiguration(MaxSdkBase.AdViewPosition.BottomCenter);
            MaxSdkUnityEditor.CreateBanner(bannerUnitId, adViewConfiguration);

            MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
            MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
            MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
            MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;
            MaxSdkCallbacks.Banner.OnAdReviewCreativeIdGeneratedEvent += OnAdReviewCreativeIdGeneratedEvent;
        }

        public void ToggleBannerAds(bool shouldShowAds)
        {
            if (shouldShowAds)
            {
                Debug.Log($"[{MaxBannerLogTag}] Show banner ads: {this._bannerUnitId}");
                MaxSdkUnityEditor.ShowBanner(this._bannerUnitId);
            }
            else
            {
                Debug.Log($"[{MaxBannerLogTag}] Hide banner ads: {this._bannerUnitId}");
                MaxSdkUnityEditor.HideBanner(this._bannerUnitId);
            }
        }

        public void DestroyBannerAds()
        {
            Debug.Log($"[{MaxBannerLogTag}] Destroy banner ads: {this._bannerUnitId}");
            MaxSdkUnityEditor.DestroyBanner(this._bannerUnitId);
        }

        private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log($"[{MaxBannerLogTag}] Banner ad loaded with id {adUnitId} at placement {adInfo.Placement}");
        }

        private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            Debug.Log(
                $"[{MaxBannerLogTag}] Banner ad load failed with id {adUnitId} with error code: {errorInfo.Code}, information: {errorInfo.Message}");
        }

        private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log($"[{MaxBannerLogTag}] Banner ad clicked with id {adUnitId} at placement {adInfo.Placement}");
        }

        private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log(
                $"[{MaxBannerLogTag}] Banner ad revenue paid with id {adUnitId} at placement {adInfo.Placement}. Revenue: {adInfo.RevenuePrecision}");
        }

        private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log($"[{MaxBannerLogTag}] Banner ad expanded with id {adUnitId} at placement {adInfo.Placement}");
        }

        private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log($"[{MaxBannerLogTag}] Banner ad collapsed with id {adUnitId} at placement {adInfo.Placement}");
        }

        private void OnAdReviewCreativeIdGeneratedEvent(string adUnitId, string adReviewCreativeId,
            MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log(
                $"[{MaxBannerLogTag}] Ad review ad with unit id {adUnitId} creative id generated: {adReviewCreativeId}");
        }
    }
}
