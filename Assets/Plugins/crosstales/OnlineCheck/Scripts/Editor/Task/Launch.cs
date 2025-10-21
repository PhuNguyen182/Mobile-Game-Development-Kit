#if UNITY_EDITOR && !UNITY_CLOUD_BUILD
using System.Linq;
using UnityEditor;

namespace Crosstales.OnlineCheck.EditorTask
{
   /// <summary>Show the configuration window on the first launch.</summary>
   public class Launch : AssetPostprocessor
   {
      public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
      {
         if (importedAssets.Any(str => str.Contains(Crosstales.OnlineCheck.EditorUtil.EditorConstants.ASSET_UID.ToString())))
         {
            Crosstales.Common.EditorTask.SetupResources.Setup();
            SetupResources.Setup();

            Crosstales.OnlineCheck.EditorIntegration.ConfigWindow.ShowWindow(4);
         }
      }
   }
}
#endif
// © 2017-2024 crosstales LLC (https://www.crosstales.com)