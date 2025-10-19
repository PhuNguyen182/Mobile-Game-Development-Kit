using UnityEngine.Purchasing;

namespace MBDK.InAppPurchases
{
    public class InAppPurchaseProductUtils
    {
        private readonly StoreController _storeController;
        private readonly InAppPurchaseProductConfigData _productConfigData;
    
        public InAppPurchaseProductUtils(StoreController storeController, InAppPurchaseProductConfigData productConfigData)
        {
            this._storeController = storeController;
            this._productConfigData = productConfigData;
        }

        public Product GetProduct(string productId)
        {
            return this._storeController.GetProductById(productId);
        }

        public string GetProductPrice(string productId)
        {
            var product = GetProduct(productId);
            if (product == null)
            {
                var productConfig = _productConfigData.GetProductConfig(productId);
                return productConfig.defaultPrice;
            }
        
            return product.metadata.localizedPriceString;
        }
    }
}
