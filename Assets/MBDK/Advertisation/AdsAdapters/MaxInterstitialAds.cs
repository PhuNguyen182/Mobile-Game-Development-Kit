using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class MaxInterstitialAds
{
    private const string MaxInterstitialLogTag = "MaxInterstitialAds";

    private int _retryAttempt = 0;
    private readonly string _interstitialUnitId;

    public MaxInterstitialAds(string interstitialUnitId)
    {
        this._interstitialUnitId = interstitialUnitId;
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdReviewCreativeIdGeneratedEvent += OnAdReviewCreativeIdGeneratedEvent;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
        MaxSdkCallbacks.Interstitial.OnExpiredAdReloadedEvent += OnExpiredAdReloadedEvent;
        this.LoadInterstitial();
    }

    public void ShowInterstitialAds(string placement = null)
    {
        if (MaxSdkUnityEditor.IsInterstitialReady(this._interstitialUnitId))
        {
            Debug.Log($"[{MaxInterstitialLogTag}] Show interstitial ads: {this._interstitialUnitId} at placement: {placement}");
            MaxSdkUnityEditor.ShowInterstitial(this._interstitialUnitId);
        }
        else
        {
            Debug.LogError($"[{MaxInterstitialLogTag}] Interstitial ads not ready: {this._interstitialUnitId}");
        }
    }

    private async UniTask ShowInterstitialAsyncWithDelay(float delay)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay));
        this.LoadInterstitial();
    }

    private void LoadInterstitial()
    {
        Debug.Log($"[{MaxInterstitialLogTag}] Load interstitial ads: {this._interstitialUnitId}");
        MaxSdkUnityEditor.LoadInterstitial(this._interstitialUnitId);
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log(
            $"[{MaxInterstitialLogTag}] Interstitial ad loaded with id {adUnitId} at placement {adInfo.Placement}");
        this._retryAttempt = 0;
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        Debug.LogError(
            $"[{MaxInterstitialLogTag}] Interstitial ad load failed with id {adUnitId} with error code: {errorInfo.Code}, information: {errorInfo.Message}, retry count: [{this._retryAttempt}]");

        this._retryAttempt++;
        double retryDelay = Mathf.Pow(2, Mathf.Min(6, this._retryAttempt));

        Debug.Log($"[{MaxInterstitialLogTag}] Retrying in {retryDelay} seconds");
        this.ShowInterstitialAsyncWithDelay((float)retryDelay).Forget();
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log(
            $"[{MaxInterstitialLogTag}] Interstitial ad displayed with id {adUnitId} at placement {adInfo.Placement}");
    }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
        MaxSdkBase.AdInfo adInfo)
    {
        Debug.LogError(
            $"[{MaxInterstitialLogTag}] Interstitial ad failed to display with id {adUnitId} with error code: {errorInfo.Code}, information: {errorInfo.Message}");
        this.LoadInterstitial();
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log(
            $"[{MaxInterstitialLogTag}] Interstitial ad clicked with id {adUnitId} at placement {adInfo.Placement}");
    }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log(
            $"[{MaxInterstitialLogTag}] Interstitial ad hidden with id {adUnitId} at placement {adInfo.Placement}");
        this.LoadInterstitial();
    }

    private void OnExpiredAdReloadedEvent(string adUnitId, MaxSdkBase.AdInfo expiredAdInfo, MaxSdkBase.AdInfo adInfo)
    {
        Debug.LogError(
            $"[{MaxInterstitialLogTag}] Interstitial ad expired with id {adUnitId} at placement {expiredAdInfo.Placement}, reloading...");
    }

    private void OnAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log(
            $"[{MaxInterstitialLogTag}] Interstitial ad revenue paid with id {adUnitId} at placement {adInfo.Placement}. Revenue: {adInfo.RevenuePrecision}");
    }

    private void OnAdReviewCreativeIdGeneratedEvent(string adUnitId, string adReviewCreativeId,
        MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log(
            $"[{MaxInterstitialLogTag}] Interstitial Ad review ad with unit id {adUnitId} creative id generated: {adReviewCreativeId}");
    }
}
