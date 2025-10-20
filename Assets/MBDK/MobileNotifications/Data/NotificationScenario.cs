using System.Collections.Generic;
using UnityEngine;

namespace MBDK.MobileNotifications.Data
{
    /// <summary>
    /// Scenario chứa nhiều notifications được setup từ trước
    /// </summary>
    /// <remarks>
    /// Scenario cho phép tạo một chuỗi notifications theo kịch bản
    /// như daily reminder, engagement campaign, hoặc checkpoint rewards.
    /// </remarks>
    [CreateAssetMenu(fileName = "NotificationScenario", menuName = "MBDK/Mobile Notifications/Notification Scenario")]
    public class NotificationScenario : ScriptableObject
    {
        [Header("Scenario Information")]
        [Tooltip("Tên scenario để dễ nhận biết")]
        public string scenarioName;

        [Tooltip("Mô tả về scenario này")]
        [TextArea(3, 6)]
        public string description;

        [Header("Checkpoint Settings")]
        [Tooltip("Scenario này dựa vào checkpoint hay không")]
        public bool useCheckpoint;

        [Tooltip("Tên checkpoint để tính toán timing (nếu useCheckpoint = true)")]
        public string checkpointName;

        [Header("Notifications")]
        [Tooltip("Danh sách notifications trong scenario")]
        public List<NotificationData> notifications;

        [Header("Scheduling Options")]
        [Tooltip("Tự động hủy scenario cũ trước khi schedule scenario mới")]
        public bool cancelPreviousOnSchedule;

        [Tooltip("Group key chung cho tất cả notifications trong scenario")]
        public string groupKey;

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public NotificationScenario()
        {
            this.scenarioName = "New Scenario";
            this.description = string.Empty;
            this.useCheckpoint = false;
            this.checkpointName = string.Empty;
            this.notifications = new List<NotificationData>();
            this.cancelPreviousOnSchedule = true;
            this.groupKey = string.Empty;
        }

        /// <summary>
        /// Validate scenario data
        /// </summary>
        /// <returns>True nếu scenario hợp lệ</returns>
        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(this.scenarioName))
            {
                return false;
            }

            if (this.notifications == null || this.notifications.Count == 0)
            {
                return false;
            }

            if (this.useCheckpoint && string.IsNullOrWhiteSpace(this.checkpointName))
            {
                return false;
            }

            // Validate từng notification trong scenario
            for (int i = 0; i < this.notifications.Count; i++)
            {
                if (!this.notifications[i].IsValid())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Lấy tổng số notifications trong scenario
        /// </summary>
        /// <value>Số lượng notifications</value>
        public int NotificationCount => this.notifications?.Count ?? 0;

        /// <summary>
        /// Thêm notification vào scenario
        /// </summary>
        /// <param name="notification">Notification cần thêm</param>
        public void AddNotification(NotificationData notification)
        {
            if (this.notifications == null)
            {
                this.notifications = new List<NotificationData>();
            }

            // Apply group key nếu có
            if (!string.IsNullOrWhiteSpace(this.groupKey))
            {
                notification.groupKey = this.groupKey;
            }

            this.notifications.Add(notification);
        }

        /// <summary>
        /// Remove notification khỏi scenario
        /// </summary>
        /// <param name="index">Index của notification</param>
        public void RemoveNotificationAt(int index)
        {
            if (this.notifications != null && index >= 0 && index < this.notifications.Count)
            {
                this.notifications.RemoveAt(index);
            }
        }

        /// <summary>
        /// Clear tất cả notifications trong scenario
        /// </summary>
        public void ClearNotifications()
        {
            if (this.notifications != null)
            {
                this.notifications.Clear();
            }
        }
    }
}

