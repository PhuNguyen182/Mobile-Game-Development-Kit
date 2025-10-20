using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MBDK.MobileNotifications.Data;
using MBDK.MobileNotifications.Interfaces;
using UnityEngine;

#if UNITY_ANDROID
using Unity.Notifications.Android;
#elif UNITY_IOS
using Unity.Notifications.iOS;
#endif

namespace MBDK.MobileNotifications.Core
{
    /// <summary>
    /// Implementation để schedule và quản lý notifications
    /// </summary>
    /// <remarks>
    /// Class này xử lý việc schedule, cancel và query notifications
    /// trên cả Android và iOS platforms sử dụng Unity Mobile Notifications API.
    /// </remarks>
    public class NotificationScheduler : INotificationScheduler
    {
        private MobileNotificationConfig _config;
        private readonly Dictionary<int, NotificationData> _scheduledNotifications;
        private int _nextNotificationId;
        private bool _isInitialized;

        /// <summary>
        /// Kiểm tra xem scheduler đã được khởi tạo chưa
        /// </summary>
        /// <value>True nếu đã khởi tạo</value>
        public bool IsInitialized => this._isInitialized;

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public NotificationScheduler()
        {
            this._scheduledNotifications = new Dictionary<int, NotificationData>();
            this._nextNotificationId = 1;
            this._isInitialized = false;
        }

        /// <summary>
        /// Khởi tạo notification scheduler với configuration
        /// </summary>
        /// <param name="config">Configuration cho scheduler</param>
        public async UniTask InitializeAsync(MobileNotificationConfig config)
        {
            try
            {
                this._config = config;

                if (this._config.enableDebugLogs)
                {
                    Debug.Log("⚙️ [NotificationScheduler] Initializing...");
                }

                // Initialize platform-specific notification centers
#if UNITY_ANDROID
                await this.InitializeAndroidAsync();
#elif UNITY_IOS
                await this.InitializeIOSAsync();
#endif

                this._isInitialized = true;

                if (this._config.enableDebugLogs)
                {
                    Debug.Log("✅ [NotificationScheduler] Initialized successfully");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"❌ [NotificationScheduler] Initialization failed: {ex.Message}");
                this._isInitialized = false;
            }

            await UniTask.CompletedTask;
        }

        /// <summary>
        /// Schedule một notification single
        /// </summary>
        /// <param name="notificationData">Data của notification</param>
        /// <returns>ID của notification đã schedule</returns>
        public async UniTask<int> ScheduleAsync(NotificationData notificationData)
        {
            if (!this._isInitialized)
            {
                Debug.LogError("❌ [NotificationScheduler] Scheduler chưa được initialized!");
                return -1;
            }

            if (notificationData == null || !notificationData.IsValid())
            {
                Debug.LogError("❌ [NotificationScheduler] Invalid notification data!");
                return -1;
            }

            try
            {
                // Assign ID nếu chưa có
                if (notificationData.identifier == 0)
                {
                    notificationData.identifier = this._nextNotificationId++;
                }

                if (this._config.enableDebugLogs)
                {
                    Debug.Log($"📅 [NotificationScheduler] Scheduling notification #{notificationData.identifier}: {notificationData.title}");
                }

#if UNITY_ANDROID
                await this.ScheduleAndroidNotificationAsync(notificationData);
#elif UNITY_IOS
                await this.ScheduleIOSNotificationAsync(notificationData);
#else
                Debug.LogWarning("⚠️ [NotificationScheduler] Platform không hỗ trợ notifications");
                return -1;
#endif

                // Store notification data
                this._scheduledNotifications[notificationData.identifier] = notificationData;

                if (this._config.enableDebugLogs)
                {
                    Debug.Log($"✅ [NotificationScheduler] Notification #{notificationData.identifier} scheduled successfully");
                }

                return notificationData.identifier;
            }
            catch (Exception ex)
            {
                Debug.LogError($"❌ [NotificationScheduler] Error scheduling notification: {ex.Message}");
                return -1;
            }
        }

        /// <summary>
        /// Schedule nhiều notifications cùng lúc
        /// </summary>
        /// <param name="notifications">Danh sách notifications</param>
        /// <returns>Danh sách IDs đã schedule</returns>
        public async UniTask<List<int>> ScheduleMultipleAsync(List<NotificationData> notifications)
        {
            var scheduledIds = new List<int>();

            if (notifications == null || notifications.Count == 0)
            {
                Debug.LogWarning("⚠️ [NotificationScheduler] Empty notification list");
                return scheduledIds;
            }

            if (this._config.enableDebugLogs)
            {
                Debug.Log($"📅 [NotificationScheduler] Scheduling {notifications.Count} notifications...");
            }

            for (int i = 0; i < notifications.Count; i++)
            {
                var notificationId = await this.ScheduleAsync(notifications[i]);
                
                if (notificationId > 0)
                {
                    scheduledIds.Add(notificationId);
                }
            }

            if (this._config.enableDebugLogs)
            {
                Debug.Log($"✅ [NotificationScheduler] Scheduled {scheduledIds.Count}/{notifications.Count} notifications");
            }

            return scheduledIds;
        }

        /// <summary>
        /// Hủy một notification theo ID
        /// </summary>
        /// <param name="notificationId">ID của notification</param>
        public void Cancel(int notificationId)
        {
            if (!this._isInitialized)
            {
                Debug.LogError("❌ [NotificationScheduler] Scheduler chưa được initialized!");
                return;
            }

            try
            {
                if (this._config.enableDebugLogs)
                {
                    Debug.Log($"🗑️ [NotificationScheduler] Cancelling notification #{notificationId}");
                }

#if UNITY_ANDROID
                AndroidNotificationCenter.CancelScheduledNotification(notificationId);
#elif UNITY_IOS
                iOSNotificationCenter.RemoveScheduledNotification(notificationId.ToString());
#endif

                // Remove from tracking
                this._scheduledNotifications.Remove(notificationId);

                if (this._config.enableDebugLogs)
                {
                    Debug.Log($"✅ [NotificationScheduler] Notification #{notificationId} cancelled");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"❌ [NotificationScheduler] Error cancelling notification: {ex.Message}");
            }
        }

        /// <summary>
        /// Hủy tất cả scheduled notifications
        /// </summary>
        public void CancelAll()
        {
            if (!this._isInitialized)
            {
                Debug.LogError("❌ [NotificationScheduler] Scheduler chưa được initialized!");
                return;
            }

            try
            {
                if (this._config.enableDebugLogs)
                {
                    Debug.Log("🗑️ [NotificationScheduler] Cancelling all notifications...");
                }

#if UNITY_ANDROID
                AndroidNotificationCenter.CancelAllScheduledNotifications();
#elif UNITY_IOS
                iOSNotificationCenter.RemoveAllScheduledNotifications();
#endif

                // Clear tracking
                this._scheduledNotifications.Clear();

                if (this._config.enableDebugLogs)
                {
                    Debug.Log("✅ [NotificationScheduler] All notifications cancelled");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"❌ [NotificationScheduler] Error cancelling all notifications: {ex.Message}");
            }
        }

        /// <summary>
        /// Clear tất cả delivered notifications (trong notification center)
        /// </summary>
        public void ClearDelivered()
        {
            if (!this._isInitialized)
            {
                Debug.LogError("❌ [NotificationScheduler] Scheduler chưa được initialized!");
                return;
            }

            try
            {
                if (this._config.enableDebugLogs)
                {
                    Debug.Log("🧹 [NotificationScheduler] Clearing delivered notifications...");
                }

#if UNITY_ANDROID
                AndroidNotificationCenter.CancelAllDisplayedNotifications();
#elif UNITY_IOS
                iOSNotificationCenter.RemoveAllDeliveredNotifications();
#endif

                if (this._config.enableDebugLogs)
                {
                    Debug.Log("✅ [NotificationScheduler] Delivered notifications cleared");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"❌ [NotificationScheduler] Error clearing delivered notifications: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy danh sách notifications đang scheduled
        /// </summary>
        /// <returns>Danh sách notification data</returns>
        public List<NotificationData> GetScheduledNotifications()
        {
            var notifications = new List<NotificationData>();

            foreach (var kvp in this._scheduledNotifications)
            {
                notifications.Add(kvp.Value);
            }

            return notifications;
        }

#if UNITY_ANDROID
        /// <summary>
        /// Khởi tạo Android notification center
        /// </summary>
        private async UniTask InitializeAndroidAsync()
        {
            try
            {
                // Tạo default notification channel
                var defaultChannel = new AndroidNotificationChannel
                {
                    Id = this.config.androidDefaultChannelId,
                    Name = this.config.androidDefaultChannelName,
                    Description = this.config.androidDefaultChannelDescription,
                    Importance = Importance.Default
                };

                AndroidNotificationCenter.RegisterNotificationChannel(defaultChannel);

                if (this.config.enableDebugLogs)
                {
                    Debug.Log($"📱 [NotificationScheduler] Android default channel created: {defaultChannel.Id}");
                }

                // Register custom channels
                if (this.config.customChannels != null && this.config.customChannels.Count > 0)
                {
                    for (int i = 0; i < this.config.customChannels.Count; i++)
                    {
                        var channelData = this.config.customChannels[i];
                        
                        var customChannel = new AndroidNotificationChannel
                        {
                            Id = channelData.channelId,
                            Name = channelData.channelName,
                            Description = channelData.description,
                            Importance = (Importance)channelData.importance
                        };

                        AndroidNotificationCenter.RegisterNotificationChannel(customChannel);

                        if (this.config.enableDebugLogs)
                        {
                            Debug.Log($"📱 [NotificationScheduler] Custom channel created: {customChannel.Id}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"❌ [NotificationScheduler] Android initialization error: {ex.Message}");
            }

            await UniTask.CompletedTask;
        }

        /// <summary>
        /// Schedule notification trên Android
        /// </summary>
        private async UniTask ScheduleAndroidNotificationAsync(NotificationData data)
        {
            try
            {
                var notification = new AndroidNotification
                {
                    Title = data.title,
                    Text = data.body,
                    FireTime = DateTime.Now.AddSeconds(data.fireTimeInSeconds),
                    SmallIcon = string.IsNullOrWhiteSpace(data.smallIcon) ? this.config.androidSmallIcon : data.smallIcon,
                    LargeIcon = string.IsNullOrWhiteSpace(data.largeIcon) ? this.config.androidLargeIcon : data.largeIcon
                };

                // Set group key nếu có
                if (!string.IsNullOrWhiteSpace(data.groupKey))
                {
                    notification.Group = data.groupKey;
                }

                // Set repeat nếu cần
                if (data.repeats && data.repeatInterval > 0)
                {
                    notification.RepeatInterval = TimeSpan.FromSeconds(data.repeatInterval);
                }

                // Set custom data
                if (!string.IsNullOrWhiteSpace(data.customData))
                {
                    notification.IntentData = data.customData;
                }

                // Get channel ID
                var channelId = string.IsNullOrWhiteSpace(data.category) 
                    ? this.config.androidDefaultChannelId 
                    : data.category;

                // Schedule notification
                AndroidNotificationCenter.SendNotificationWithExplicitID(
                    notification, 
                    channelId, 
                    data.identifier
                );
            }
            catch (Exception ex)
            {
                Debug.LogError($"❌ [NotificationScheduler] Android schedule error: {ex.Message}");
            }

            await UniTask.CompletedTask;
        }
#endif

#if UNITY_IOS
        /// <summary>
        /// Khởi tạo iOS notification center
        /// </summary>
        private async UniTask InitializeIOSAsync()
        {
            try
            {
                // iOS không cần setup channels như Android
                // Chỉ cần clear old notifications nếu cần
                iOSNotificationCenter.RemoveAllDeliveredNotifications();

                if (this.config.enableDebugLogs)
                {
                    Debug.Log("📱 [NotificationScheduler] iOS notification center initialized");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"❌ [NotificationScheduler] iOS initialization error: {ex.Message}");
            }

            await UniTask.CompletedTask;
        }

        /// <summary>
        /// Schedule notification trên iOS
        /// </summary>
        private async UniTask ScheduleIOSNotificationAsync(NotificationData data)
        {
            try
            {
                var notification = new iOSNotification
                {
                    Identifier = data.identifier.ToString(),
                    Title = data.title,
                    Body = data.body,
                    Subtitle = data.subtitle,
                    Badge = data.badge > 0 ? data.badge : this.config.defaultBadge,
                    ShowInForeground = true,
                    ForegroundPresentationOption = (PresentationOption.Alert | 
                                                   PresentationOption.Sound | 
                                                   PresentationOption.Badge),
                    CategoryIdentifier = string.IsNullOrWhiteSpace(data.category) 
                        ? this.config.defaultCategory 
                        : data.category
                };

                // Set trigger time
                var timeTrigger = new iOSNotificationTimeIntervalTrigger
                {
                    TimeInterval = TimeSpan.FromSeconds(data.fireTimeInSeconds),
                    Repeats = data.repeats
                };

                notification.Trigger = timeTrigger;

                // Set custom data
                if (!string.IsNullOrWhiteSpace(data.customData))
                {
                    notification.Data = data.customData;
                }

                // Schedule notification
                iOSNotificationCenter.ScheduleNotification(notification);
            }
            catch (Exception ex)
            {
                Debug.LogError($"❌ [NotificationScheduler] iOS schedule error: {ex.Message}");
            }

            await UniTask.CompletedTask;
        }
#endif
    }
}

