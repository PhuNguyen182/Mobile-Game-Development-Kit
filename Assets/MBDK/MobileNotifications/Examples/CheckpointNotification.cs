using System;
using Cysharp.Threading.Tasks;
using MBDK.MobileNotifications.Core;
using MBDK.MobileNotifications.Data;
using MBDK.MobileNotifications.Interfaces;
using UnityEngine;

namespace MBDK.MobileNotifications.Examples
{
    /// <summary>
    /// Example về cách sử dụng Checkpoint-based Notifications
    /// </summary>
    /// <remarks>
    /// Script này minh họa cách schedule notifications dựa vào game checkpoints
    /// như level complete, achievement unlock, hoặc tutorial complete.
    /// </remarks>
    public class CheckpointNotification : MonoBehaviour
    {
        [Header("Notification Manager")]
        [SerializeField]
        [Tooltip("Reference đến MobileNotificationManager trong scene")]
        private MobileNotificationManager notificationManager;

        [Header("Checkpoint Scenarios")]
        [SerializeField]
        [Tooltip("Scenario khi hoàn thành tutorial")]
        private NotificationScenario tutorialCompleteScenario;

        [SerializeField]
        [Tooltip("Scenario khi hoàn thành level")]
        private NotificationScenario levelCompleteScenario;

        [SerializeField]
        [Tooltip("Scenario khi unlock achievement")]
        private NotificationScenario achievementUnlockScenario;

        // Service để quản lý checkpoints
        private IMobileNotificationService notificationService;

        /// <summary>
        /// Unity Start lifecycle
        /// </summary>
        private async void Start()
        {
            // Wait for manager initialization
            await UniTask.WaitUntil(() => this.notificationManager != null && this.notificationManager.IsInitialized);

            Debug.Log("🎮 [CheckpointNotification] Checkpoint notification system ready!");
        }

        /// <summary>
        /// PUBLIC: Mark tutorial complete checkpoint
        /// Gọi khi người chơi hoàn thành tutorial
        /// </summary>
        public async void OnTutorialCompleted()
        {
            Debug.Log("🎓 [CheckpointNotification] Tutorial completed!");

            // Update checkpoint timestamp
            var checkpointName = "tutorial_complete";
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // Giả sử chúng ta có access đến service thông qua manager
            // (Trong production, có thể expose service qua manager hoặc inject dependency)
            
            Debug.Log($"📍 [CheckpointNotification] Checkpoint marked: {checkpointName} at {timestamp}");

            // Schedule tutorial complete scenario
            if (this.tutorialCompleteScenario != null)
            {
                // Update checkpoint trong scenario
                this.tutorialCompleteScenario.checkpointName = checkpointName;
                this.tutorialCompleteScenario.useCheckpoint = true;

                var scheduledIds = await this.notificationManager.ScheduleScenarioAsync(this.tutorialCompleteScenario);
                Debug.Log($"✅ [CheckpointNotification] Tutorial scenario scheduled: {scheduledIds.Count} notifications");
            }
        }

        /// <summary>
        /// PUBLIC: Mark level complete checkpoint
        /// Gọi khi người chơi hoàn thành một level
        /// </summary>
        /// <param name="levelNumber">Số level đã hoàn thành</param>
        public async void OnLevelCompleted(int levelNumber)
        {
            Debug.Log($"🏆 [CheckpointNotification] Level {levelNumber} completed!");

            // Update checkpoint với level-specific name
            var checkpointName = $"level_{levelNumber}_complete";
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            Debug.Log($"📍 [CheckpointNotification] Checkpoint marked: {checkpointName} at {timestamp}");

            // Schedule level complete scenario
            if (this.levelCompleteScenario != null && this.notificationManager.HasPermission)
            {
                // Customize notifications dựa vào level
                var customScenario = this.CreateLevelCompleteScenario(levelNumber);

                var scheduledIds = await this.notificationManager.ScheduleScenarioAsync(customScenario);
                Debug.Log($"✅ [CheckpointNotification] Level scenario scheduled: {scheduledIds.Count} notifications");
            }
        }

        /// <summary>
        /// PUBLIC: Mark achievement unlock checkpoint
        /// Gọi khi người chơi unlock achievement
        /// </summary>
        /// <param name="achievementId">ID của achievement</param>
        /// <param name="achievementName">Tên achievement</param>
        public async void OnAchievementUnlocked(string achievementId, string achievementName)
        {
            Debug.Log($"🏅 [CheckpointNotification] Achievement unlocked: {achievementName}");

            // Update checkpoint
            var checkpointName = $"achievement_{achievementId}";
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            Debug.Log($"📍 [CheckpointNotification] Checkpoint marked: {checkpointName} at {timestamp}");

            // Schedule achievement scenario
            if (this.achievementUnlockScenario != null && this.notificationManager.HasPermission)
            {
                // Customize notifications dựa vào achievement
                var customScenario = this.CreateAchievementScenario(achievementId, achievementName);

                var scheduledIds = await this.notificationManager.ScheduleScenarioAsync(customScenario);
                Debug.Log($"✅ [CheckpointNotification] Achievement scenario scheduled: {scheduledIds.Count} notifications");
            }
        }

        /// <summary>
        /// PUBLIC: Schedule reminder dựa vào checkpoint cụ thể
        /// </summary>
        /// <param name="checkpointName">Tên checkpoint</param>
        /// <param name="delayInSeconds">Delay từ checkpoint (seconds)</param>
        /// <param name="title">Tiêu đề notification</param>
        /// <param name="body">Nội dung notification</param>
        public async UniTask ScheduleCheckpointReminder(
            string checkpointName, 
            long delayInSeconds, 
            string title, 
            string body)
        {
            if (!this.notificationManager.HasPermission)
            {
                Debug.LogWarning("⚠️ [CheckpointNotification] Không có permission!");
                return;
            }

            Debug.Log($"⏰ [CheckpointNotification] Scheduling reminder from checkpoint: {checkpointName}");

            // Tạo notification với checkpoint timing
            var notification = new NotificationData(title, body, delayInSeconds);
            notification.customData = $"checkpoint:{checkpointName}";

            var notificationId = await this.notificationManager.ScheduleNotificationAsync(notification);

            if (notificationId > 0)
            {
                Debug.Log($"✅ [CheckpointNotification] Reminder scheduled: #{notificationId}");
            }
        }

        /// <summary>
        /// Tạo custom level complete scenario
        /// </summary>
        private NotificationScenario CreateLevelCompleteScenario(int levelNumber)
        {
            var scenario = ScriptableObject.CreateInstance<NotificationScenario>();
            scenario.scenarioName = $"Level {levelNumber} Complete";
            scenario.description = $"Notifications sau khi hoàn thành level {levelNumber}";
            scenario.cancelPreviousOnSchedule = true;
            scenario.groupKey = "level_complete";
            scenario.useCheckpoint = true;
            scenario.checkpointName = $"level_{levelNumber}_complete";

            // Notification 1: Comeback sau 1 giờ
            var comeback1h = new NotificationData(
                title: $"🎮 Continue Your Journey!",
                body: $"You're doing great! Ready for level {levelNumber + 1}?",
                fireTimeInSeconds: 60 * 60 // 1 giờ
            );
            comeback1h.customData = $"level:{levelNumber + 1}";
            scenario.AddNotification(comeback1h);

            // Notification 2: Reminder sau 1 ngày
            var reminder1d = new NotificationData(
                title: "🌟 Your Adventure Awaits!",
                body: "Don't forget about your progress! Jump back in!",
                fireTimeInSeconds: 60 * 60 * 24 // 1 ngày
            );
            reminder1d.customData = $"level:{levelNumber + 1}";
            scenario.AddNotification(reminder1d);

            // Notification 3: Special offer sau 3 ngày
            var offer3d = new NotificationData(
                title: "🎁 Special Offer!",
                body: "We miss you! Get a special bonus when you return!",
                fireTimeInSeconds: 60 * 60 * 24 * 3 // 3 ngày
            );
            offer3d.customData = "special_offer";
            scenario.AddNotification(offer3d);

            return scenario;
        }

        /// <summary>
        /// Tạo custom achievement scenario
        /// </summary>
        private NotificationScenario CreateAchievementScenario(string achievementId, string achievementName)
        {
            var scenario = ScriptableObject.CreateInstance<NotificationScenario>();
            scenario.scenarioName = $"Achievement: {achievementName}";
            scenario.description = $"Notifications sau khi unlock achievement {achievementName}";
            scenario.cancelPreviousOnSchedule = false; // Không cancel achievements khác
            scenario.groupKey = "achievements";
            scenario.useCheckpoint = true;
            scenario.checkpointName = $"achievement_{achievementId}";

            // Notification 1: Congratulations
            var congrats = new NotificationData(
                title: "🎊 Congratulations!",
                body: $"You've unlocked: {achievementName}! Keep going!",
                fireTimeInSeconds: 60 * 5 // 5 phút
            );
            congrats.customData = $"achievement:{achievementId}";
            scenario.AddNotification(congrats);

            // Notification 2: More achievements to unlock
            var moreAchievements = new NotificationData(
                title: "🏆 More Achievements Await!",
                body: "There are more achievements to unlock. Can you get them all?",
                fireTimeInSeconds: 60 * 60 * 6 // 6 giờ
            );
            moreAchievements.customData = "achievements_list";
            scenario.AddNotification(moreAchievements);

            return scenario;
        }

        /// <summary>
        /// PUBLIC: Example - Schedule reminders khi người chơi offline
        /// Gọi khi người chơi thoát game hoặc app vào background
        /// </summary>
        public async UniTask OnPlayerWentOffline()
        {
            if (!this.notificationManager.HasPermission)
            {
                return;
            }

            Debug.Log("👋 [CheckpointNotification] Player went offline, scheduling reminders...");

            // Mark checkpoint
            var checkpointName = "player_offline";
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // Schedule một series reminders

            // 30 phút sau
            await this.ScheduleCheckpointReminder(
                checkpointName,
                60 * 30,
                "⚡ Energy Refilled!",
                "Your energy is full! Come back and play!"
            );

            // 6 giờ sau
            await this.ScheduleCheckpointReminder(
                checkpointName,
                60 * 60 * 6,
                "🎮 We Miss You!",
                "Your friends are waiting! Jump back in!"
            );

            // 1 ngày sau
            await this.ScheduleCheckpointReminder(
                checkpointName,
                60 * 60 * 24,
                "🎁 Daily Reward Available!",
                "Don't miss your daily reward! Login now!"
            );

            Debug.Log("✅ [CheckpointNotification] Offline reminders scheduled");
        }
    }
}

