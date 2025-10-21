using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MBDK.MobileNotifications.Data;

namespace MBDK.MobileNotifications.Interfaces
{
    /// <summary>
    /// Interface cho service xử lý business logic của notification system
    /// </summary>
    /// <remarks>
    /// Service này xử lý các tác vụ như validate, transform data và quản lý
    /// lifecycle của notifications.
    /// </remarks>
    public interface IMobileNotificationService
    {
        /// <summary>
        /// Khởi tạo notification service với configuration
        /// </summary>
        /// <param name="config">Configuration cho service</param>
        public UniTask InitializeAsync(MobileNotificationConfig config);

        /// <summary>
        /// Validate notification data trước khi schedule
        /// </summary>
        /// <param name="data">Notification data cần validate</param>
        /// <returns>True nếu data hợp lệ, false nếu không</returns>
        public bool ValidateNotificationData(NotificationData data);

        /// <summary>
        /// Xử lý notification scenario và chuyển thành danh sách notifications
        /// </summary>
        /// <param name="scenario">Scenario cần xử lý</param>
        /// <returns>Danh sách notification data từ scenario</returns>
        public List<NotificationData> ProcessScenario(NotificationScenario scenario);

        /// <summary>
        /// Tính toán thời gian trigger cho notification dựa vào checkpoint
        /// </summary>
        /// <param name="checkpointName">Tên checkpoint</param>
        /// <param name="delayInSeconds">Thời gian delay từ checkpoint (giây)</param>
        /// <returns>Thời gian trigger tính theo seconds từ bây giờ</returns>
        public long CalculateTriggerTime(string checkpointName, long delayInSeconds);

        /// <summary>
        /// Cập nhật checkpoint cho việc tính toán notification timing
        /// </summary>
        /// <param name="checkpointName">Tên checkpoint</param>
        /// <param name="timestamp">Timestamp của checkpoint</param>
        public void UpdateCheckpoint(string checkpointName, long timestamp);

        /// <summary>
        /// Lấy configuration hiện tại của service
        /// </summary>
        /// <value>Configuration đang được sử dụng</value>
        public MobileNotificationConfig CurrentConfig { get; }
    }
}

