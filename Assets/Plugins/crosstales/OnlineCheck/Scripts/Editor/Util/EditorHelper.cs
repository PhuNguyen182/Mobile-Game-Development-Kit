#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Crosstales.OnlineCheck.Util;

namespace Crosstales.OnlineCheck.EditorUtil
{
   /// <summary>Editor helper class.</summary>
   public abstract class EditorHelper : Crosstales.Common.EditorUtil.BaseEditorHelper
   {
      #region Static variables

      /// <summary>Start index inside the "GameObject"-menu.</summary>
      public const int GO_ID = 29;

      /// <summary>Start index inside the "Tools"-menu.</summary>
      public const int MENU_ID = 11415; // 1, O = 14, N = 15

      private static Texture2D logo_asset;
      private static Texture2D logo_asset_small;

      #endregion


      #region Static properties

      public static Texture2D Logo_Asset => loadImage(ref logo_asset, "logo_asset_pro.png");

      public static Texture2D Logo_Asset_Small => loadImage(ref logo_asset_small, "logo_asset_small_pro.png");

      #endregion


      #region Static methods

      /// <summary>Shows an "Online Check unavailable"-UI.</summary>
      public static void OCUnavailable()
      {
         EditorGUILayout.HelpBox("Online Check not available!", MessageType.Warning);

         EditorGUILayout.HelpBox($"Did you add the '{Constants.ONLINECHECK_SCENE_OBJECT_NAME}'-prefab to the scene?", MessageType.Info);

         GUILayout.Space(8);

         if (GUILayout.Button(new GUIContent($"Add {Constants.ONLINECHECK_SCENE_OBJECT_NAME}", Icon_Plus, $"Add the '{Constants.ONLINECHECK_SCENE_OBJECT_NAME}'-prefab to the current scene.")))
         {
            InstantiatePrefab(Constants.ONLINECHECK_SCENE_OBJECT_NAME);
         }
      }

      /// <summary>Instantiates a prefab.</summary>
      /// <param name="prefabName">Name of the prefab.</param>
      public static void InstantiatePrefab(string prefabName)
      {
         InstantiatePrefab(prefabName, Crosstales.OnlineCheck.EditorUtil.EditorConfig.PREFAB_PATH);
      }

      /// <summary>Checks if the 'OnlineCheck'-prefab is in the scene.</summary>
      /// <returns>True if the 'OnlineCheck'-prefab is in the scene.</returns>
#if UNITY_2023_1_OR_NEWER
      public static bool isOnlineCheckInScene => GameObject.FindFirstObjectByType<OnlineCheck>() != null;
#else
      public static bool isOnlineCheckInScene => GameObject.FindObjectOfType(typeof(OnlineCheck)) != null; //GameObject.Find(Constants.ONLINECHECK_SCENE_OBJECT_NAME) != null;
#endif

      /// <summary>Checks if the 'Proxy'-prefab is in the scene.</summary>
      /// <returns>True if the 'Proxy'-prefab is in the scene.</returns>
#if UNITY_2023_1_OR_NEWER
      public static bool isProxyInScene => GameObject.FindFirstObjectByType<Tool.Proxy>() != null;
#else
      public static bool isProxyInScene => GameObject.FindObjectOfType(typeof(Tool.Proxy)) != null; //GameObject.Find(Constants.PROXY_SCENE_OBJECT_NAME) != null;
#endif

      /// <summary>Checks if the 'PingCheck'-prefab is in the scene.</summary>
      /// <returns>True if the 'PingCheck'-prefab is in the scene.</returns>
      public static bool isPingInScene => GameObject.Find(Constants.PINGCHECK_SCENE_OBJECT_NAME) != null; //GameObject.FindObjectOfType(typeof(Tool.PingCheck.PingCheck)) != null;

      /// <summary>Checks if the 'SpeedTest'-prefab is in the scene.</summary>
      /// <returns>True if the 'SpeedTest'-prefab is in the scene.</returns>
      public static bool isSpeedTestInScene => GameObject.Find(Constants.SPEEDTEST_SCENE_OBJECT_NAME) != null; //GameObject.FindObjectOfType(typeof(Tool.SpeedTest)) != null;

      /// <summary>Checks if the 'SpeedTestNET'-prefab is in the scene.</summary>
      /// <returns>True if the 'SpeedTestNET'-prefab is in the scene.</returns>
      public static bool isSpeedTestNETInScene => GameObject.Find(Constants.SPEEDTESTNET_SCENE_OBJECT_NAME) != null; //GameObject.FindObjectOfType(typeof(Tool.SpeedTestNET.SpeedTestNET)) != null;

      /// <summary>Loads an image as Texture2D from 'Editor Default Resources'.</summary>
      /// <param name="logo">Logo to load.</param>
      /// <param name="fileName">Name of the image.</param>
      /// <returns>Image as Texture2D from 'Editor Default Resources'.</returns>
      private static Texture2D loadImage(ref Texture2D logo, string fileName)
      {
         if (logo == null)
         {
#if CT_DEVELOP
            logo = (Texture2D)AssetDatabase.LoadAssetAtPath($"Assets{Crosstales.OnlineCheck.EditorUtil.EditorConfig.ASSET_PATH}Icons/{fileName}", typeof(Texture2D));
#else
                logo = (Texture2D)EditorGUIUtility.Load($"crosstales/OnlineCheck/{fileName}");
#endif

            if (logo == null)
            {
               Debug.LogWarning($"Image not found: {fileName}");
            }
         }

         return logo;
      }

      #endregion
   }
}
#endif
// © 2017-2024 crosstales LLC (https://www.crosstales.com)