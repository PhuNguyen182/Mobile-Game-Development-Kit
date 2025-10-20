using System;
using UnityEngine;

namespace MBDK.MobileNotifications.Data
{
    /// <summary>
    /// Data model chứa thông tin của một mobile notification
    /// </summary>
    /// <remarks>
    /// Class này chứa tất cả thông tin cần thiết để tạo và hiển thị
    /// một notification trên cả Android và iOS.
    /// </remarks>
    [Serializable]
    public class NotificationData
    {
        [Header("Basic Information")]
        [Tooltip("ID duy nhất của notification (auto-generated nếu = 0)")]
        public int identifier;

        [Tooltip("Tiêu đề của notification")]
        public string title;

        [Tooltip("Nội dung chính của notification")]
        public string body;

        [Tooltip("Subtitle cho iOS (optional)")]
        public string subtitle;

        [Header("Timing")]
        [Tooltip("Thời gian delay trước khi hiển thị (seconds)")]
        public long fireTimeInSeconds;

        [Tooltip("Badge number hiển thị trên app icon (iOS)")]
        public int badge;

        [Header("Grouping & Category")]
        [Tooltip("Category identifier (iOS) hoặc Channel ID (Android)")]
        public string category;

        [Tooltip("Group key để nhóm notifications (Android)")]
        public string groupKey;

        [Header("Media")]
        [Tooltip("Path đến small icon (Android)")]
        public string smallIcon;

        [Tooltip("Path đến large icon (Android/iOS)")]
        public string largeIcon;

        [Header("Repeat Settings")]
        [Tooltip("Notification có lặp lại không")]
        public bool repeats;

        [Tooltip("Interval lặp lại (seconds) nếu repeats = true")]
        public long repeatInterval;

        [Header("Custom Data")]
        [Tooltip("Custom data để xử lý khi notification được tap")]
        public string customData;

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public NotificationData()
        {
            this.identifier = 0;
            this.title = string.Empty;
            this.body = string.Empty;
            this.subtitle = string.Empty;
            this.fireTimeInSeconds = 0;
            this.badge = 0;
            this.category = string.Empty;
            this.groupKey = string.Empty;
            this.smallIcon = string.Empty;
            this.largeIcon = string.Empty;
            this.repeats = false;
            this.repeatInterval = 0;
            this.customData = string.Empty;
        }

        /// <summary>
        /// Constructor với parameters cơ bản
        /// </summary>
        /// <param name="title">Tiêu đề notification</param>
        /// <param name="body">Nội dung notification</param>
        /// <param name="fireTimeInSeconds">Thời gian delay (seconds)</param>
        public NotificationData(string title, string body, long fireTimeInSeconds)
        {
            this.identifier = 0;
            this.title = title;
            this.body = body;
            this.subtitle = string.Empty;
            this.fireTimeInSeconds = fireTimeInSeconds;
            this.badge = 0;
            this.category = string.Empty;
            this.groupKey = string.Empty;
            this.smallIcon = string.Empty;
            this.largeIcon = string.Empty;
            this.repeats = false;
            this.repeatInterval = 0;
            this.customData = string.Empty;
        }

        /// <summary>
        /// Tạo bản copy của notification data
        /// </summary>
        /// <returns>NotificationData mới với cùng giá trị</returns>
        public NotificationData Clone()
        {
            return new NotificationData
            {
                identifier = this.identifier,
                title = this.title,
                body = this.body,
                subtitle = this.subtitle,
                fireTimeInSeconds = this.fireTimeInSeconds,
                badge = this.badge,
                category = this.category,
                groupKey = this.groupKey,
                smallIcon = this.smallIcon,
                largeIcon = this.largeIcon,
                repeats = this.repeats,
                repeatInterval = this.repeatInterval,
                customData = this.customData
            };
        }

        /// <summary>
        /// Validate notification data
        /// </summary>
        /// <returns>True nếu data hợp lệ</returns>
        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(this.title))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.body))
            {
                return false;
            }

            if (this.fireTimeInSeconds < 0)
            {
                return false;
            }

            if (this.repeats && this.repeatInterval <= 0)
            {
                return false;
            }

            return true;
        }
    }
}

