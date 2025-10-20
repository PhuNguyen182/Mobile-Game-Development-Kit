#if UNITY_EDITOR
using UnityEditor;
using Crosstales.OnlineCheck.Util;

namespace Crosstales.OnlineCheck.EditorTask
{
   /// <summary>Loads the configuration at startup.</summary>
   [InitializeOnLoad]
   public static class AAAConfigLoader
   {
      #region Constructor

      static AAAConfigLoader()
      {
         if (!Config.isLoaded)
         {
            Config.Load();

            if (Config.DEBUG)
               UnityEngine.Debug.Log("Config data loaded");
         }
      }

      #endregion
   }
}
#endif
// © 2017-2024 crosstales LLC (https://www.crosstales.com)