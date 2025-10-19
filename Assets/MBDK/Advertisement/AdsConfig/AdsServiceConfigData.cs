using MBDK.Advertisement.AdsConfig.Enums;
using UnityEngine;

namespace MBDK.Advertisement.AdsConfig
{
    [CreateAssetMenu(fileName = "AdsServiceConfigData", menuName = "Scriptable Objects/AdsServiceConfigData")]
    public class AdsServiceConfigData : ScriptableObject
    {
        [SerializeField] public float interstitialAdsCooldown = 45f;
        [SerializeField] public AdsServiceType bannerAdsServiceType;
        [SerializeField] public AdsServiceType interstitialAdsServiceType;
        [SerializeField] public AdsServiceType rewardedAdsServiceType;
    }
}
