using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MBDK.MobileNotifications.Data;

namespace MBDK.MobileNotifications.Interfaces
{
    /// <summary>
    /// Interface chính để quản lý toàn bộ hệ thống mobile notifications
    /// </summary>
    /// <remarks>
    /// Interface này cung cấp các phương thức để khởi tạo, quản lý và điều khiển
    /// hệ thống mobile notifications cross-platform cho game.
    /// </remarks>
    public interface IMobileNotificationManager
    {
        /// <summary>
        /// Khởi tạo notification manager với configuration được cung cấp
        /// </summary>
        /// <param name="config">Configuration cho notification system</param>
        /// <returns>UniTask để theo dõi quá trình khởi tạo</returns>
        public UniTask InitializeAsync(MobileNotificationConfig config);

        /// <summary>
        /// Kiểm tra xem notification manager đã được khởi tạo chưa
        /// </summary>
        /// <value>True nếu đã khởi tạo, false nếu chưa</value>
        public bool IsInitialized { get; }

        /// <summary>
        /// Request quyền hiển thị notification từ người dùng
        /// </summary>
        /// <returns>UniTask với bool cho biết quyền đã được cấp hay chưa</returns>
        public UniTask<bool> RequestPermissionAsync();

        /// <summary>
        /// Kiểm tra xem đã có quyền hiển thị notification hay chưa
        /// </summary>
        /// <value>True nếu có quyền, false nếu không</value>
        public bool HasPermission { get; }

        /// <summary>
        /// Schedule một notification với data được cung cấp
        /// </summary>
        /// <param name="notificationData">Data của notification cần schedule</param>
        /// <returns>ID của notification đã được schedule</returns>
        public UniTask<int> ScheduleNotificationAsync(NotificationData notificationData);

        /// <summary>
        /// Schedule nhiều notifications cùng lúc
        /// </summary>
        /// <param name="notifications">Danh sách notifications cần schedule</param>
        /// <returns>Danh sách IDs của các notifications đã được schedule</returns>
        public UniTask<List<int>> ScheduleMultipleNotificationsAsync(List<NotificationData> notifications);

        /// <summary>
        /// Schedule notification scenario đã được setup từ trước
        /// </summary>
        /// <param name="scenario">Scenario cần schedule</param>
        /// <returns>Danh sách IDs của các notifications trong scenario</returns>
        public UniTask<List<int>> ScheduleScenarioAsync(NotificationScenario scenario);

        /// <summary>
        /// Hủy một notification đã được schedule
        /// </summary>
        /// <param name="notificationId">ID của notification cần hủy</param>
        public void CancelNotification(int notificationId);

        /// <summary>
        /// Hủy tất cả notifications đã được schedule
        /// </summary>
        public void CancelAllNotifications();

        /// <summary>
        /// Hủy tất cả notifications đã được delivered (hiển thị trong notification center)
        /// </summary>
        public void ClearDeliveredNotifications();

        /// <summary>
        /// Lấy danh sách tất cả notifications đang được schedule
        /// </summary>
        /// <returns>Danh sách notification data đang scheduled</returns>
        public List<NotificationData> GetScheduledNotifications();

        /// <summary>
        /// Event được raise khi notification permission thay đổi
        /// </summary>
        public event Action<bool> OnPermissionChanged;

        /// <summary>
        /// Event được raise khi có notification được tap bởi user
        /// </summary>
        public event Action<NotificationData> OnNotificationReceived;

        /// <summary>
        /// Event được raise khi có lỗi xảy ra trong notification system
        /// </summary>
        public event Action<string> OnNotificationError;
    }
}

