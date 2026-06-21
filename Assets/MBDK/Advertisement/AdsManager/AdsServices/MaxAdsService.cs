using System;
using MBDK.Advertisement.AdsAdapters.Max;
using MBDK.Advertisement.AdsConfig;
using UnityEngine;

namespace MBDK.Advertisement.AdsManager.AdsServices
{
    public class MaxAdsService : IAdsService, IDisposable
    {
        private const string MaxLogTag = "MaxAdsService";
        
        private readonly MaxAdsConfig maxAdsConfig;
        private MaxBannerAds maxBannerAds;
        private MaxInterstitialAds maxInterstitialAds;
        private MaxRewardedAds maxRewardedAds;
    
        public bool IsAdServiceReady { get; private set; }

        public MaxAdsService(MaxAdsConfig maxAdsConfig)
        {
            IsAdServiceReady = false;
            this.maxAdsConfig = maxAdsConfig;
            MaxSdkCallbacks.OnSdkInitializedEvent += OnSdkInitializedEvent;
            MaxSdk.InitializeSdk();
        }

        private void OnSdkInitializedEvent(MaxSdkBase.SdkConfiguration sdkConfiguration)
        {
            this.InitializeBannerAds();
            this.InitializeInterstitialAds();
            this.InitializeRewardedAds();
        
            this.IsAdServiceReady = sdkConfiguration.IsSuccessfullyInitialized;
            if (this.IsAdServiceReady)
            {
                Debug.Log($"[{MaxLogTag}] Max SDK initialized successfully");
            }
            else
            {
                Debug.LogError($"[{MaxLogTag}] Max SDK failed to initialize");
            }
        }

        private void InitializeBannerAds()
        {
            string bannerAdUnitId = this.maxAdsConfig.GetBannerAdUnityId();
            this.maxBannerAds = new MaxBannerAds(bannerAdUnitId);
            Debug.Log($"[{MaxLogTag}] Max banner ads initialized successfully: {bannerAdUnitId}");
        }

        private void InitializeInterstitialAds()
        {
            string interstitialAdUnitId = this.maxAdsConfig.GetInterstitialAdUnitId();
            this.maxInterstitialAds = new MaxInterstitialAds(interstitialAdUnitId);
            Debug.Log($"[{MaxLogTag}] Max interstitial ads initialized successfully: {interstitialAdUnitId}");
        }
    
        private void InitializeRewardedAds()
        {
            string rewardedAdUnitId = this.maxAdsConfig.GetRewardedAdUnitId();
            this.maxRewardedAds = new MaxRewardedAds(rewardedAdUnitId);
            Debug.Log($"[{MaxLogTag}] Max rewarded ads initialized successfully: {rewardedAdUnitId}");
        }

        public void ToggleBannerAds(bool shouldShowAds)
        {
            this.maxBannerAds.ToggleBannerAds(shouldShowAds);
        }

        public void ShowInterstitialAds(string placement = null)
        {
            this.maxInterstitialAds.ShowInterstitialAds();
        }

        public void ShowRewardedAds(string placement = null, Action onReceivedRewardAfterAdShow = null)
        {
            this.maxRewardedAds.ShowRewardedAds(placement, onReceivedRewardAfterAdShow);
        }

        public void Dispose()
        {
            MaxSdkCallbacks.OnSdkInitializedEvent -= OnSdkInitializedEvent;
        }
    }
}
