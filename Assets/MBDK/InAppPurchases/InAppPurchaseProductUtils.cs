using UnityEngine.Purchasing;

namespace MBDK.InAppPurchases
{
    public class InAppPurchaseProductUtils
    {
        private readonly StoreController storeController;
        private readonly InAppPurchaseProductConfigData productConfigData;
    
        public InAppPurchaseProductUtils(StoreController storeController, InAppPurchaseProductConfigData productConfigData)
        {
            this.storeController = storeController;
            this.productConfigData = productConfigData;
        }

        public Product GetProduct(string productId)
        {
            return this.storeController.GetProductById(productId);
        }

        public string GetProductPrice(string productId)
        {
            var product = GetProduct(productId);
            if (product != null) return product.metadata.localizedPriceString;
            var productConfig = productConfigData.GetProductConfig(productId);
            return productConfig.defaultPrice;

        }
    }
}
