using System;
using System.Collections.Generic;
using UnityEngine.Purchasing;
using Cysharp.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

namespace MBDK.InAppPurchases
{
    public class InAppPurchaseManager : IDisposable
    {
        private const string Tag = "InAppPurchaseManager";
        private const string DevelopmentEnvironment = "development";
        private const string ProductionEnvironment = "production";

        private readonly InAppPurchaseProductConfigData productConfigData;
        private StoreController storeController;
        private Action onProductBuySuccess;
        private Action onProductBuyFailed;

        public event Action<bool> OnRestoreEvent;

        public InAppPurchaseManager(InAppPurchaseProductConfigData productConfigData)
        {
            this.productConfigData = productConfigData;
            Initialize().Forget();
        }

        private async UniTask Initialize()
        {
            await InitializeGamingService(OnUnityServicesInitializeSuccess, OnUnityServicesInitializeFailure);
            await InitializeInAppPurchase();
        }

        private async UniTask InitializeGamingService(Action onSuccess, Action<string> onError)
        {
            try
            {
                Debug.Log($"[{Tag}] Trying to initialize Unity Gaming Services with environment: {DevelopmentEnvironment}");
                InitializationOptions option = new InitializationOptions().SetEnvironmentName(DevelopmentEnvironment);
                await UnityServices.InitializeAsync(option).ContinueWith(_ => onSuccess?.Invoke());
            }
            catch (Exception exception)
            {
                onError?.Invoke(exception.Message);
            }
        }

        private async UniTask InitializeInAppPurchase()
        {
            if (UnityServices.State == ServicesInitializationState.Uninitialized)
            {
                string text = $"[{Tag}] Error: Unity Gaming Services not initialized.";
                Debug.LogError(text);
            }

            storeController = UnityIAPServices.StoreController();
            storeController.OnPurchasePending += OnPurchasePending;
            storeController.OnPurchaseDeferred += OnPurchaseDeferred;
            storeController.OnPurchaseConfirmed += OnPurchaseConfirmed;
            storeController.OnPurchaseFailed += OnPurchaseFailed;
            storeController.OnStoreDisconnected += OnStoreDisconnected;
            storeController.OnCheckEntitlement += OnCheckEntitlement;
            await storeController.Connect();
            this.FetchProducts();
        }

        private void OnUnityServicesInitializeSuccess()
        {
            Debug.Log($"[{Tag}] Unity Gaming Services with environment {DevelopmentEnvironment} initialized successfully.]");
        }

        private void OnUnityServicesInitializeFailure(string errorMessage)
        {
            Debug.LogError($"[{Tag}] Unity Gaming Services with environment {DevelopmentEnvironment} initialized failed. Error message: {errorMessage}.]");
        }

        private void FetchProducts()
        {
            List<ProductDefinition> productsToFetch = new List<ProductDefinition>();
            for (int i = 0; i < this.productConfigData.productConfigs.Length; i++)
            {
                string productId = this.productConfigData.productConfigs[i].GetProductID();
                ProductType productType = this.productConfigData.productConfigs[i].productType;
                ProductDefinition productDefinition = new(productId, productType);
                productsToFetch.Add(productDefinition);
            }

            storeController.OnProductsFetched += OnProductsFetched;
            storeController.OnProductsFetchFailed += OnProductsFetchFailed;
            storeController.FetchProducts(productsToFetch);
        }

        private void OnProductsFetchFailed(ProductFetchFailed productFetchFailed)
        {
            Debug.LogError($"[{Tag}] All products fetched failed. More information: {productFetchFailed.FailureReason}");
            foreach (var productDefinition in productFetchFailed.FailedFetchProducts)
            {
                Debug.LogError($"[{Tag}] Fetch Error: Product ID: {productDefinition.id}, Product Type: {productDefinition.type}");
            }
        }

        private void OnProductsFetched(List<Product> products)
        {
            Debug.Log($"[{Tag}] All products fetched successfully.");
            foreach (var product in products)
            {
                Debug.Log($"[{Tag}] Product ID: {product.definition.id}, Product Type: {product.definition.type}, Price: {product.metadata.localizedPriceString}");
            }
        }

        private void OnPurchaseDeferred(DeferredOrder order)
        {
            Debug.Log($"[{Tag}] Your purchase is deferred. More information: {order.Info.TransactionID}");
        }

        private void OnCheckEntitlement(Entitlement entitlement)
        {
            Debug.Log($"[{Tag}] Checking entitlement... More information:\nStatus: {entitlement.Status}, Product ID: {entitlement.Product?.definition.id}, Error message: {entitlement.ErrorMessage}");
        }

        private void OnStoreDisconnected(StoreConnectionFailureDescription description)
        {
            Debug.LogError($"[{Tag}] Store disconnected. More information: {description.Message}. Can it retryable: {description.IsRetryable}.");
            this.ExecutePurchaseFailedCallback();
        }

        private void OnPurchaseFailed(FailedOrder order)
        {
            Debug.LogError($"[{Tag}] Your product purchase failed. More information: {order.Details}\nReason failed: {order.FailureReason}\nTransactionID: {order.Info.TransactionID}");
            this.ExecutePurchaseFailedCallback();
        }

        private void OnPurchasePending(PendingOrder order)
        {
            Debug.Log($"[{Tag}] Your purchase is pending.");
            this.storeController.ConfirmPurchase(order);
        }

        private void OnPurchaseConfirmed(Order order)
        {
            Debug.Log($"[{Tag}] Your product purchase was successful.");
            // This place is used for adding purchased items to the inventory
            this.ExecutePurchaseSuccessCallback();
        }

        private void ExecutePurchaseSuccessCallback()
        {
            Debug.Log($"[{Tag}] Buying product success.");
            this.onProductBuySuccess?.Invoke();
            this.onProductBuySuccess = null;
        }

        private void ExecutePurchaseFailedCallback()
        {
            Debug.Log($"[{Tag}] Buying product failed.");
            this.onProductBuyFailed?.Invoke();
            this.onProductBuyFailed = null;
        }

        private void SetupPurchaseCallbacks(Action onBuySuccess = null, Action onBuyFailed = null)
        {
            Debug.Log($"[{Tag}] Setting up callbacks for product purchase.");
            this.onProductBuySuccess = onBuySuccess;
            this.onProductBuyFailed = onBuyFailed;
        }

        public void BuyProduct(string productId, Action onBuySuccess = null, Action onBuyFailed = null)
        {
            Debug.Log($"[{Tag}] Start buying product: {productId}");
            this.storeController.PurchaseProduct(productId);
            this.SetupPurchaseCallbacks(onBuySuccess, onBuyFailed);
        }

        public void RestorePurchase()
        {
            Debug.Log($"[{Tag}] Starting restore transactions...");
            this.storeController.RestoreTransactions(OnRestoreTransactionsSuccess);
        }

        private void OnRestoreTransactionsSuccess(bool success, string error)
        {
            string message = success
                ? $"[{Tag}] Restored transactions successfully"
                : $"[{Tag}] Failed to restore transactions: {error}";
            Debug.Log(message);
            OnRestoreEvent?.Invoke(success);
        }

        public void Dispose()
        {
            storeController.OnPurchasePending -= OnPurchasePending;
            storeController.OnPurchaseDeferred -= OnPurchaseDeferred;
            storeController.OnPurchaseConfirmed -= OnPurchaseConfirmed;
            storeController.OnPurchaseFailed -= OnPurchaseFailed;
            storeController.OnStoreDisconnected -= OnStoreDisconnected;
            storeController.OnCheckEntitlement -= OnCheckEntitlement;
            storeController.OnProductsFetched -= OnProductsFetched;
            storeController.OnProductsFetchFailed -= OnProductsFetchFailed;
        }
    }
}
