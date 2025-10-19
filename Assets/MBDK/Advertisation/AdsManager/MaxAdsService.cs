using System;
using UnityEngine;

public class MaxAdsService
{
    private readonly MaxAdsConfig _maxAdsConfig;
    private MaxBannerAds _maxBannerAds;
    private MaxInterstitialAds _maxInterstitialAds;
    private MaxRewardedAds _maxRewardedAds;
    
    public bool IsAdServiceReady { get; private set; }

    public MaxAdsService(MaxAdsConfig maxAdsConfig)
    {
        IsAdServiceReady = false;
        this._maxAdsConfig = maxAdsConfig;
        MaxSdkCallbacks.OnSdkInitializedEvent += OnSdkInitializedEvent;
        MaxSdkUnityEditor.InitializeSdk();
    }

    private void OnSdkInitializedEvent(MaxSdkBase.SdkConfiguration sdkConfiguration)
    {
        this.InitializeBannerAds();
        this.InitializeInterstitialAds();
        this.InitializeRewardedAds();
        
        this.IsAdServiceReady = sdkConfiguration.IsSuccessfullyInitialized;
        if (this.IsAdServiceReady)
        {
            Debug.Log("Max SDK initialized successfully");
        }
        else
        {
            Debug.LogError("Max SDK failed to initialize");
        }
    }

    private void InitializeBannerAds()
    {
        string bannerAdUnitId = this._maxAdsConfig.GetBannerAdUnityId();
        this._maxBannerAds = new MaxBannerAds(bannerAdUnitId);
    }

    private void InitializeInterstitialAds()
    {
        string interstitialAdUnitId = this._maxAdsConfig.GetInterstitialAdUnitId();
        this._maxInterstitialAds = new MaxInterstitialAds(interstitialAdUnitId);
    }
    
    private void InitializeRewardedAds()
    {
        string rewardedAdUnitId = this._maxAdsConfig.GetRewardedAdUnitId();
        this._maxRewardedAds = new MaxRewardedAds(rewardedAdUnitId);
    }

    public void ToggleBannerAds(bool shouldShowAds)
    {
        this._maxBannerAds.ToggleBannerAds(shouldShowAds);
    }

    public void ShowInterstitialAds(string placement = null)
    {
        this._maxInterstitialAds.ShowInterstitialAds();
    }

    public void ShowRewardedAds(string placement = null, Action onReceivedRewardAfterAdShow = null)
    {
        this._maxRewardedAds.ShowRewardedAds(placement, onReceivedRewardAfterAdShow);
    }
}
