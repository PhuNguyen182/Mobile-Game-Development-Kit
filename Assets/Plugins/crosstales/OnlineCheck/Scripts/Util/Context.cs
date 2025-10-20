namespace Crosstales.OnlineCheck.Util
{
   /// <summary>Context for the asset.</summary>
   public static class Context
   {
      #region Changable variables

      /// <summary>The current total number of checks.</summary>
      public static int NumberOfChecks = 0;

      /// <summary>Time since start of the scene.</summary>
      public static float Runtime = 0f;

      /// <summary>The current total of Internet availability uptime.</summary>
      public static float Uptime = 0f;

      #endregion


      #region Properties

      /// <summary>Calculates checks per minute.</summary>
      /// <returns>Returns checks done within 60 seconds</returns>
      public static float ChecksPerMinute => NumberOfChecks / (Runtime / 60f);

      /// <summary>Calculates Internet unavailability.</summary>
      /// <returns>Returns downtime in seconds.</returns>
      public static float Downtime => Runtime - Uptime;

      #endregion
   }
}
// © 2017-2024 crosstales LLC (https://www.crosstales.com)