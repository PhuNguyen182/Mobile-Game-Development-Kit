#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Crosstales.OnlineCheck.Data;

namespace Crosstales.OnlineCheck.EditorExtension
{
   /// <summary>Custom editor for the 'CustomCheck'-class.</summary>
   [UnityEditor.CustomEditor(typeof(CustomCheck))]
   public class CustomCheckEditor : Editor
   {
      private CustomCheck script;

      private void OnEnable()
      {
         script = (CustomCheck)target;
      }

      public override void OnInspectorGUI()
      {
         DrawDefaultInspector();

         if (!Crosstales.Common.Util.NetworkHelper.isURL(script.URL))
            EditorGUILayout.HelpBox("The 'URL' is empty or invalid! Please add a valid URL.", MessageType.Warning);

         if (string.IsNullOrEmpty(script.ExpectedData))
            EditorGUILayout.HelpBox("The 'Expected Data' is empty! Please add an expected result string.", MessageType.Warning);

         if (GUI.changed)
         {
            //Debug.Log("Changed");
            EditorUtility.SetDirty(script);
            AssetDatabase.SaveAssets();
         }
      }
   }
}
#endif
// © 2018-2024 crosstales LLC (https://www.crosstales.com)