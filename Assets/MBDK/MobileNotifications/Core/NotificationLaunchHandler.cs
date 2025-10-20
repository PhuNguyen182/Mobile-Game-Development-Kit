using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using MBDK.MobileNotifications.Data;
using MBDK.MobileNotifications.Interfaces;

namespace MBDK.MobileNotifications.Core
{

    /// <summary>
    /// Handler xử lý logic khi app được launch từ notification
    /// </summary>
    /// <remarks>
    /// Class này xử lý các trường hợp khác nhau khi app mở:
    /// - Cold Start: App hoàn toàn đóng, mở từ notification
    /// - Warm Start: App trong background, tap notification để foreground
    /// - Hot Start: App đang chạy, nhận notification
    /// </remarks>
    public class NotificationLaunchHandler : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField]
        [Tooltip("Enable debug logs")]
        private bool enableDebugLogs = true;

        [SerializeField]
        [Tooltip("Delay sau khi app ready trước khi process notification (milliseconds)")]
        private int processDelayMs = 1000;

        [Header("Dependencies")]
        [SerializeField]
        [Tooltip("Reference đến NotificationRouter")]
        private NotificationRouter notificationRouter;

        // State tracking
        private NotificationData pendingNotification;
        private bool isAppReady;
        private bool hasProcessedLaunchNotification;

        // Events
        public event Action<AppLaunchState> OnAppLaunched;
        public event Action<NotificationData> OnLaunchFromNotification;

        /// <summary>
        /// Unity Awake lifecycle
        /// </summary>
        private void Awake()
        {
            // Detect launch state
            this.DetectLaunchState();
        }

        /// <summary>
        /// Unity Start lifecycle
        /// </summary>
        private async void Start()
        {
            await this.WaitForAppReadyAsync();
        }

        /// <summary>
        /// Unity OnApplicationPause lifecycle
        /// </summary>
        /// <param name="pauseStatus">True khi app pause (vào background)</param>
        private void OnApplicationPause(bool pauseStatus)
        {
            if (this.enableDebugLogs)
            {
                Debug.Log($"📱 [NotificationLaunchHandler] App {(pauseStatus ? "paused" : "resumed")}");
            }

            if (!pauseStatus)
            {
                // App resumed từ background
                this.HandleAppResumed();
            }
            else
            {
                // App vào background
                this.HandleAppPaused();
            }
        }

        /// <summary>
        /// Unity OnApplicationFocus lifecycle
        /// </summary>
        /// <param name="hasFocus">True khi app có focus</param>
        private void OnApplicationFocus(bool hasFocus)
        {
            if (this.enableDebugLogs)
            {
                Debug.Log($"🎯 [NotificationLaunchHandler] App focus: {hasFocus}");
            }

            if (hasFocus && !this.hasProcessedLaunchNotification)
            {
                // Check nếu có pending notification từ launch
                this.CheckLaunchNotificationAsync().Forget();
            }
        }

        /// <summary>
        /// Detect launch state của app
        /// </summary>
        private void DetectLaunchState()
        {
            // Check nếu app launched từ notification
            var launchState = this.GetLaunchState();

            if (this.enableDebugLogs)
            {
                Debug.Log($"🚀 [NotificationLaunchHandler] App launch state: {launchState}");
            }

            // Trigger event
            this.OnAppLaunched?.Invoke(launchState);
        }

        /// <summary>
        /// Lấy launch state của app
        /// </summary>
        /// <returns>AppLaunchState enum</returns>
        private AppLaunchState GetLaunchState()
        {
            // Kiểm tra PlayerPrefs để xác định launch state
            var lastPauseTime = PlayerPrefs.GetFloat("app_last_pause_time", 0f);
            var currentTime = Time.realtimeSinceStartup;

            if (lastPauseTime == 0f)
            {
                // First launch hoặc cold start
                return AppLaunchState.ColdStart;
            }

            var timeSincePause = currentTime - lastPauseTime;

            if (timeSincePause < 5f)
            {
                // App vừa mở lại gần đây - hot start
                return AppLaunchState.HotStart;
            }
            else if (timeSincePause < 300f)
            {
                // App trong background ít hơn 5 phút - warm start
                return AppLaunchState.WarmStart;
            }
            else
            {
                // App đã đóng lâu - cold start
                return AppLaunchState.ColdStart;
            }
        }

        /// <summary>
        /// Wait cho app ready trước khi process notification
        /// </summary>
        private async UniTask WaitForAppReadyAsync()
        {
            if (this.enableDebugLogs)
            {
                Debug.Log("⏳ [NotificationLaunchHandler] Waiting for app to be ready...");
            }

            // Wait delay để đảm bảo tất cả managers đã initialized
            await UniTask.Delay(this.processDelayMs);

            this.isAppReady = true;

            if (this.enableDebugLogs)
            {
                Debug.Log("✅ [NotificationLaunchHandler] App is ready!");
            }

            // Check launch notification
            await this.CheckLaunchNotificationAsync();
        }

        /// <summary>
        /// Check nếu app launched từ notification
        /// </summary>
        private async UniTask CheckLaunchNotificationAsync()
        {
            if (!this.isAppReady || this.hasProcessedLaunchNotification)
            {
                return;
            }

            if (this.notificationRouter == null)
            {
                Debug.LogWarning("⚠️ [NotificationLaunchHandler] NotificationRouter is null!");
                return;
            }

            // Check pending notification
            if (this.pendingNotification != null)
            {
                if (this.enableDebugLogs)
                {
                    Debug.Log($"📬 [NotificationLaunchHandler] Processing pending notification: {this.pendingNotification.title}");
                }

                // Trigger event
                this.OnLaunchFromNotification?.Invoke(this.pendingNotification);

                // Route notification
                await this.notificationRouter.RouteNotificationAsync(this.pendingNotification);

                // Mark as processed
                this.hasProcessedLaunchNotification = true;
                this.pendingNotification = null;
            }
        }

        /// <summary>
        /// Set pending notification để process khi app ready
        /// </summary>
        /// <param name="notification">Notification data</param>
        public void SetPendingNotification(NotificationData notification)
        {
            if (notification == null)
            {
                Debug.LogWarning("⚠️ [NotificationLaunchHandler] Notification is null!");
                return;
            }

            this.pendingNotification = notification;

            if (this.enableDebugLogs)
            {
                Debug.Log($"📋 [NotificationLaunchHandler] Pending notification set: {notification.title}");
            }

            // Process ngay nếu app đã ready
            if (this.isAppReady && !this.hasProcessedLaunchNotification)
            {
                this.CheckLaunchNotificationAsync().Forget();
            }
        }

        /// <summary>
        /// Handle khi app resumed từ background
        /// </summary>
        private void HandleAppResumed()
        {
            if (this.enableDebugLogs)
            {
                Debug.Log("▶️ [NotificationLaunchHandler] App resumed from background");
            }

            // Reset processed flag để có thể process notification mới
            this.hasProcessedLaunchNotification = false;

            // Check nếu có notification mới
            this.CheckLaunchNotificationAsync().Forget();
        }

        /// <summary>
        /// Handle khi app paused (vào background)
        /// </summary>
        private void HandleAppPaused()
        {
            if (this.enableDebugLogs)
            {
                Debug.Log("⏸️ [NotificationLaunchHandler] App paused (going to background)");
            }

            // Save pause time
            PlayerPrefs.SetFloat("app_last_pause_time", Time.realtimeSinceStartup);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Clear pending notification
        /// </summary>
        public void ClearPendingNotification()
        {
            this.pendingNotification = null;
            this.hasProcessedLaunchNotification = false;

            if (this.enableDebugLogs)
            {
                Debug.Log("🗑️ [NotificationLaunchHandler] Pending notification cleared");
            }
        }

        /// <summary>
        /// Check nếu có pending notification
        /// </summary>
        /// <value>True nếu có pending notification</value>
        public bool HasPendingNotification => this.pendingNotification != null;
    }

    /// <summary>
    /// Enum định nghĩa các trạng thái launch của app
    /// </summary>
    public enum AppLaunchState
    {
        /// <summary>
        /// App mở lần đầu hoặc đã đóng hoàn toàn
        /// </summary>
        ColdStart = 0,

        /// <summary>
        /// App trong background một thời gian ngắn
        /// </summary>
        WarmStart = 1,

        /// <summary>
        /// App đang chạy, chỉ resume
        /// </summary>
        HotStart = 2
    }
}

