using System;
using Cysharp.Threading.Tasks;

namespace MBDK.MobileNotifications.Interfaces
{
    /// <summary>
    /// Interface để xử lý quyền truy cập notification từ người dùng
    /// </summary>
    /// <remarks>
    /// Handler này chịu trách nhiệm request và kiểm tra quyền hiển thị
    /// notifications trên cả Android và iOS.
    /// </remarks>
    public interface INotificationPermissionHandler
    {
        /// <summary>
        /// Request quyền hiển thị notification từ user
        /// </summary>
        /// <returns>UniTask với bool cho biết permission đã được granted</returns>
        public UniTask<bool> RequestPermissionAsync();

        /// <summary>
        /// Kiểm tra xem hiện tại có quyền hiển thị notification không
        /// </summary>
        /// <returns>True nếu có quyền, false nếu không</returns>
        public bool CheckPermission();

        /// <summary>
        /// Event được raise khi permission status thay đổi
        /// </summary>
        public event Action<bool> OnPermissionStatusChanged;

        /// <summary>
        /// Lấy trạng thái permission hiện tại
        /// </summary>
        /// <value>True nếu đã granted permission, false nếu chưa</value>
        public bool HasPermission { get; }
    }
}

