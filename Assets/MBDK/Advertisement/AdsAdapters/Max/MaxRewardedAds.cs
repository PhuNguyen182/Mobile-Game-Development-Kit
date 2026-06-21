using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MBDK.Advertisement.AdsAdapters.Max
{
    public class MaxRewardedAds : IDisposable
    {
        private const string MaxRewardedLogTag = "MaxRewardedAds";

        private int retryAttempt;
        private Action onReceivedRewardAfterAdShow;
        private readonly string rewardedUnitId;

        public MaxRewardedAds(string rewardedUnitId)
        {
            this.rewardedUnitId = rewardedUnitId;
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
            MaxSdkCallbacks.Rewarded.OnAdReviewCreativeIdGeneratedEvent += OnAdReviewCreativeIdGeneratedEvent;
            MaxSdkCallbacks.Rewarded.OnExpiredAdReloadedEvent += OnExpiredAdReloadedEvent;
            this.LoadRewardedAd();
        }

        public void ShowRewardedAds(string placement = null, Action onReceivedRewardAfterAdShowCallback = null)
        {
            if (MaxSdk.IsRewardedAdReady(rewardedUnitId))
            {
                Debug.Log($"[{MaxRewardedLogTag}] Show rewarded ad: {this.rewardedUnitId} at placement: {placement}");
                MaxSdk.ShowRewardedAd(rewardedUnitId);
                this.onReceivedRewardAfterAdShow = onReceivedRewardAfterAdShowCallback;
            }
            else
            {
                Debug.LogError($"[{MaxRewardedLogTag}] Rewarded ad not ready: {this.rewardedUnitId}");
            }
        }

        private void LoadRewardedAd()
        {
            Debug.Log($"[{MaxRewardedLogTag}] Loading rewarded ad: {this.rewardedUnitId}");
            MaxSdk.LoadRewardedAd(rewardedUnitId);
        }

        private async UniTask LoadRewardedAdAsyncWithDelay(float delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            this.LoadRewardedAd();
        }

        private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log($"[{MaxRewardedLogTag}] Rewarded ad loaded with id {adUnitId} at placement {adInfo.Placement}");
            this.retryAttempt = 0;
        }

        private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            Debug.LogError($"[{MaxRewardedLogTag}] Rewarded ad load failed with id {adUnitId} with error code: {errorInfo.Code}, information: {errorInfo.Message}, retry count: [{this.retryAttempt}]");

            this.retryAttempt++;
            double retryDelay = Mathf.Pow(2, Mathf.Min(6, this.retryAttempt));

            Debug.Log($"[{MaxRewardedLogTag}] Retrying in {retryDelay} seconds]");
            this.LoadRewardedAdAsyncWithDelay((float)retryDelay).Forget();
        }

        private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log($"[{MaxRewardedLogTag}] Rewarded ad displayed with id {adUnitId} at placement {adInfo.Placement}");
        }

        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
            MaxSdkBase.AdInfo adInfo)
        {
            Debug.LogError($"[{MaxRewardedLogTag}] Rewarded ad failed to display with id {adUnitId} with error code: {errorInfo.Code}, information: {errorInfo.Message}");
            this.LoadRewardedAd();
        }

        private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log($"[{MaxRewardedLogTag}] Rewarded ad clicked with id {adUnitId} at placement {adInfo.Placement}");
        }

        private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log($"[{MaxRewardedLogTag}] Rewarded ad hidden with id {adUnitId} at placement {adInfo.Placement}");
            this.LoadRewardedAd();
        }

        private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log($"[{MaxRewardedLogTag}] Rewarded ad received reward with id {adUnitId} at placement {adInfo.Placement}.\nReward: [label: {reward.Label}, amount: {reward.Amount}]");
            this.onReceivedRewardAfterAdShow?.Invoke();
            this.onReceivedRewardAfterAdShow = null;
        }

        private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log($"[{MaxRewardedLogTag}] Rewarded ad revenue paid with id {adUnitId} at placement {adInfo.Placement}. Revenue: {adInfo.RevenuePrecision}");
        }

        private void OnExpiredAdReloadedEvent(string adUnityId, MaxSdkBase.AdInfo expiredAdInfo, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log($"[{MaxRewardedLogTag}] Rewarded ad expired with id {adUnityId} at placement {expiredAdInfo.Placement}, reloading...");
        }

        private void OnAdReviewCreativeIdGeneratedEvent(string adUnitId, string adReviewCreativeId,
            MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log($"[{MaxRewardedLogTag}] Ad review ad with unit id {adUnitId} creative id generated: {adReviewCreativeId}");
        }

        public void Dispose()
        {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent -= OnRewardedAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent -= OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent -= OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent -= OnRewardedAdRevenuePaidEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent -= OnRewardedAdHiddenEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent -= OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= OnRewardedAdReceivedRewardEvent;
            MaxSdkCallbacks.Rewarded.OnAdReviewCreativeIdGeneratedEvent -= OnAdReviewCreativeIdGeneratedEvent;
            MaxSdkCallbacks.Rewarded.OnExpiredAdReloadedEvent -= OnExpiredAdReloadedEvent;
        }
    }
}
