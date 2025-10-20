#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Crosstales.OnlineCheck.EditorUtil;
using Crosstales.OnlineCheck.Util;

namespace Crosstales.OnlineCheck.EditorExtension
{
   /// <summary>Custom editor for the 'OnlineCheck'-class.</summary>
   [InitializeOnLoad]
   [CustomEditor(typeof(OnlineCheck))]
   public class OnlineCheckEditor : Editor
   {
      #region Variables

      private OnlineCheck script;

      private static bool showNE;
      private static bool showChecks;

      #endregion


      #region Static constructor

      static OnlineCheckEditor()
      {
         EditorApplication.hierarchyWindowItemOnGUI += hierarchyItemCB;
      }

      #endregion


      #region Editor methods

      private void OnEnable()
      {
         script = (OnlineCheck)target;
         EditorApplication.update += onUpdate;
         //onUpdate();
      }

      private void OnDisable()
      {
         EditorApplication.update -= onUpdate;
      }

      public override void OnInspectorGUI()
      {
         DrawDefaultInspector();

         if (!script.Microsoft && !script.Google204 && !script.GoogleBlank && !script.Apple && !script.Ubuntu && script.CustomCheck == null)
            EditorGUILayout.HelpBox("No check selected - please select at least one or add a custom check!", MessageType.Warning);

         if (script.CustomCheck != null && !string.IsNullOrEmpty(script.CustomCheck.URL) && (script.CustomCheck.URL.CTContains("crosstales.com") || script.CustomCheck.URL.Contains("207.154.226.218")))
            EditorGUILayout.HelpBox($"'Custom Check' uses 'crosstales.com' for detection: this is only allowed for test-builds and the check interval will be limited!{System.Environment.NewLine}Please use your own URL for detection.", MessageType.Warning);

         EditorHelper.SeparatorUI();

         if (script.isActiveAndEnabled)
         {
            EditorStyles.foldout.fontStyle = FontStyle.Bold;
            showNE = EditorGUILayout.Foldout(showNE, "Network Environment");
            EditorStyles.foldout.fontStyle = FontStyle.Normal;

            if (showNE)
            {
               EditorGUI.indentLevel++;
               //onUpdate();

               /*
               GUILayout.BeginHorizontal();
               {
                   GUILayout.Label("Internet Available:");
                   GUI.enabled = false;
                   EditorGUILayout.Toggle(new GUIContent(string.Empty, "Is Internet currently available?"), script.isInternetAvailable);
                   GUI.enabled = true;
               }
               GUILayout.EndHorizontal();
               */

               GUILayout.Label($"Internet Available:\t{(script.isInternetAvailable ? "Yes" : "No")}");

               GUILayout.Label($"Reachability:{Constants.TAB}{script.NetworkReachabilityShort}");
               GUILayout.Label($"Public IP:\t\t{NetworkInfo.LastPublicIP}");
               GUILayout.Label("Active Interfaces:");
               GUILayout.Label(NetworkInfo.LastNetworkInterfaces.CTDump());

               EditorGUI.indentLevel--;
            }

            EditorHelper.SeparatorUI();


            EditorStyles.foldout.fontStyle = FontStyle.Bold;
            showChecks = EditorGUILayout.Foldout(showChecks, "Checks");
            EditorStyles.foldout.fontStyle = FontStyle.Normal;

            if (showChecks)
            {
               EditorGUI.indentLevel++;

               GUILayout.Label($"Last checked:{Constants.TAB}{script.LastCheck}");
               GUILayout.Label($"Total:{Constants.TAB}\t{Context.NumberOfChecks}");

               if (!Helper.isEditorMode)
               {
                  GUILayout.Label($"Checks/Minute:{Constants.TAB}{Context.ChecksPerMinute:#0.0}");
                  GUILayout.Label($"Data Downloaded:\t{Helper.FormatBytesToHRF(script.DataDownloaded)}");
                  EditorHelper.SeparatorUI();

                  GUILayout.Label("Timers", EditorStyles.boldLabel);
                  GUILayout.Label($"Runtime:\t\t{Helper.FormatSecondsToHRF(Context.Runtime)}");
                  GUILayout.Label($"Uptime:{Constants.TAB}\t{Helper.FormatSecondsToHRF(Context.Uptime)}");
                  GUILayout.Label($"Downtime:\t\t{Helper.FormatSecondsToHRF(Context.Downtime)}");
               }

               if (Helper.isEditorMode)
               {
                  GUI.enabled = !script.isBusy;

                  if (GUILayout.Button(new GUIContent(" Refresh", EditorHelper.Icon_Reset, "Restart the Internet availability check.")))
                  {
                     script.Refresh();
                     NetworkInfo.Refresh();
                  }

                  GUI.enabled = true;
               }

               EditorGUI.indentLevel--;
            }
         }
         else
         {
            EditorGUILayout.HelpBox("Script is disabled!", MessageType.Info);
         }
      }

      #endregion


      #region Private methods

      private void onUpdate()
      {
         Repaint();
      }

      private static void hierarchyItemCB(int instanceID, Rect selectionRect)
      {
         if (EditorConfig.HIERARCHY_ICON)
         {
            GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (go != null && go.GetComponent<OnlineCheck>())
            {
               Rect r = new Rect(selectionRect);
               r.x = r.width - 4;

               GUI.Label(r, EditorHelper.Logo_Asset_Small);
            }
         }
      }

      #endregion
   }
}
#endif
// © 2017-2024 crosstales LLC (https://www.crosstales.com)