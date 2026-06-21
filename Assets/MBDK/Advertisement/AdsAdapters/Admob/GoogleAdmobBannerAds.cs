using GoogleMobileAds.Api;
using UnityEngine;

namespace MBDK.Advertisement.AdsAdapters.Admob
{
    public class GoogleAdmobBannerAds
    {
        private const string GoogleAdmobBannerLogTag = "GoogleAdmobBannerAds";
    
        private BannerView bannerView;
        private readonly string bannerUnitId;
    
        public GoogleAdmobBannerAds(string bannerUnitId)
        {
            this.bannerUnitId = bannerUnitId;
            this.DestroyBannerAds();
            this.CreateBannerAds();
        }

        public void ToggleBannerAds(bool shouldShowAds)
        {
            if (this.bannerView == null)
            {
                Debug.Log($"[{GoogleAdmobBannerLogTag}] Cannot toggle this banner ads: {this.bannerUnitId} - Banner view is null");
                return;    
            }
        
            if (shouldShowAds)
            {
                Debug.Log($"[{GoogleAdmobBannerLogTag}] Show banner ads: {this.bannerUnitId}");
                this.bannerView.Show();
            }
            else
            {
                Debug.Log($"[{GoogleAdmobBannerLogTag}] Hide banner ads: {this.bannerUnitId}");
                this.bannerView.Hide();
            }
        }

        private void CreateBannerAds()
        {
            this.bannerView = new BannerView(this.bannerUnitId, AdSize.Banner, AdPosition.Bottom);
        
            this.bannerView.OnAdPaid += OnAdPaid;
            this.bannerView.OnBannerAdLoaded += OnBannerAdLoaded;
            this.bannerView.OnBannerAdLoadFailed += OnBannerAdLoadFailed;
            this.bannerView.OnAdImpressionRecorded += OnAdImpressionRecorded;
            this.bannerView.OnAdFullScreenContentOpened += OnAdFullScreenContentOpened;
            this.bannerView.OnAdFullScreenContentClosed += OnAdFullScreenContentClosed;
            this.bannerView.OnAdClicked += OnAdClicked;
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
            Debug.Log($"[{GoogleAdmobBannerLogTag}] Banner ad paid! Currency code: {adValue.CurrencyCode} with value: {adValue.Value} and precision: {adValue.Precision}");
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
            Debug.Log($"[{GoogleAdmobBannerLogTag}] Load banner ads: {this.bannerUnitId}");
            this.bannerView.LoadAd(new AdRequest());
        }
    
        private void DestroyBannerAds()
        {
            if (this.bannerView == null)
            {
                Debug.Log($"[{GoogleAdmobBannerLogTag}] Cannot destroy this banner ads: {this.bannerUnitId} - Banner view is null");
                return;
            }

            Debug.Log($"[{GoogleAdmobBannerLogTag}] Destroy banner ads: {this.bannerUnitId}");
            this.bannerView.OnAdPaid -= OnAdPaid;
            this.bannerView.OnBannerAdLoaded -= OnBannerAdLoaded;
            this.bannerView.OnBannerAdLoadFailed -= OnBannerAdLoadFailed;
            this.bannerView.OnAdImpressionRecorded -= OnAdImpressionRecorded;
            this.bannerView.OnAdFullScreenContentOpened -= OnAdFullScreenContentOpened;
            this.bannerView.OnAdFullScreenContentClosed -= OnAdFullScreenContentClosed;
            this.bannerView.OnAdClicked -= OnAdClicked;
            
            this.bannerView.Destroy();
            this.bannerView = null;
        }
    }
}
