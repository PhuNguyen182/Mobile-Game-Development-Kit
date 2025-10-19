using System;
using UnityEngine;
using UnityEngine.Purchasing;

namespace MBDK.InAppPurchases
{
    [CreateAssetMenu(fileName = "InAppPurchaseProductConfigData", menuName = "InAppPurchase/ConfigData/InAppPurchaseProductConfigData")]
    public class InAppPurchaseProductConfigData : ScriptableObject
    {
        [SerializeField] public InAppPurchaseProductConfig[] productConfigs;
    
        public InAppPurchaseProductConfig GetProductConfig(string productId)
        {
            foreach (var productConfig in productConfigs)
            {
                if (string.CompareOrdinal(productConfig.GetProductID(), productId) == 0)
                {
                    return productConfig;
                }
            }
        
            return default;
        }
    }

    [Serializable]
    public struct InAppPurchaseProductConfig
    {
        public string appleProductId;
        public string googleProductId;
        public ProductType productType;
        public string defaultPrice;

        public string GetProductID()
        {
#if UNITY_IOS
        return appleProductId;
#elif UNITY_ANDROID
        return googleProductId;
#endif
            return null;
        }
    }
}