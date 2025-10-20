using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using MBDK.MobileNotifications.Core;
using MBDK.MobileNotifications.Data;
using MBDK.MobileNotifications.Interfaces;

namespace MBDK.MobileNotifications.Examples
{

    /// <summary>
    /// Example về cách handle notifications và routing actions
    /// </summary>
    /// <remarks>
    /// Script này demonstrate cách:
    /// - Setup NotificationRouter
    /// - Handle different notification types
    /// - Implement deep linking
    /// - Navigate đến screens/features cụ thể
    /// </remarks>
    public class NotificationHandling : MonoBehaviour
    {
        [Header("Notification System")]
        [SerializeField]
        [Tooltip("Reference đến MobileNotificationManager")]
        private MobileNotificationManager notificationManager;

        [SerializeField]
        [Tooltip("Reference đến NotificationRouter")]
        private NotificationRouter notificationRouter;

        [Header("Game Managers")]
        [SerializeField]
        [Tooltip("Reference đến GameManager của bạn")]
        private GameObject gameManager;

        /// <summary>
        /// Unity Start lifecycle
        /// </summary>
        private void Start()
        {
            this.SetupNotificationHandling();
        }

        /// <summary>
        /// Unity OnEnable lifecycle
        /// </summary>
        private void OnEnable()
        {
            // Subscribe to notification manager events
            if (this.notificationManager != null)
            {
                this.notificationManager.OnNotificationReceived += this.HandleNotificationReceived;
            }

            // Subscribe to router events
            if (this.notificationRouter != null)
            {
                this.notificationRouter.OnOpenScreen += this.HandleOpenScreen;
                this.notificationRouter.OnOpenLevel += this.HandleOpenLevel;
                this.notificationRouter.OnOpenShop += this.HandleOpenShop;
                this.notificationRouter.OnClaimReward += this.HandleClaimReward;
                this.notificationRouter.OnOpenDailyReward += this.HandleOpenDailyReward;
                this.notificationRouter.OnOpenAchievements += this.HandleOpenAchievements;
                this.notificationRouter.OnOpenEvent += this.HandleOpenEvent;
                this.notificationRouter.OnOpenProfile += this.HandleOpenProfile;
                this.notificationRouter.OnStartLevel += this.HandleStartLevel;
                this.notificationRouter.OnCustomAction += this.HandleCustomAction;
            }
        }

        /// <summary>
        /// Unity OnDisable lifecycle
        /// </summary>
        private void OnDisable()
        {
            // Unsubscribe
            if (this.notificationManager != null)
            {
                this.notificationManager.OnNotificationReceived -= this.HandleNotificationReceived;
            }

            if (this.notificationRouter != null)
            {
                this.notificationRouter.OnOpenScreen -= this.HandleOpenScreen;
                this.notificationRouter.OnOpenLevel -= this.HandleOpenLevel;
                this.notificationRouter.OnOpenShop -= this.HandleOpenShop;
                this.notificationRouter.OnClaimReward -= this.HandleClaimReward;
                this.notificationRouter.OnOpenDailyReward -= this.HandleOpenDailyReward;
                this.notificationRouter.OnOpenAchievements -= this.HandleOpenAchievements;
                this.notificationRouter.OnOpenEvent -= this.HandleOpenEvent;
                this.notificationRouter.OnOpenProfile -= this.HandleOpenProfile;
                this.notificationRouter.OnStartLevel -= this.HandleStartLevel;
                this.notificationRouter.OnCustomAction -= this.HandleCustomAction;
            }
        }

        /// <summary>
        /// Setup notification handling system
        /// </summary>
        private void SetupNotificationHandling()
        {
            Debug.Log("🎯 [NotificationHandling] Setting up notification handling...");

            // Verify references
            if (this.notificationManager == null)
            {
                Debug.LogError("❌ [NotificationHandling] NotificationManager is null!");
                return;
            }

            if (this.notificationRouter == null)
            {
                Debug.LogError("❌ [NotificationHandling] NotificationRouter is null!");
                return;
            }

            Debug.Log("✅ [NotificationHandling] Notification handling setup complete");
        }

        /// <summary>
        /// Handle notification received event
        /// </summary>
        private async void HandleNotificationReceived(NotificationData notification)
        {
            Debug.Log($"📬 [NotificationHandling] Notification received: {notification.title}");

            // Route notification đến handler tương ứng
            var success = await this.notificationRouter.RouteNotificationAsync(notification);

            if (success)
            {
                Debug.Log($"✅ [NotificationHandling] Notification routed successfully");
            }
            else
            {
                Debug.LogWarning($"⚠️ [NotificationHandling] Failed to route notification");
            }
        }

        // ============================================
        // Notification Action Handlers
        // ============================================

        /// <summary>
        /// Handle open screen action
        /// </summary>
        private void HandleOpenScreen(string screenName)
        {
            Debug.Log($"📱 [NotificationHandling] Opening screen: {screenName}");

            // Example: Load scene based on screen name
            switch (screenName.ToLower())
            {
                case "main_menu":
                    this.LoadScene("MainMenu");
                    break;

                case "gameplay":
                    this.LoadScene("Gameplay");
                    break;

                case "settings":
                    this.LoadScene("Settings");
                    break;

                default:
                    Debug.LogWarning($"⚠️ Unknown screen: {screenName}");
                    break;
            }
        }

        /// <summary>
        /// Handle open level action
        /// </summary>
        private void HandleOpenLevel(string levelId)
        {
            Debug.Log($"🎮 [NotificationHandling] Opening level: {levelId}");

            // Example: Load level selection screen với level được pre-selected
            PlayerPrefs.SetString("selected_level", levelId);
            this.LoadScene("LevelSelection");
        }

        /// <summary>
        /// Handle open shop action
        /// </summary>
        private void HandleOpenShop(string itemId)
        {
            Debug.Log($"🛒 [NotificationHandling] Opening shop with item: {itemId}");

            // Example: Load shop và highlight item cụ thể
            PlayerPrefs.SetString("shop_highlight_item", itemId);
            this.LoadScene("Shop");
        }

        /// <summary>
        /// Handle claim reward action
        /// </summary>
        private void HandleClaimReward(string rewardId)
        {
            Debug.Log($"🎁 [NotificationHandling] Claiming reward: {rewardId}");

            // Example: Auto-claim reward và show reward popup
            this.ClaimRewardAsync(rewardId).Forget();
        }

        /// <summary>
        /// Handle open daily reward action
        /// </summary>
        private void HandleOpenDailyReward()
        {
            Debug.Log("🎁 [NotificationHandling] Opening daily reward");

            // Example: Load daily reward screen
            this.LoadScene("DailyReward");
        }

        /// <summary>
        /// Handle open achievements action
        /// </summary>
        private void HandleOpenAchievements()
        {
            Debug.Log("🏆 [NotificationHandling] Opening achievements");

            // Example: Load achievements screen
            this.LoadScene("Achievements");
        }

        /// <summary>
        /// Handle open event action
        /// </summary>
        private void HandleOpenEvent(string eventId)
        {
            Debug.Log($"🎪 [NotificationHandling] Opening event: {eventId}");

            // Example: Load event screen với event cụ thể
            PlayerPrefs.SetString("active_event", eventId);
            this.LoadScene("Events");
        }

        /// <summary>
        /// Handle open profile action
        /// </summary>
        private void HandleOpenProfile()
        {
            Debug.Log("👤 [NotificationHandling] Opening profile");

            // Example: Load profile screen
            this.LoadScene("Profile");
        }

        /// <summary>
        /// Handle start level action (immediately start gameplay)
        /// </summary>
        private void HandleStartLevel(string levelId)
        {
            Debug.Log($"▶️ [NotificationHandling] Starting level: {levelId}");

            // Example: Load gameplay với level được specified
            PlayerPrefs.SetString("auto_start_level", levelId);
            this.LoadScene("Gameplay");
        }

        /// <summary>
        /// Handle custom action
        /// </summary>
        private void HandleCustomAction(string actionId, string parameters)
        {
            Debug.Log($"⚙️ [NotificationHandling] Custom action: {actionId}");
            Debug.Log($"   Parameters: {parameters}");

            // Example: Parse parameters và execute custom logic
            switch (actionId)
            {
                case "special_offer":
                    this.HandleSpecialOfferAsync(parameters).Forget();
                    break;

                case "tournament_join":
                    this.HandleTournamentJoin(parameters);
                    break;

                case "friend_request":
                    this.HandleFriendRequest(parameters);
                    break;

                default:
                    Debug.LogWarning($"⚠️ Unknown custom action: {actionId}");
                    break;
            }
        }

        // ============================================
        // Helper Methods
        // ============================================

        /// <summary>
        /// Load scene helper
        /// </summary>
        private void LoadScene(string sceneName)
        {
            Debug.Log($"📂 [NotificationHandling] Loading scene: {sceneName}");

            try
            {
                SceneManager.LoadScene(sceneName);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"❌ [NotificationHandling] Error loading scene: {ex.Message}");
            }
        }

        /// <summary>
        /// Claim reward async
        /// </summary>
        private async UniTask ClaimRewardAsync(string rewardId)
        {
            Debug.Log($"⏳ [NotificationHandling] Processing reward claim...");

            // Simulate API call hoặc game logic
            await UniTask.Delay(1000);

            // Grant reward to player
            // Example: Add coins, gems, items, etc.
            Debug.Log($"✅ [NotificationHandling] Reward claimed: {rewardId}");

            // Show reward popup
            // Example: ShowRewardPopup(rewardId);
        }

        /// <summary>
        /// Handle special offer custom action
        /// </summary>
        private async UniTask HandleSpecialOfferAsync(string parameters)
        {
            Debug.Log($"💰 [NotificationHandling] Processing special offer...");

            // Parse parameters (có thể là JSON)
            // Example: { "offerId": "123", "discount": 50, "expiry": "2024-12-31" }

            await UniTask.Delay(500);

            // Navigate đến shop với offer pre-selected
            PlayerPrefs.SetString("active_offer", parameters);
            this.LoadScene("Shop");
        }

        /// <summary>
        /// Handle tournament join
        /// </summary>
        private void HandleTournamentJoin(string parameters)
        {
            Debug.Log($"🏆 [NotificationHandling] Joining tournament...");

            // Parse tournament info
            PlayerPrefs.SetString("join_tournament", parameters);
            this.LoadScene("Tournament");
        }

        /// <summary>
        /// Handle friend request
        /// </summary>
        private void HandleFriendRequest(string parameters)
        {
            Debug.Log($"👥 [NotificationHandling] Processing friend request...");

            // Show friend request popup hoặc navigate to friends screen
            PlayerPrefs.SetString("pending_friend_request", parameters);
            this.LoadScene("Friends");
        }

        // ============================================
        // PUBLIC: Example Methods để Schedule Notifications với Actions
        // ============================================

        /// <summary>
        /// PUBLIC: Schedule notification với action mở level
        /// </summary>
        public async void ScheduleOpenLevelNotification(string levelId, long fireTimeInSeconds)
        {
            if (!this.notificationManager.HasPermission)
            {
                Debug.LogWarning("⚠️ Không có permission!");
                return;
            }

            // Tạo action
            var action = NotificationAction.CreateOpenLevelAction(levelId);

            // Tạo notification
            var notification = new NotificationData(
                title: $"🎮 Level {levelId} Awaits!",
                body: "Ready to continue your adventure?",
                fireTimeInSeconds: fireTimeInSeconds
            );

            // Attach action vào notification
            notification.customData = action.ToJson();

            // Schedule
            var notificationId = await this.notificationManager.ScheduleNotificationAsync(notification);

            Debug.Log($"✅ Scheduled level notification with action: #{notificationId}");
        }

        /// <summary>
        /// PUBLIC: Schedule notification với action claim reward
        /// </summary>
        public async void ScheduleClaimRewardNotification(string rewardId, long fireTimeInSeconds)
        {
            if (!this.notificationManager.HasPermission)
            {
                return;
            }

            var action = NotificationAction.CreateClaimRewardAction(rewardId);

            var notification = new NotificationData(
                title: "🎁 Reward Available!",
                body: "Tap to claim your reward now!",
                fireTimeInSeconds: fireTimeInSeconds
            );

            notification.customData = action.ToJson();

            var notificationId = await this.notificationManager.ScheduleNotificationAsync(notification);

            Debug.Log($"✅ Scheduled reward notification: #{notificationId}");
        }

        /// <summary>
        /// PUBLIC: Schedule notification với custom action
        /// </summary>
        public async void ScheduleCustomActionNotification(
            string actionId,
            string parameters,
            string title,
            string body,
            long fireTimeInSeconds)
        {
            if (!this.notificationManager.HasPermission)
            {
                return;
            }

            var action = NotificationAction.CreateCustomAction(actionId, parameters);

            var notification = new NotificationData(title, body, fireTimeInSeconds);
            notification.customData = action.ToJson();

            var notificationId = await this.notificationManager.ScheduleNotificationAsync(notification);

            Debug.Log($"✅ Scheduled custom action notification: #{notificationId}");
        }
    }
}

