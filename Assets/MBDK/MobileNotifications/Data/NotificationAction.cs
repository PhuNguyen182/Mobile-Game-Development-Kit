using System;
using UnityEngine;

namespace MBDK.MobileNotifications.Data
{
    /// <summary>
    /// Enum định nghĩa các loại action có thể xử lý từ notification
    /// </summary>
    public enum NotificationActionType
    {
        None = 0,
        OpenScreen = 1,          // Mở một screen cụ thể
        OpenLevel = 2,           // Mở level cụ thể
        OpenShop = 3,            // Mở shop với item cụ thể
        ClaimReward = 4,         // Claim reward
        OpenDailyReward = 5,     // Mở màn hình daily reward
        OpenAchievements = 6,    // Mở màn hình achievements
        OpenEvent = 7,           // Mở event cụ thể
        OpenProfile = 8,         // Mở profile
        StartLevel = 9,          // Start level ngay
        CustomAction = 100       // Custom action với handler riêng
    }

    /// <summary>
    /// Data model chứa thông tin về action cần thực hiện khi notification được tap
    /// </summary>
    /// <remarks>
    /// Class này serialize thành JSON string để lưu trong notification.customData
    /// </remarks>
    [Serializable]
    public class NotificationAction
    {
        [Tooltip("Loại action cần thực hiện")]
        public NotificationActionType actionType;

        [Tooltip("ID của target (level ID, screen name, reward ID, etc.)")]
        public string targetId;

        [Tooltip("Additional parameters (JSON format)")]
        public string parameters;

        [Tooltip("Priority của action (higher = process first)")]
        public int priority;

        [Tooltip("Có show loading screen không")]
        public bool showLoading;

        [Tooltip("Message hiển thị khi processing")]
        public string loadingMessage;

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public NotificationAction()
        {
            this.actionType = NotificationActionType.None;
            this.targetId = string.Empty;
            this.parameters = string.Empty;
            this.priority = 0;
            this.showLoading = false;
            this.loadingMessage = "Loading...";
        }

        /// <summary>
        /// Constructor với parameters cơ bản
        /// </summary>
        /// <param name="actionType">Loại action</param>
        /// <param name="targetId">Target ID</param>
        public NotificationAction(NotificationActionType actionType, string targetId)
        {
            this.actionType = actionType;
            this.targetId = targetId;
            this.parameters = string.Empty;
            this.priority = 0;
            this.showLoading = false;
            this.loadingMessage = "Loading...";
        }

        /// <summary>
        /// Serialize action thành JSON string
        /// </summary>
        /// <returns>JSON string</returns>
        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        /// <summary>
        /// Deserialize từ JSON string
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <returns>NotificationAction object</returns>
        public static NotificationAction FromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return new NotificationAction();
            }

            try
            {
                return JsonUtility.FromJson<NotificationAction>(json);
            }
            catch (Exception ex)
            {
                Debug.LogError($"❌ [NotificationAction] Error parsing JSON: {ex.Message}");
                return new NotificationAction();
            }
        }

        /// <summary>
        /// Validate action data
        /// </summary>
        /// <returns>True nếu action hợp lệ</returns>
        public bool IsValid()
        {
            if (this.actionType == NotificationActionType.None)
            {
                return false;
            }

            // Một số action types yêu cầu targetId
            switch (this.actionType)
            {
                case NotificationActionType.OpenScreen:
                case NotificationActionType.OpenLevel:
                case NotificationActionType.OpenShop:
                case NotificationActionType.ClaimReward:
                case NotificationActionType.OpenEvent:
                case NotificationActionType.StartLevel:
                    if (string.IsNullOrWhiteSpace(this.targetId))
                    {
                        return false;
                    }
                    break;
            }

            return true;
        }

        /// <summary>
        /// Tạo action để mở screen
        /// </summary>
        /// <param name="screenName">Tên screen cần mở</param>
        /// <returns>NotificationAction</returns>
        public static NotificationAction CreateOpenScreenAction(string screenName)
        {
            return new NotificationAction(NotificationActionType.OpenScreen, screenName)
            {
                showLoading = true,
                loadingMessage = "Opening..."
            };
        }

        /// <summary>
        /// Tạo action để mở level
        /// </summary>
        /// <param name="levelId">ID của level</param>
        /// <returns>NotificationAction</returns>
        public static NotificationAction CreateOpenLevelAction(string levelId)
        {
            return new NotificationAction(NotificationActionType.OpenLevel, levelId)
            {
                showLoading = true,
                loadingMessage = "Loading level..."
            };
        }

        /// <summary>
        /// Tạo action để claim reward
        /// </summary>
        /// <param name="rewardId">ID của reward</param>
        /// <returns>NotificationAction</returns>
        public static NotificationAction CreateClaimRewardAction(string rewardId)
        {
            return new NotificationAction(NotificationActionType.ClaimReward, rewardId)
            {
                showLoading = false,
                priority = 10
            };
        }

        /// <summary>
        /// Tạo action để mở shop với item
        /// </summary>
        /// <param name="itemId">ID của item</param>
        /// <returns>NotificationAction</returns>
        public static NotificationAction CreateOpenShopAction(string itemId)
        {
            return new NotificationAction(NotificationActionType.OpenShop, itemId)
            {
                showLoading = true,
                loadingMessage = "Opening shop..."
            };
        }

        /// <summary>
        /// Tạo action để mở daily reward
        /// </summary>
        /// <returns>NotificationAction</returns>
        public static NotificationAction CreateOpenDailyRewardAction()
        {
            return new NotificationAction(NotificationActionType.OpenDailyReward, "daily_reward")
            {
                showLoading = true,
                loadingMessage = "Opening rewards...",
                priority = 5
            };
        }

        /// <summary>
        /// Tạo custom action với handler riêng
        /// </summary>
        /// <param name="actionId">ID của custom action</param>
        /// <param name="parameters">Parameters (JSON)</param>
        /// <returns>NotificationAction</returns>
        public static NotificationAction CreateCustomAction(string actionId, string parameters = "")
        {
            return new NotificationAction(NotificationActionType.CustomAction, actionId)
            {
                parameters = parameters,
                showLoading = false
            };
        }
    }
}

