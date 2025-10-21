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
}