using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using UnityEngine;

namespace MBDK.GeneralUsages
{
    public class FirebaseRemoteConfigService : IDisposable
    {
        private bool _disposed;
        private FirebaseRemoteConfig _remoteConfig;
        
        public async UniTask InitializeRemoteConfig()
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
        
        public string GetRemoteConfigString(string key)
        {
            string value = this._remoteConfig.GetValue(key).StringValue;
            Debug.Log($"Fetched string value for key {key}: {value}");
            return value;
        }

        public bool GetRemoteConfigBool(string key)
        {
            bool value = this._remoteConfig.GetValue(key).BooleanValue;
            Debug.Log($"Fetched bool value for key {key}: {value}");
            return value;
        }

        public long GetRemoteConfigLong(string key)
        {
            long value = this._remoteConfig.GetValue(key).LongValue;
            Debug.Log($"Fetched long value for key {key}: {value}");
            return value;
        }
        
        public double GetRemoteConfigDouble(string key)
        {
            double value = this._remoteConfig.GetValue(key).DoubleValue;
            Debug.Log($"Fetched double value for key {key}: {value}");
            return value;
        }
        
        public IEnumerable<byte> GetRemoteConfigByteArray(string key)
        {
            IEnumerable<byte> value = this._remoteConfig.GetValue(key).ByteArrayValue;
            Debug.Log($"Fetched byte array value for key {key}: {value}");
            return value;
        }

        private void ReleaseUnmanagedResources()
        {
            if (_remoteConfig != null)
                _remoteConfig.OnConfigUpdateListener -= OnConfigUpdateListener;
        }

        private void Dispose(bool disposing)
        {
            if (this._disposed)
                return;
            
            if (disposing)
            {
                ReleaseUnmanagedResources();
            }
            
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~FirebaseRemoteConfigService()
        {
            Dispose(false);
        }
    }
}
