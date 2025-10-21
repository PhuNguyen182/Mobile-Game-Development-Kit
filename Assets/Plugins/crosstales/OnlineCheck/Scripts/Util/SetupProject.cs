using UnityEngine;

namespace Crosstales.OnlineCheck.Util
{
   /// <summary>Setup the project to use OnlineCheck.</summary>
#if UNITY_EDITOR
   [UnityEditor.InitializeOnLoadAttribute]
#endif
   public class SetupProject
   {
      #region Constructor

      static SetupProject()
      {
         setup();
      }

      #endregion


      #region Public methods

      [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
      private static void setup()
      {
         Crosstales.Common.Util.Singleton<OnlineCheck>.PrefabPath = "Prefabs/OnlineCheck";
         Crosstales.Common.Util.Singleton<OnlineCheck>.GameObjectName = "OnlineCheck";
      }

      #endregion
   }
}
// © 2020-2024 crosstales LLC (https://www.crosstales.com)