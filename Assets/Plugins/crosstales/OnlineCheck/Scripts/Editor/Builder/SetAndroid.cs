#if UNITY_EDITOR && (UNITY_ANDROID || CT_DEVELOP)
using UnityEditor;
using UnityEngine;

namespace Crosstales.OnlineCheck.EditorBuild
{
   /// <summary>Sets the required build parameters for Android.</summary>
   [InitializeOnLoad]
   public static class SetAndroid
   {
      #region Constructor

      static SetAndroid()
      {
         if (!PlayerSettings.Android.forceInternetPermission)
         {
            PlayerSettings.Android.forceInternetPermission = true;

            Debug.Log("Android: 'forceInternetPermission' set to true");
         }
      }

      #endregion
   }
}
#endif
// © 2017-2024 crosstales LLC (https://www.crosstales.com)