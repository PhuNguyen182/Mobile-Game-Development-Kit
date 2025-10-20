using System;
using UnityEngine;

namespace MBDK.MobileNotifications.Data
{
    /// <summary>
    /// Data cho một notification channel (Android)
    /// </summary>
    [Serializable]
    public class NotificationChannelData
    {
        [Tooltip("ID duy nhất của channel")]
        public string channelId;

        [Tooltip("Tên hiển thị của channel")]
        public string channelName;

        [Tooltip("Mô tả về channel")]
        public string description;

        [Tooltip("Importance level (0-4): None, Min, Low, Default, High")]
        [Range(0, 4)]
        public int importance;

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public NotificationChannelData()
        {
            this.channelId = string.Empty;
            this.channelName = string.Empty;
            this.description = string.Empty;
            this.importance = 3; // Default importance
        }

        /// <summary>
        /// Constructor với parameters
        /// </summary>
        /// <param name="channelId">ID của channel</param>
        /// <param name="channelName">Tên channel</param>
        /// <param name="description">Mô tả channel</param>
        /// <param name="importance">Importance level</param>
        public NotificationChannelData(string channelId, string channelName, string description, int importance = 3)
        {
            this.channelId = channelId;
            this.channelName = channelName;
            this.description = description;
            this.importance = Mathf.Clamp(importance, 0, 4);
        }
    }
}