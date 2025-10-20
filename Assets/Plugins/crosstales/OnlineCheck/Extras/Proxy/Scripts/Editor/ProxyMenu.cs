#if UNITY_EDITOR
using UnityEditor;
using Crosstales.OnlineCheck.EditorUtil;
using Crosstales.OnlineCheck.Util;

namespace Crosstales.OnlineCheck.EditorIntegration
{
   /// <summary>Editor component for the "Tools"-menu.</summary>
   public static class ProxyMenu
   {
      [MenuItem("Tools/" + Constants.ASSET_NAME + "/Prefabs/" + Constants.PROXY_SCENE_OBJECT_NAME, false, EditorHelper.MENU_ID + 110)]
      private static void AddProxy()
      {
         EditorHelper.InstantiatePrefab(Constants.PROXY_SCENE_OBJECT_NAME, $"{EditorConfig.ASSET_PATH}Extras/Proxy/Resources/Prefabs/");
      }

      [MenuItem("Tools/" + Constants.ASSET_NAME + "/Prefabs/" + Constants.PROXY_SCENE_OBJECT_NAME, true)]
      private static bool AddProxyValidator()
      {
         return !EditorHelper.isProxyInScene;
      }
   }
}
#endif
// © 2021-2024 crosstales LLC (https://www.crosstales.com)