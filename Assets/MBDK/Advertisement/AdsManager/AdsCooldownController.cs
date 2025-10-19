using UnityEngine;

namespace MBDK.Advertisement.AdsManager
{
    public class AdsCooldownController
    {
        private float _lastShowTime;
        private float _cooldownDuration;

        public void SetCooldownDuration(float cooldownDuration)
        {
            this._cooldownDuration = Mathf.Max(0, cooldownDuration);
        }

        public bool CanShowAds()
        {
            if (this._lastShowTime <= 0)
                return true;

            bool canShowAds = Time.time - this._lastShowTime >= this._cooldownDuration;
            return canShowAds;
        }

        public void MarkAdsShown()
        {
            this._lastShowTime = Time.time;
        }

        public void ResetCooldownImmediately()
        {
            this._lastShowTime = 0;
        }
    }
}
