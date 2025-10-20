using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MBDK.MobileNotifications.Data;

namespace MBDK.MobileNotifications.Interfaces
{
    /// <summary>
    /// Interface để schedule và quản lý notifications
    /// </summary>
    /// <remarks>
    /// Scheduler này xử lý việc schedule, cancel và query các notifications
    /// trên cả Android và iOS platforms.
    /// </remarks>
    public interface INotificationScheduler
    {
        /// <summary>
        /// Khởi tạo notification scheduler với configuration
        /// </summary>
        /// <param name="config">Configuration cho scheduler</param>
        public UniTask InitializeAsync(MobileNotificationConfig config);

        /// <summary>
        /// Schedule một notification single
        /// </summary>
        /// <param name="notificationData">Data của notification</param>
        /// <returns>ID của notification đã schedule</returns>
        public UniTask<int> ScheduleAsync(NotificationData notificationData);

        /// <summary>
        /// Schedule nhiều notifications cùng lúc
        /// </summary>
        /// <param name="notifications">Danh sách notifications</param>
        /// <returns>Danh sách IDs đã schedule</returns>
        public UniTask<List<int>> ScheduleMultipleAsync(List<NotificationData> notifications);

        /// <summary>
        /// Hủy một notification theo ID
        /// </summary>
        /// <param name="notificationId">ID của notification</param>
        public void Cancel(int notificationId);

        /// <summary>
        /// Hủy tất cả scheduled notifications
        /// </summary>
        public void CancelAll();

        /// <summary>
        /// Clear tất cả delivered notifications (trong notification center)
        /// </summary>
        public void ClearDelivered();

        /// <summary>
        /// Lấy danh sách notifications đang scheduled
        /// </summary>
        /// <returns>Danh sách notification data</returns>
        public List<NotificationData> GetScheduledNotifications();

        /// <summary>
        /// Kiểm tra xem scheduler đã được khởi tạo chưa
        /// </summary>
        /// <value>True nếu đã khởi tạo</value>
        public bool IsInitialized { get; }
    }
}

