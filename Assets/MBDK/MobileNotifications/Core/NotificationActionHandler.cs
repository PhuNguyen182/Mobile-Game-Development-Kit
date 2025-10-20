using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using MBDK.MobileNotifications.Data;
using MBDK.MobileNotifications.Interfaces;

namespace MBDK.MobileNotifications.Core
{

    /// <summary>
    /// Handler để xử lý các actions từ notifications khi người chơi tap
    /// </summary>
    /// <remarks>
    /// Class này routing actions đến các handlers tương ứng và
    /// quản lý execution flow của notification actions.
    /// </remarks>
    public class NotificationActionHandler
    {
        // Delegates cho các action handlers
        public delegate UniTask<bool> ActionHandlerDelegate(NotificationAction action, NotificationData notification);

        // Dictionary chứa các registered handlers
        private readonly Dictionary<NotificationActionType, ActionHandlerDelegate> actionHandlers;

        // Configuration
        private MobileNotificationConfig config;

        // Callbacks
        private Action<NotificationAction> onActionStarted;
        private Action<NotificationAction, bool> onActionCompleted;
        private Action<NotificationAction, string> onActionError;

        /// <summary>
        /// Constructor
        /// </summary>
        public NotificationActionHandler()
        {
            this.actionHandlers = new Dictionary<NotificationActionType, ActionHandlerDelegate>();
        }

        /// <summary>
        /// Khởi tạo handler với configuration
        /// </summary>
        /// <param name="config">Configuration</param>
        public void Initialize(MobileNotificationConfig config)
        {
            this.config = config;

            if (this.config.enableDebugLogs)
            {
                Debug.Log("⚙️ [NotificationActionHandler] Initialized");
            }
        }

        /// <summary>
        /// Register handler cho một action type
        /// </summary>
        /// <param name="actionType">Loại action</param>
        /// <param name="handler">Handler function</param>
        public void RegisterHandler(NotificationActionType actionType, ActionHandlerDelegate handler)
        {
            if (handler == null)
            {
                Debug.LogWarning($"⚠️ [NotificationActionHandler] Handler is null for {actionType}");
                return;
            }

            this.actionHandlers[actionType] = handler;

            if (this.config != null && this.config.enableDebugLogs)
            {
                Debug.Log($"✅ [NotificationActionHandler] Handler registered for {actionType}");
            }
        }

        /// <summary>
        /// Unregister handler cho một action type
        /// </summary>
        /// <param name="actionType">Loại action</param>
        public void UnregisterHandler(NotificationActionType actionType)
        {
            if (this.actionHandlers.Remove(actionType))
            {
                if (this.config != null && this.config.enableDebugLogs)
                {
                    Debug.Log($"🗑️ [NotificationActionHandler] Handler unregistered for {actionType}");
                }
            }
        }

        /// <summary>
        /// Set callback khi action bắt đầu
        /// </summary>
        /// <param name="callback">Callback function</param>
        public void SetOnActionStarted(Action<NotificationAction> callback)
        {
            this.onActionStarted = callback;
        }

        /// <summary>
        /// Set callback khi action complete
        /// </summary>
        /// <param name="callback">Callback function (action, success)</param>
        public void SetOnActionCompleted(Action<NotificationAction, bool> callback)
        {
            this.onActionCompleted = callback;
        }

        /// <summary>
        /// Set callback khi action có error
        /// </summary>
        /// <param name="callback">Callback function (action, errorMessage)</param>
        public void SetOnActionError(Action<NotificationAction, string> callback)
        {
            this.onActionError = callback;
        }

        /// <summary>
        /// Process notification và execute action nếu có
        /// </summary>
        /// <param name="notification">Notification data</param>
        /// <returns>True nếu action được executed successfully</returns>
        public async UniTask<bool> ProcessNotificationAsync(NotificationData notification)
        {
            if (notification == null)
            {
                Debug.LogWarning("⚠️ [NotificationActionHandler] Notification is null");
                return false;
            }

            if (string.IsNullOrWhiteSpace(notification.customData))
            {
                if (this.config != null && this.config.enableDebugLogs)
                {
                    Debug.Log($"📭 [NotificationActionHandler] No custom data in notification: {notification.title}");
                }
                return false;
            }

            try
            {
                // Parse action từ customData
                var action = NotificationAction.FromJson(notification.customData);

                if (action == null || !action.IsValid())
                {
                    if (this.config != null && this.config.enableDebugLogs)
                    {
                        Debug.LogWarning($"⚠️ [NotificationActionHandler] Invalid action in notification: {notification.title}");
                    }
                    return false;
                }

                // Execute action
                return await this.ExecuteActionAsync(action, notification);
            }
            catch (Exception ex)
            {
                Debug.LogError($"❌ [NotificationActionHandler] Error processing notification: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Execute một action
        /// </summary>
        /// <param name="action">Action cần execute</param>
        /// <param name="notification">Notification gốc</param>
        /// <returns>True nếu success</returns>
        public async UniTask<bool> ExecuteActionAsync(NotificationAction action, NotificationData notification)
        {
            if (action == null || !action.IsValid())
            {
                Debug.LogWarning("⚠️ [NotificationActionHandler] Invalid action");
                return false;
            }

            try
            {
                if (this.config != null && this.config.enableDebugLogs)
                {
                    Debug.Log($"🎯 [NotificationActionHandler] Executing action: {action.actionType} for {notification.title}");
                }

                // Trigger onActionStarted callback
                this.onActionStarted?.Invoke(action);

                // Check nếu có registered handler
                if (!this.actionHandlers.TryGetValue(action.actionType, out var handler))
                {
                    Debug.LogWarning($"⚠️ [NotificationActionHandler] No handler registered for {action.actionType}");
                    
                    // Trigger error callback
                    this.onActionError?.Invoke(action, $"No handler for {action.actionType}");
                    return false;
                }

                // Execute handler
                var success = await handler(action, notification);

                if (this.config != null && this.config.enableDebugLogs)
                {
                    Debug.Log($"{(success ? "✅" : "❌")} [NotificationActionHandler] Action {action.actionType} {(success ? "completed" : "failed")}");
                }

                // Trigger onActionCompleted callback
                this.onActionCompleted?.Invoke(action, success);

                return success;
            }
            catch (Exception ex)
            {
                Debug.LogError($"❌ [NotificationActionHandler] Error executing action: {ex.Message}");
                
                // Trigger error callback
                this.onActionError?.Invoke(action, ex.Message);
                
                return false;
            }
        }

        /// <summary>
        /// Check nếu có handler cho action type
        /// </summary>
        /// <param name="actionType">Action type</param>
        /// <returns>True nếu có handler</returns>
        public bool HasHandler(NotificationActionType actionType)
        {
            return this.actionHandlers.ContainsKey(actionType);
        }

        /// <summary>
        /// Lấy số lượng handlers đã registered
        /// </summary>
        /// <value>Số lượng handlers</value>
        public int RegisteredHandlerCount => this.actionHandlers.Count;

        /// <summary>
        /// Clear tất cả handlers
        /// </summary>
        public void ClearHandlers()
        {
            this.actionHandlers.Clear();

            if (this.config != null && this.config.enableDebugLogs)
            {
                Debug.Log("🗑️ [NotificationActionHandler] All handlers cleared");
            }
        }
    }
}

