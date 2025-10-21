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
        private FirebaseRemoteConfig _remoteConfig;
        
        public bool IsFirebaseInitialized => this._isInitialized;

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
                        this.InitializeRemoteConfig().Forget();
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
        
        private async UniTaskVoid InitializeRemoteConfig()
        {
            try
            {
                _remoteConfig = FirebaseRemoteConfig.DefaultInstance;
                _remoteConfig.OnConfigUpdateListener += OnConfigUpdateListener;
                var configSettings = new ConfigSettings
                {
                    MinimumFetchIntervalInMilliseconds = 3600000 // 1 giờ
                };

                await _remoteConfig.SetConfigSettingsAsync(configSettings);
                Debug.Log("Firebase Remote Config initialized successfully!");
                await this.FetchDataAsync();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Remote Config initialization failed: {ex.Message}");
            }
        }

        private async UniTask FetchDataAsync()
        {
            try
            {
                await this._remoteConfig.FetchAsync(TimeSpan.Zero).ContinueWithOnMainThread(FetchComplete);
                Debug.Log("Remote Config fetched and activated!");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Remote Config fetch failed: {ex.Message}");
            }
        }

        private void FetchComplete(Task fetchTask)
        {
            if (!fetchTask.IsCompleted)
            {
                Debug.LogError("Retrieval hasn't finished.");
                return;
            }
            
            ConfigInfo configInfo = _remoteConfig.Info;
            
            if (configInfo.LastFetchStatus != LastFetchStatus.Success)
            {
                Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(configInfo.LastFetchStatus)}: {configInfo.LastFetchStatus}");
                return;
            }
            
            _remoteConfig.ActivateAsync().ContinueWithOnMainThread(_ =>
            {
                Debug.Log($"Remote data loaded and ready for use. Last fetch time {configInfo.FetchTime}.");
            });
        }
        
        private void OnConfigUpdateListener(object sender, ConfigUpdateEventArgs e)
        {
            if (e.Error != RemoteConfigError.None)
            {
                Debug.Log($"Error occurred while listening: {e.Error}");
                return;
            }

            string updatedKey = string.Join(", ", e.UpdatedKeys);
            Debug.Log($"Updated keys: {updatedKey}");

            this._remoteConfig.ActivateAsync().ContinueWithOnMainThread(_ =>
            {
                DisplayWelcomeMessage();
            });
            
            void DisplayWelcomeMessage()
            {
                Debug.Log("You are now on the latest version of remote config!");
            }
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

        public string GetRemoteConfigString(string key)
        {
            return IsFirebaseInitialized ? FirebaseRemoteConfig.DefaultInstance.GetValue(key).StringValue : string.Empty;
        }

        public bool GetRemoteConfigBool(string key)
        {
            return IsFirebaseInitialized && FirebaseRemoteConfig.DefaultInstance.GetValue(key).BooleanValue;
        }

        public long GetRemoteConfigLong(string key)
        {
            return IsFirebaseInitialized ? FirebaseRemoteConfig.DefaultInstance.GetValue(key).LongValue : 0;
        }
        
        public void Cleanup()
        {
            if (!IsFirebaseInitialized) 
                return;
            
            FirebaseMessaging.TokenReceived -= OnTokenReceived;
            FirebaseMessaging.MessageReceived -= OnMessageReceived;
            
            if (_remoteConfig != null)
                _remoteConfig.OnConfigUpdateListener -= OnConfigUpdateListener;
        }
    }
}