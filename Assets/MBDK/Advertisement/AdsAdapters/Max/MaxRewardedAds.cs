using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MBDK.Advertisement.AdsAdapters.Max
{
    public class MaxRewardedAds
    {
        private const string MaxRewardedLogTag = "MaxRewardedAds";

        private int _retryAttempt;
        private Action _onReceivedRewardAfterAdShow;
        private readonly string _rewardedUnitId;

        public MaxRewardedAds(string rewardedUnitId)
        {
            this._rewardedUnitId = rewardedUnitId;
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

        public void ShowRewardedAds(string placement = null, Action onReceivedRewardAfterAdShow = null)
        {
            if (MaxSdkUnityEditor.IsRewardedAdReady(_rewardedUnitId))
            {
                Debug.Log($"[{MaxRewardedLogTag}] Show rewarded ad: {this._rewardedUnitId} at placement: {placement}");
                MaxSdkUnityEditor.ShowRewardedAd(_rewardedUnitId);
                this._onReceivedRewardAfterAdShow = onReceivedRewardAfterAdShow;
            }
            else
            {
                Debug.LogError($"[{MaxRewardedLogTag}] Rewarded ad not ready: {this._rewardedUnitId}");
            }
        }

        private void LoadRewardedAd()
        {
            Debug.Log($"[{MaxRewardedLogTag}] Loading rewarded ad: {this._rewardedUnitId}");
            MaxSdkUnityEditor.LoadRewardedAd(_rewardedUnitId);
        }

        private async UniTask LoadRewardedAdAsyncWithDelay(float delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            this.LoadRewardedAd();
        }

        private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log($"[{MaxRewardedLogTag}] Rewarded ad loaded with id {adUnitId} at placement {adInfo.Placement}");
            this._retryAttempt = 0;
        }

        private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            Debug.LogError(
                $"[{MaxRewardedLogTag}] Rewarded ad load failed with id {adUnitId} with error code: {errorInfo.Code}, information: {errorInfo.Message}, retry count: [{this._retryAttempt}]");

            this._retryAttempt++;
            double retryDelay = Mathf.Pow(2, Mathf.Min(6, this._retryAttempt));

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
            Debug.LogError(
                $"[{MaxRewardedLogTag}] Rewarded ad failed to display with id {adUnitId} with error code: {errorInfo.Code}, information: {errorInfo.Message}");
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
            Debug.Log(
                $"[{MaxRewardedLogTag}] Rewarded ad received reward with id {adUnitId} at placement {adInfo.Placement}.\nReward: [label: {reward.Label}, amount: {reward.Amount}]");
            this._onReceivedRewardAfterAdShow?.Invoke();
            this._onReceivedRewardAfterAdShow = null;
        }

        private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log(
                $"[{MaxRewardedLogTag}] Rewarded ad revenue paid with id {adUnitId} at placement {adInfo.Placement}. Revenue: {adInfo.RevenuePrecision}");
        }

        private void OnExpiredAdReloadedEvent(string adUnityId, MaxSdkBase.AdInfo expiredAdInfo, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log(
                $"[{MaxRewardedLogTag}] Rewarded ad expired with id {adUnityId} at placement {expiredAdInfo.Placement}, reloading...");
        }

        private void OnAdReviewCreativeIdGeneratedEvent(string adUnitId, string adReviewCreativeId,
            MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log(
                $"[{MaxRewardedLogTag}] Ad review ad with unit id {adUnitId} creative id generated: {adReviewCreativeId}");
        }
    }
}
