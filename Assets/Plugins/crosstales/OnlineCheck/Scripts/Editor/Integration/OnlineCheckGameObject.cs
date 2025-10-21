#if UNITY_EDITOR
using UnityEditor;
using Crosstales.OnlineCheck.EditorUtil;
using Crosstales.OnlineCheck.Util;

namespace Crosstales.OnlineCheck.EditorIntegration
{
   /// <summary>Editor component for the "Hierarchy"-menu.</summary>
   public static class OnlineCheckGameObject
   {
      [MenuItem("GameObject/" + Constants.ASSET_NAME + "/" + Constants.ONLINECHECK_SCENE_OBJECT_NAME, false, EditorHelper.GO_ID)]
      private static void AddOnlineCheck()
      {
         EditorHelper.InstantiatePrefab(Constants.ONLINECHECK_SCENE_OBJECT_NAME);
      }

      [MenuItem("GameObject/" + Constants.ASSET_NAME + "/" + Constants.ONLINECHECK_SCENE_OBJECT_NAME, true)]
      private static bool AddOnlineCheckValidator()
      {
         return !EditorHelper.isOnlineCheckInScene;
      }
   }
}
#endif
// © 2017-2024 crosstales LLC (https://www.crosstales.com)