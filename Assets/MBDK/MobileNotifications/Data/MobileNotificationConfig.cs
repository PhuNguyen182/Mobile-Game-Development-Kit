using System.Collections.Generic;
using UnityEngine;

namespace MBDK.MobileNotifications.Data
{
    /// <summary>
    /// Configuration cho mobile notification system
    /// </summary>
    /// <remarks>
    /// ScriptableObject này chứa tất cả settings và configurations
    /// cần thiết để setup notification system cho game.
    /// </remarks>
    [CreateAssetMenu(fileName = "MobileNotificationConfig", menuName = "MBDK/Mobile Notifications/Config")]
    public class MobileNotificationConfig : ScriptableObject
    {
        [Header("General Settings")]
        [Tooltip("Enable debug logs cho notification system")]
        public bool enableDebugLogs = false;

        [Tooltip("Tự động request permission khi initialize")]
        public bool autoRequestPermission = true;

        [Tooltip("Maximum số notifications có thể schedule cùng lúc")]
        public int maxScheduledNotifications = 64;

        [Header("Android Settings")]
        [Tooltip("Default notification channel cho Android")]
        public string androidDefaultChannelId = "default_channel";

        [Tooltip("Default notification channel name")]
        public string androidDefaultChannelName = "Default Notifications";

        [Tooltip("Default notification channel description")]
        public string androidDefaultChannelDescription = "General game notifications";

        [Tooltip("Default small icon name (phải có trong res/drawable)")]
        public string androidSmallIcon = "icon_notification";

        [Tooltip("Default large icon name (optional)")]
        public string androidLargeIcon = string.Empty;

        [Header("iOS Settings")]
        [Tooltip("Request authorization options cho iOS")]
        public bool iosRequestAlert = true;

        [Tooltip("Request badge authorization cho iOS")]
        public bool iosRequestBadge = true;

        [Tooltip("Request sound authorization cho iOS")]
        public bool iosRequestSound = true;

        [Header("Notification Channels")]
        [Tooltip("Danh sách custom notification channels (Android)")]
        public List<NotificationChannelData> customChannels;

        [Header("Default Values")]
        [Tooltip("Default badge number cho notifications")]
        public int defaultBadge = 1;

        [Tooltip("Default category/channel cho notifications")]
        public string defaultCategory = string.Empty;

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public MobileNotificationConfig()
        {
            this.enableDebugLogs = false;
            this.autoRequestPermission = true;
            this.maxScheduledNotifications = 64;
            this.androidDefaultChannelId = "default_channel";
            this.androidDefaultChannelName = "Default Notifications";
            this.androidDefaultChannelDescription = "General game notifications";
            this.androidSmallIcon = "icon_notification";
            this.androidLargeIcon = string.Empty;
            this.iosRequestAlert = true;
            this.iosRequestBadge = true;
            this.iosRequestSound = true;
            this.customChannels = new List<NotificationChannelData>();
            this.defaultBadge = 1;
            this.defaultCategory = string.Empty;
        }

        /// <summary>
        /// Validate configuration
        /// </summary>
        /// <returns>True nếu config hợp lệ</returns>
        public bool IsValid()
        {
            if (this.maxScheduledNotifications <= 0)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.androidDefaultChannelId))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.androidDefaultChannelName))
            {
                return false;
            }

            return !string.IsNullOrWhiteSpace(this.androidSmallIcon);
        }

        /// <summary>
        /// Tạo preset config cho Development
        /// </summary>
        /// <returns>Config với settings phù hợp cho development</returns>
        public static MobileNotificationConfig CreateDevelopmentPreset()
        {
            var config = CreateInstance<MobileNotificationConfig>();
            config.enableDebugLogs = true;
            config.autoRequestPermission = true;
            config.maxScheduledNotifications = 32;
            return config;
        }

        /// <summary>
        /// Tạo preset config cho Production
        /// </summary>
        /// <returns>Config với settings phù hợp cho production</returns>
        public static MobileNotificationConfig CreateProductionPreset()
        {
            var config = CreateInstance<MobileNotificationConfig>();
            config.enableDebugLogs = false;
            config.autoRequestPermission = false;
            config.maxScheduledNotifications = 64;
            return config;
        }
    }
}

