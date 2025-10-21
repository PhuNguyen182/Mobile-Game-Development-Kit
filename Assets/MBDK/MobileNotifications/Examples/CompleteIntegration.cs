/*using UnityEngine;
using Cysharp.Threading.Tasks;
using MBDK.MobileNotifications.Core;
using MBDK.MobileNotifications.Data;
using MBDK.MobileNotifications.Interfaces;

namespace MBDK.MobileNotifications.Examples
{

    /// <summary>
    /// Example tích hợp hoàn chỉnh notification system với action handling
    /// </summary>
    /// <remarks>
    /// Script này demonstrate một integration hoàn chỉnh của:
    /// - Notification Manager
    /// - Notification Router
    /// - Action Handling
    /// - Deep Linking
    /// 
    /// Đây là example "production-ready" cho việc implement trong game thật.
    /// </remarks>
    public class CompleteIntegration : MonoBehaviour
    {
        [Header("Notification System")]
        [SerializeField]
        [Tooltip("Reference đến MobileNotificationManager")]
        private MobileNotificationManager notificationManager;

        [SerializeField]
        [Tooltip("Reference đến NotificationRouter")]
        private NotificationRouter notificationRouter;

        [Header("Configuration")]
        [SerializeField]
        [Tooltip("Config cho notification system")]
        private MobileNotificationConfig config;

        /// <summary>
        /// Unity Start lifecycle
        /// </summary>
        private async void Start()
        {
            await this.InitializeCompleteSystemAsync();
        }

        /// <summary>
        /// Unity OnEnable lifecycle
        /// </summary>
        private void OnEnable()
        {
            this.SubscribeToEvents();
        }

        /// <summary>
        /// Unity OnDisable lifecycle
        /// </summary>
        private void OnDisable()
        {
            this.UnsubscribeFromEvents();
        }

        /// <summary>
        /// Khởi tạo toàn bộ notification system
        /// </summary>
        private async UniTask InitializeCompleteSystemAsync()
        {
            Debug.Log("🚀 [CompleteIntegration] Initializing complete notification system...");

            // Step 1: Initialize notification manager
            if (this.notificationManager != null && this.config != null)
            {
                await this.notificationManager.InitializeAsync(this.config);
                Debug.Log("✅ [CompleteIntegration] Notification manager initialized");
            }
            else
            {
                Debug.LogError("❌ [CompleteIntegration] Missing manager or config!");
                return;
            }

            // Step 2: Wait for permission
            await UniTask.WaitUntil(() => this.notificationManager.IsInitialized);

            if (this.config.autoRequestPermission)
            {
                await UniTask.WaitUntil(() => this.notificationManager.HasPermission);
            }

            // Step 3: Setup router events
            this.SetupRouterHandlers();

            Debug.Log("✅ [CompleteIntegration] Complete system initialized!");

            // Step 4: Schedule welcome notifications (example)
            await this.ScheduleWelcomeNotificationsAsync();
        }

        /// <summary>
        /// Subscribe to notification events
        /// </summary>
        private void SubscribeToEvents()
        {
            if (this.notificationManager != null)
            {
                this.notificationManager.OnNotificationReceived += this.HandleNotificationReceived;
                this.notificationManager.OnPermissionChanged += this.HandlePermissionChanged;
                this.notificationManager.OnNotificationError += this.HandleNotificationError;
            }
        }

        /// <summary>
        /// Unsubscribe from events
        /// </summary>
        private void UnsubscribeFromEvents()
        {
            if (this.notificationManager != null)
            {
                this.notificationManager.OnNotificationReceived -= this.HandleNotificationReceived;
                this.notificationManager.OnPermissionChanged -= this.HandlePermissionChanged;
                this.notificationManager.OnNotificationError -= this.HandleNotificationError;
            }
        }

        /// <summary>
        /// Setup router event handlers
        /// </summary>
        private void SetupRouterHandlers()
        {
            if (this.notificationRouter == null)
            {
                Debug.LogWarning("⚠️ [CompleteIntegration] Notification router is null!");
                return;
            }

            // Subscribe to all router events
            this.notificationRouter.OnOpenScreen += (screenName) =>
            {
                Debug.Log($"📱 [CompleteIntegration] Router event: Open screen {screenName}");
                // Implement your screen navigation logic here
            };

            this.notificationRouter.OnOpenLevel += (levelId) =>
            {
                Debug.Log($"🎮 [CompleteIntegration] Router event: Open level {levelId}");
                // Implement your level loading logic here
            };

            this.notificationRouter.OnOpenShop += (itemId) =>
            {
                Debug.Log($"🛒 [CompleteIntegration] Router event: Open shop with {itemId}");
                // Implement your shop opening logic here
            };

            this.notificationRouter.OnClaimReward += (rewardId) =>
            {
                Debug.Log($"🎁 [CompleteIntegration] Router event: Claim reward {rewardId}");
                // Implement your reward claiming logic here
            };

            this.notificationRouter.OnOpenDailyReward += () =>
            {
                Debug.Log("🎁 [CompleteIntegration] Router event: Open daily reward");
                // Implement your daily reward screen logic here
            };

            this.notificationRouter.OnCustomAction += (actionId, parameters) =>
            {
                Debug.Log($"⚙️ [CompleteIntegration] Router event: Custom action {actionId}");
                this.HandleCustomRouterAction(actionId, parameters);
            };

            Debug.Log("✅ [CompleteIntegration] Router handlers setup complete");
        }

        /// <summary>
        /// Handle notification received
        /// </summary>
        private async void HandleNotificationReceived(NotificationData notification)
        {
            Debug.Log($"📬 [CompleteIntegration] Notification tapped: {notification.title}");

            // Route notification automatically
            if (this.notificationRouter != null)
            {
                var success = await this.notificationRouter.RouteNotificationAsync(notification);

                if (success)
                {
                    Debug.Log("✅ [CompleteIntegration] Notification processed successfully");
                }
                else
                {
                    Debug.LogWarning("⚠️ [CompleteIntegration] Failed to process notification");
                }
            }
        }

        /// <summary>
        /// Handle permission changed
        /// </summary>
        private void HandlePermissionChanged(bool granted)
        {
            Debug.Log($"🔐 [CompleteIntegration] Permission {(granted ? "granted" : "denied")}");

            if (granted)
            {
                // Permission granted - có thể schedule notifications
                this.ScheduleWelcomeNotificationsAsync().Forget();
            }
        }

        /// <summary>
        /// Handle notification error
        /// </summary>
        private void HandleNotificationError(string errorMessage)
        {
            Debug.LogError($"❌ [CompleteIntegration] Notification error: {errorMessage}");
        }

        /// <summary>
        /// Handle custom router actions
        /// </summary>
        private void HandleCustomRouterAction(string actionId, string parameters)
        {
            switch (actionId)
            {
                case "vip_offer":
                    this.HandleVIPOfferAsync(parameters).Forget();
                    break;

                case "limited_event":
                    this.HandleLimitedEvent(parameters);
                    break;

                case "comeback_bonus":
                    this.HandleComebackBonus(parameters);
                    break;

                default:
                    Debug.LogWarning($"⚠️ Unknown custom action: {actionId}");
                    break;
            }
        }

        // ============================================
        // Example: Schedule Notifications với Actions
        // ============================================

        /// <summary>
        /// Schedule welcome notifications khi player first time hoặc quay lại
        /// </summary>
        private async UniTask ScheduleWelcomeNotificationsAsync()
        {
            if (!this.notificationManager.HasPermission)
            {
                return;
            }

            Debug.Log("📅 [CompleteIntegration] Scheduling welcome notifications...");

            // Notification 1: Comeback reminder (6 giờ)
            var comebackAction = NotificationAction.CreateOpenScreenAction("main_menu");
            var comebackNotification = new NotificationData(
                "👋 Welcome Back!",
                "We miss you! Come back and continue your adventure!",
                60 * 60 * 6 // 6 giờ
            );
            comebackNotification.customData = comebackAction.ToJson();
            await this.notificationManager.ScheduleNotificationAsync(comebackNotification);

            // Notification 2: Daily reward (1 ngày)
            var dailyRewardAction = NotificationAction.CreateOpenDailyRewardAction();
            var dailyRewardNotification = new NotificationData(
                "🎁 Daily Reward Ready!",
                "Don't miss your daily reward! Claim it now!",
                60 * 60 * 24 // 1 ngày
            );
            dailyRewardNotification.customData = dailyRewardAction.ToJson();
            await this.notificationManager.ScheduleNotificationAsync(dailyRewardNotification);

            // Notification 3: Special offer (3 ngày)
            var offerParameters = JsonUtility.ToJson(new { offerId = "welcome_back_50", discount = 50 });
            var offerAction = NotificationAction.CreateCustomAction("vip_offer", offerParameters);
            var offerNotification = new NotificationData(
                "💰 Special Offer!",
                "50% OFF on all items! Limited time only!",
                60 * 60 * 24 * 3 // 3 ngày
            );
            offerNotification.customData = offerAction.ToJson();
            await this.notificationManager.ScheduleNotificationAsync(offerNotification);

            Debug.Log("✅ [CompleteIntegration] Welcome notifications scheduled");
        }

        /// <summary>
        /// PUBLIC: Schedule notification khi player complete level
        /// Gọi method này từ game code khi player finish level
        /// </summary>
        public async void OnPlayerCompletedLevel(int levelNumber)
        {
            if (!this.notificationManager.HasPermission)
            {
                return;
            }

            Debug.Log($"🏆 [CompleteIntegration] Player completed level {levelNumber}");

            // Cancel old level notifications
            this.notificationManager.CancelAllNotifications();

            // Schedule new notifications cho next level
            var nextLevel = levelNumber + 1;

            // Notification 1: Continue playing (2 giờ)
            var continueAction = NotificationAction.CreateOpenLevelAction(nextLevel.ToString());
            var continueNotification = new NotificationData(
                $"🎮 Ready for Level {nextLevel}?",
                "Your next challenge awaits! Are you ready?",
                60 * 60 * 2 // 2 giờ
            );
            continueNotification.customData = continueAction.ToJson();
            await this.notificationManager.ScheduleNotificationAsync(continueNotification);

            // Notification 2: Comeback reminder (1 ngày)
            var comebackAction = NotificationAction.CreateStartLevelAction(nextLevel.ToString());
            var comebackNotification = new NotificationData(
                "🌟 Don't Forget Your Progress!",
                $"You're on level {nextLevel}! Continue your journey!",
                60 * 60 * 24 // 1 ngày
            );
            comebackNotification.customData = comebackAction.ToJson();
            await this.notificationManager.ScheduleNotificationAsync(comebackNotification);

            Debug.Log($"✅ [CompleteIntegration] Level {nextLevel} notifications scheduled");
        }

        /// <summary>
        /// PUBLIC: Schedule notifications khi có event mới
        /// </summary>
        public async void OnNewEventStarted(string eventId, string eventName, long eventDurationSeconds)
        {
            if (!this.notificationManager.HasPermission)
            {
                return;
            }

            Debug.Log($"🎪 [CompleteIntegration] New event started: {eventName}");

            // Notification 1: Event started (immediate)
            var eventStartAction = NotificationAction.CreateOpenEventAction(eventId);
            var eventStartNotification = new NotificationData(
                $"🎉 {eventName} Started!",
                "Join now and win amazing rewards!",
                60 // 1 phút
            );
            eventStartNotification.customData = eventStartAction.ToJson();
            await this.notificationManager.ScheduleNotificationAsync(eventStartNotification);

            // Notification 2: Mid-event reminder (giữa event)
            var midEventTime = eventDurationSeconds / 2;
            var midEventAction = NotificationAction.CreateOpenEventAction(eventId);
            var midEventNotification = new NotificationData(
                $"⏰ {eventName} Halfway!",
                "Don't miss out! Time is running!",
                midEventTime
            );
            midEventNotification.customData = midEventAction.ToJson();
            await this.notificationManager.ScheduleNotificationAsync(midEventNotification);

            // Notification 3: Last chance (1 giờ trước end)
            var lastChanceTime = eventDurationSeconds - (60 * 60);
            if (lastChanceTime > 0)
            {
                var lastChanceAction = NotificationAction.CreateOpenEventAction(eventId);
                var lastChanceNotification = new NotificationData(
                    $"🔥 {eventName} Ending Soon!",
                    "Last chance to participate! Only 1 hour left!",
                    lastChanceTime
                );
                lastChanceNotification.customData = lastChanceAction.ToJson();
                await this.notificationManager.ScheduleNotificationAsync(lastChanceNotification);
            }

            Debug.Log($"✅ [CompleteIntegration] Event notifications scheduled");
        }

        /// <summary>
        /// PUBLIC: Cancel tất cả notifications
        /// Gọi khi player đang active trong game
        /// </summary>
        public void CancelAllActiveNotifications()
        {
            if (this.notificationManager != null)
            {
                this.notificationManager.CancelAllNotifications();
                Debug.Log("🗑️ [CompleteIntegration] All active notifications cancelled");
            }
        }

        // ============================================
        // Custom Action Handlers
        // ============================================

        private async UniTask HandleVIPOfferAsync(string parameters)
        {
            Debug.Log($"💰 [CompleteIntegration] Processing VIP offer: {parameters}");

            // Parse offer parameters
            // Example: { "offerId": "...", "discount": 50 }

            await UniTask.Delay(500);

            // Show offer popup or navigate to shop
            Debug.Log("✅ [CompleteIntegration] VIP offer shown");
        }

        private void HandleLimitedEvent(string parameters)
        {
            Debug.Log($"🎪 [CompleteIntegration] Processing limited event: {parameters}");

            // Navigate to event screen
            // UnityEngine.SceneManagement.SceneManager.LoadScene("Events");
        }

        private void HandleComebackBonus(string parameters)
        {
            Debug.Log($"🎁 [CompleteIntegration] Processing comeback bonus: {parameters}");

            // Grant comeback bonus to player
            // ShowBonusPopup();
        }
    }
}*/

