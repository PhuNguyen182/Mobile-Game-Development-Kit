using System;
using Crosstales.OnlineCheck;
using UnityEngine;

namespace MBDK.OnlineChecking
{
    public class OnlineCheckService : IDisposable
    {
        private bool _isDisposed;
        private bool _lastConnectingState;
        private float _lastCheckTime;
        private NetworkReachability _networkReachability;
        
        public event Action OnReconnected;
        public event Action OnDisconnected;

        public bool IsConnected =>
            this._lastConnectingState && this._networkReachability != NetworkReachability.NotReachable;

        public OnlineCheckService()
        {
            if (OnlineCheck.Instance)
            {
                OnlineCheck.Instance.OnOnlineStatusChange += OnOnlineStatusChange;
                OnlineCheck.Instance.OnOnlineCheckComplete += OnOnlineCheckComplete;
            }

            this._networkReachability = Application.internetReachability;
            this._lastConnectingState = Application.internetReachability != NetworkReachability.NotReachable;
            this._lastCheckTime = Time.time;
        }

        private void OnOnlineCheckComplete(bool isConnected, NetworkReachability networkReachability)
        {
            if (!this._lastConnectingState && isConnected)
            {
                Debug.Log($"Internet reconnected! Network reachability: {networkReachability}");
                this.OnReconnected?.Invoke();
                return;
            }

            if (!isConnected)
            {
                Debug.LogError($"Internet disconnected! Network reachability: {networkReachability}");
                this.OnDisconnected?.Invoke();
            }

            this.PrintNetworkCheckState(isConnected);
            this._lastConnectingState = isConnected;
            this._networkReachability = networkReachability;
        }

        private void OnOnlineStatusChange(bool isConnected)
        {
            if (!this._lastConnectingState && isConnected)
            {
                Debug.Log("Internet reconnected!");
                this.OnReconnected?.Invoke();
                return;
            }
            
            if (!isConnected)
            {
                Debug.LogError("Internet disconnected!");
                this.OnDisconnected?.Invoke();
            }
            
            this.PrintNetworkCheckState(isConnected);
            this._lastConnectingState = isConnected;
        }

        private void PrintNetworkCheckState(bool isConnected)
        {
            float timeSinceLastCheck = Time.time - this._lastCheckTime;
            string status = isConnected ? "Connected" : "Disconnected";
            Debug.Log($"Internet check within duration: {timeSinceLastCheck} with status: {status}");
            this._lastCheckTime = Time.time;
        }

        private void ReleaseUnmanagedResources()
        {
            this.OnReconnected = null;
            this.OnDisconnected = null;
            
            if (OnlineCheck.Instance)
            {
                OnlineCheck.Instance.OnOnlineStatusChange -= OnOnlineStatusChange;
                OnlineCheck.Instance.OnOnlineCheckComplete -= OnOnlineCheckComplete;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._isDisposed)
                return;

            if (disposing)
            {
                ReleaseUnmanagedResources();
            }

            this._isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~OnlineCheckService()
        {
            Dispose(false);
        }
    }
}
