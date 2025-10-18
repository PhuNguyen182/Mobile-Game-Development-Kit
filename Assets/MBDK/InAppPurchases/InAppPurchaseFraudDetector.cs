using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Purchasing;

public class InAppPurchaseFraudDetector
{
    private readonly StoreController _storeController;

    public InAppPurchaseFraudDetector(StoreController storeController)
    {
        this._storeController = storeController;
    }

    public void ConfigureFraudDetection(string userName = null, string accountId = null, string profileId = null)
    {
#if UNITY_ANDROID
        var googlePlayStoreExtendedService = _storeController.GooglePlayStoreExtendedService;
        ConfigureGoogleFraudDetection(googlePlayStoreExtendedService, accountId, profileId);
#elif UNITY_IOS
        ConfigureAppleFraudDetection(userName);
#endif
    }

    private void ConfigureAppleFraudDetection(string userName)
    {
        //To make sure the account id and profile id do not contain personally identifiable information, we obfuscate this information by hashing it.
        var hashedUsername = HashString(userName);
        _storeController.AppleStoreExtendedService?.SetAppAccountToken(new Guid(hashedUsername));
    }

    private void ConfigureGoogleFraudDetection(IGooglePlayStoreExtendedService googlePlayStoreExtendedService,
        string accountId, string profileId)
    {
        if (googlePlayStoreExtendedService == null)
        {
            Debug.Log(
                "Google Play Store Extended Service is not available. Please make sure the project is being built for Android and the Google Play Store.");
            return;
        }

        //To make sure the account id and profile id do not contain personally identifiable information, we obfuscate this information by hashing it.
        var obfuscatedAccountId = HashString(accountId);
        var obfuscatedProfileId = HashString(profileId);

        googlePlayStoreExtendedService.SetObfuscatedAccountId(obfuscatedAccountId);
        googlePlayStoreExtendedService.SetObfuscatedProfileId(obfuscatedProfileId);
    }

    private string HashString(string input)
    {
        var stringBuilder = new StringBuilder();
        foreach (var b in GetHash(input))
            stringBuilder.Append(b.ToString("X2"));

        return stringBuilder.ToString();
    }

    private IEnumerable<byte> GetHash(string input)
    {
        using HashAlgorithm algorithm = SHA256.Create();
        return algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
    }
}
