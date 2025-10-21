#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Crosstales.OnlineCheck.EditorUtil
{
   /// <summary>Editor configuration for the asset.</summary>
   [InitializeOnLoad]
   public static class EditorConfig
   {
      #region Variables

      /// <summary>Enable or disable update-checks for the asset.</summary>
      public static bool UPDATE_CHECK = Crosstales.OnlineCheck.EditorUtil.EditorConstants.DEFAULT_UPDATE_CHECK;

      /// <summary>Enable or disable adding compile define "CT_OC" for the asset.</summary>
      public static bool COMPILE_DEFINES = Crosstales.OnlineCheck.EditorUtil.EditorConstants.DEFAULT_COMPILE_DEFINES;

      /// <summary>Enable or disable the icon in the hierarchy.</summary>
      public static bool HIERARCHY_ICON = Crosstales.OnlineCheck.EditorUtil.EditorConstants.DEFAULT_HIERARCHY_ICON;

      /// <summary>Is the configuration loaded?</summary>
      public static bool isLoaded;

      private static string assetPath;
      private const string idPath = "Documentation/id/";
      private static readonly string idName = $"{Crosstales.OnlineCheck.EditorUtil.EditorConstants.ASSET_UID}.txt";

      #endregion


      #region Constructor

      static EditorConfig()
      {
         if (!isLoaded)
         {
            Load();
         }
      }

      #endregion


      #region Properties

      /// <summary>Returns the path to the asset inside the Unity project.</summary>
      /// <returns>The path to the asset inside the Unity project.</returns>
      public static string ASSET_PATH
      {
         get
         {
            if (assetPath == null)
            {
               try
               {
                  if (Crosstales.Common.Util.FileHelper.ExistsFile(Application.dataPath + Crosstales.OnlineCheck.EditorUtil.EditorConstants.DEFAULT_ASSET_PATH + idPath + idName))
                  {
                     assetPath = Crosstales.OnlineCheck.EditorUtil.EditorConstants.DEFAULT_ASSET_PATH;
                  }
                  else
                  {
                     string[] files = System.IO.Directory.GetFiles(Application.dataPath, idName, System.IO.SearchOption.AllDirectories);

                     if (files.Length > 0)
                     {
                        string name = files[0].Substring(Application.dataPath.Length);
                        assetPath = name.Substring(0, name.Length - idPath.Length - idName.Length).Replace("\\", "/");
                     }
                     else
                     {
                        Debug.LogWarning($"Could not locate the asset! File not found: {idName}");
                        assetPath = Crosstales.OnlineCheck.EditorUtil.EditorConstants.DEFAULT_ASSET_PATH;
                     }
                  }
               }
               catch (System.Exception ex)
               {
                  Debug.LogWarning($"Could not locate asset: {ex}");
               }
            }

            return assetPath;
         }
      }

      /// <summary>Returns the path of the prefabs.</summary>
      /// <returns>The path of the prefabs.</returns>
      public static string PREFAB_PATH => ASSET_PATH + Crosstales.OnlineCheck.EditorUtil.EditorConstants.PREFAB_SUBPATH;

      #endregion


      #region Public static methods

      /// <summary>Resets all changeable variables to their default value.</summary>
      public static void Reset()
      {
         assetPath = null;

         UPDATE_CHECK = Crosstales.OnlineCheck.EditorUtil.EditorConstants.DEFAULT_UPDATE_CHECK;
         COMPILE_DEFINES = Crosstales.OnlineCheck.EditorUtil.EditorConstants.DEFAULT_COMPILE_DEFINES;
         HIERARCHY_ICON = Crosstales.OnlineCheck.EditorUtil.EditorConstants.DEFAULT_HIERARCHY_ICON;
      }

      /// <summary>Loads the all changeable variables.</summary>
      public static void Load()
      {
         assetPath = null;

         if (Crosstales.Common.Util.CTPlayerPrefs.HasKey(Crosstales.OnlineCheck.EditorUtil.EditorConstants.KEY_UPDATE_CHECK))
            UPDATE_CHECK = Crosstales.Common.Util.CTPlayerPrefs.GetBool(Crosstales.OnlineCheck.EditorUtil.EditorConstants.KEY_UPDATE_CHECK);

         if (Crosstales.Common.Util.CTPlayerPrefs.HasKey(Crosstales.OnlineCheck.EditorUtil.EditorConstants.KEY_COMPILE_DEFINES))
            COMPILE_DEFINES = Crosstales.Common.Util.CTPlayerPrefs.GetBool(Crosstales.OnlineCheck.EditorUtil.EditorConstants.KEY_COMPILE_DEFINES);

         if (Crosstales.Common.Util.CTPlayerPrefs.HasKey(Crosstales.OnlineCheck.EditorUtil.EditorConstants.KEY_HIERARCHY_ICON))
            HIERARCHY_ICON = Crosstales.Common.Util.CTPlayerPrefs.GetBool(Crosstales.OnlineCheck.EditorUtil.EditorConstants.KEY_HIERARCHY_ICON);

         isLoaded = true;
      }

      /// <summary>Saves the all changeable variables.</summary>
      public static void Save()
      {
         Crosstales.Common.Util.CTPlayerPrefs.SetBool(Crosstales.OnlineCheck.EditorUtil.EditorConstants.KEY_UPDATE_CHECK, UPDATE_CHECK);
         Crosstales.Common.Util.CTPlayerPrefs.SetBool(Crosstales.OnlineCheck.EditorUtil.EditorConstants.KEY_COMPILE_DEFINES, COMPILE_DEFINES);
         Crosstales.Common.Util.CTPlayerPrefs.SetBool(Crosstales.OnlineCheck.EditorUtil.EditorConstants.KEY_HIERARCHY_ICON, HIERARCHY_ICON);

         Crosstales.Common.Util.CTPlayerPrefs.Save();
      }

      #endregion
   }
}
#endif
// © 2017-2024 crosstales LLC (https://www.crosstales.com)