using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

public class InAppPurchaseValidator
{
    private CrossPlatformValidator _crossPlatformValidator;

    private void CreateCrossPlatformValidator()
    {
#if !UNITY_EDITOR
        try
        {
            if (CanCrossPlatformValidate())
            {
                _crossPlatformValidator = new CrossPlatformValidator(GooglePlayTangle.Data(), Application.identifier);
            }
        }
        catch (NotImplementedException exception)
        {
            Debug.Log("===========");
            Debug.LogError($"Cross Platform Validator Not Implemented: {exception}");
        }
#endif
    }

    public void ValidatePurchaseIfPossible(IOrderInfo orderInfo)
    {
        if (CanCrossPlatformValidate())
        {
            ValidatePurchase(orderInfo);
        }
    }
    
    private void ValidatePurchase(IOrderInfo orderInfo)
    {
        try
        {
            var result = _crossPlatformValidator.Validate(orderInfo.Receipt);

            if (IsGooglePlay())
            {
                Debug.Log("Validated Receipt. Contents:");
                foreach (IPurchaseReceipt productReceipt in result)
                {
                    Debug.Log(productReceipt);
                }
            }
            else
            {
                Debug.Log("Validated Receipt.");
            }
        }
        catch (IAPSecurityException ex)
        {
            Debug.LogError($"Invalid receipt, not unlocking content. {ex.Message}");
        }
    }
    
    private bool CanCrossPlatformValidate() => this.IsGooglePlay() || this.IsApple();

    private bool IsGooglePlay()
    {
        return Application.platform == RuntimePlatform.Android &&
               DefaultStoreHelper.GetDefaultStoreName() == GooglePlay.Name;
    }

    private bool IsApple()
    {
#if UNITY_IOS
        return true;
#else
        return false;
#endif
    }
}
