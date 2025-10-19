using GoogleMobileAds.Api;
using UnityEngine;

namespace MBDK.Advertisement.AdsAdapters.Admob
{
    public class GoogleAdmobBannerAds
    {
        private const string GoogleAdmobBannerLogTag = "GoogleAdmobBannerAds";
    
        private BannerView _bannerView;
        private readonly string _bannerUnitId;
    
        public GoogleAdmobBannerAds(string bannerUnitId)
        {
            this._bannerUnitId = bannerUnitId;
            this.DestroyBannerAds();
            this.CreateBannerAds();
        }

        public void ToggleBannerAds(bool shouldShowAds)
        {
            if (this._bannerView == null)
            {
                Debug.Log($"[{GoogleAdmobBannerLogTag}] Cannot toggle this banner ads: {this._bannerUnitId} - Banner view is null");
                return;    
            }
        
            if (shouldShowAds)
            {
                Debug.Log($"[{GoogleAdmobBannerLogTag}] Show banner ads: {this._bannerUnitId}");
                this._bannerView.Show();
            }
            else
            {
                Debug.Log($"[{GoogleAdmobBannerLogTag}] Hide banner ads: {this._bannerUnitId}");
                this._bannerView.Hide();
            }
        }

        private void CreateBannerAds()
        {
            this._bannerView = new BannerView(this._bannerUnitId, AdSize.Banner, AdPosition.Bottom);
        
            this._bannerView.OnAdPaid += OnAdPaid;
            this._bannerView.OnBannerAdLoaded += OnBannerAdLoaded;
            this._bannerView.OnBannerAdLoadFailed += OnBannerAdLoadFailed;
            this._bannerView.OnAdImpressionRecorded += OnAdImpressionRecorded;
            this._bannerView.OnAdFullScreenContentOpened += OnAdFullScreenContentOpened;
            this._bannerView.OnAdFullScreenContentClosed += OnAdFullScreenContentClosed;
            this._bannerView.OnAdClicked += OnAdClicked;
            this.LoadBannerAds();
        }

        private void OnAdFullScreenContentClosed()
        {
            Debug.Log($"[{GoogleAdmobBannerLogTag}] Ad full screen content closed");
        }

        private void OnAdFullScreenContentOpened()
        {
            Debug.Log($"[{GoogleAdmobBannerLogTag}] Ad full screen content opened");
        }

        private void OnAdClicked()
        {
            Debug.Log($"[{GoogleAdmobBannerLogTag}] Ad clicked");
        }

        private void OnAdImpressionRecorded()
        {
            Debug.Log($"[{GoogleAdmobBannerLogTag}] Ad impression recorded");
        }

        private void OnAdPaid(AdValue adValue)
        {
            Debug.Log(
                $"[{GoogleAdmobBannerLogTag}] Banner ad paid! Currency code: {adValue.CurrencyCode} with value: {adValue.Value} and precision: {adValue.Precision}");
        }

        private void OnBannerAdLoadFailed(LoadAdError adError)
        {
            Debug.LogError($"[{GoogleAdmobBannerLogTag}] Banner ad load failed with error because: {adError.GetMessage()}, Error code: {adError.GetCode()}");
        }

        private void OnBannerAdLoaded()
        {
            Debug.Log($"[{GoogleAdmobBannerLogTag}] Banner ad loaded");
        }

        private void LoadBannerAds()
        {
            Debug.Log($"[{GoogleAdmobBannerLogTag}] Load banner ads: {this._bannerUnitId}");
            this._bannerView.LoadAd(new AdRequest());
        }
    
        private void DestroyBannerAds()
        {
            if (this._bannerView == null)
            {
                Debug.Log($"[{GoogleAdmobBannerLogTag}] Cannot destroy this banner ads: {this._bannerUnitId} - Banner view is null");
                return;
            }

            Debug.Log($"[{GoogleAdmobBannerLogTag}] Destroy banner ads: {this._bannerUnitId}");
            this._bannerView.Destroy();
            this._bannerView = null;
        }
    }
}
