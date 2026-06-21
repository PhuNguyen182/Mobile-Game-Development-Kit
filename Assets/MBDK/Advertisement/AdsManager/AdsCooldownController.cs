using UnityEngine;

namespace MBDK.Advertisement.AdsManager
{
    public class AdsCooldownController
    {
        private float lastShowTime;
        private float cooldownDuration;

        public void SetCooldownDuration(float cooldown)
        {
            this.cooldownDuration = Mathf.Max(0, cooldown);
        }

        public bool CanShowAds()
        {
            if (this.lastShowTime <= 0)
                return true;

            bool canShowAds = Time.time - this.lastShowTime >= this.cooldownDuration;
            return canShowAds;
        }

        public void MarkAdsShown()
        {
            this.lastShowTime = Time.time;
        }

        public void ResetCooldownImmediately()
        {
            this.lastShowTime = 0;
        }
    }
}
