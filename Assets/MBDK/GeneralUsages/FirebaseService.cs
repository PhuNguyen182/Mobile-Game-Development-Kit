using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.Messaging;
using Firebase.RemoteConfig;
using Firebase.Crashlytics;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MBDK.GeneralUsages
{
    public class FirebaseService
    {
        private bool _isInitialized;
        private FirebaseApp _firebaseApp;
        
        public bool IsFirebaseInitialized => this._isInitialized;
        public FirebaseRemoteConfigService FirebaseRemoteConfigService { get; private set; }

        public FirebaseService()
        {
            InitializeFirebase().Forget();
        }

        private async UniTask InitializeFirebase()
        {
            try
            {
                await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
                {
                    var dependencyStatus = task.Result;
                    if (dependencyStatus == DependencyStatus.Available)
                    {
                        this._firebaseApp = FirebaseApp.DefaultInstance;
                        Debug.Log($"Firebase initialized successfully!\n" +
                                  $"Firebase app name: {this._firebaseApp.Name} with option: {this._firebaseApp.Options}");
                        _isInitialized = true;

                        this.InitializeAnalytics();
                        this.InitializeRemoteConfig();
                        this.InitializeCloudMessaging();
                        this.InitializeCrashlytics();
                    }
                    else
                    {
                        Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                        this._isInitialized = false;
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.LogError($"Firebase initialization failed: {ex.Message}");
                this._isInitialized = false;
            }
        }

        #region Firebase Analytics
        
        private void InitializeAnalytics()
        {
            try
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                Debug.Log("Firebase Analytics initialized successfully!");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Analytics initialization failed: {ex.Message}");
            }
        }
        
        #endregion

        #region Firebase Remote Config
        
        private void InitializeRemoteConfig()
        {
            this.FirebaseRemoteConfigService = new FirebaseRemoteConfigService();
        }
        
        #endregion

        #region Firebase Cloud Messaging
        
        private void InitializeCloudMessaging()
        {
            try
            {
                FirebaseMessaging.TokenReceived += OnTokenReceived;
                FirebaseMessaging.MessageReceived += OnMessageReceived;
                Debug.Log("Firebase Cloud Messaging initialized successfully!");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Cloud Messaging initialization failed: {ex.Message}");
            }
        }

        private void OnTokenReceived(object sender, TokenReceivedEventArgs args)
        {
            Debug.Log($"FCM Token received: {args.Token}");
            // Lưu token để gửi lên server nếu cần
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs args)
        {
            Debug.Log("Message received!");
        
            if (args.Message.Notification != null)
            {
                Debug.Log($"Title: {args.Message.Notification.Title}");
                Debug.Log($"Body: {args.Message.Notification.Body}");
            }

            if (args.Message.Data is not { Count: > 0 }) 
                return;
            
            Debug.Log("Message data:");
            foreach (var kvp in args.Message.Data)
            {
                Debug.Log($"{kvp.Key}: {kvp.Value}");
            }
        }
        
        #endregion
        
        #region Firebase Crashlytics
        private void InitializeCrashlytics()
        {
            Crashlytics.ReportUncaughtExceptionsAsFatal = true;
            Debug.Log($"Firebase Crashlytics Initialized successfully with ReportUncaughtExceptionsAsFatal set to True!");
        }
        #endregion
        
        public void Cleanup()
        {
            if (!IsFirebaseInitialized) 
                return;
            
            FirebaseMessaging.TokenReceived -= OnTokenReceived;
            FirebaseMessaging.MessageReceived -= OnMessageReceived;
            this.FirebaseRemoteConfigService.Dispose();
        }
    }
}