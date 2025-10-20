#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Crosstales.OnlineCheck.Tool;

namespace Crosstales.OnlineCheck.EditorExtension
{
   /// <summary>Custom editor for the 'Proxy'-class.</summary>
   [CustomEditor(typeof(Proxy))]
   public class ProxyEditor : Editor
   {
      #region Variables

      private Proxy script;

      private string httpProxyURL;
      private int httpProxyPort;
      private string httpProxyUsername;
      private string httpProxyPassword;
      private string httpProxyProtocol;

      private string httpsProxyURL;
      private int httpsProxyPort;
      private string httpsProxyUsername;
      private string httpsProxyPassword;
      private string httpsProxyProtocol;

      private bool enableOnAwake;

      #endregion


      #region Editor methods

      private void OnEnable()
      {
         script = (Proxy)target;
      }

      public override void OnInspectorGUI()
      {
         serializedObject.Update();

         GUILayout.Label("HTTP Settings", EditorStyles.boldLabel);

         httpProxyURL = EditorGUILayout.TextField(new GUIContent("URL", "URL (without protocol) or IP of the HTTP proxy server."), script.HTTPProxyURL);
         if (!httpProxyURL.Equals(script.HTTPProxyURL))
         {
            serializedObject.FindProperty("HTTPProxyURL").stringValue = httpProxyURL;
            serializedObject.ApplyModifiedProperties();
         }

         httpProxyPort = EditorGUILayout.IntSlider("Port", script.HTTPProxyPort, 0, 65535);
         if (httpProxyPort != script.HTTPProxyPort)
         {
            serializedObject.FindProperty("HTTPProxyPort").intValue = httpProxyPort;
            serializedObject.ApplyModifiedProperties();
         }

         httpProxyUsername = EditorGUILayout.TextField(new GUIContent("User", "Username for the HTTP proxy server (optional)."), script.HTTPProxyUsername);
         if (!httpProxyUsername.Equals(script.HTTPProxyUsername))
         {
            serializedObject.FindProperty("HTTPProxyUsername").stringValue = httpProxyUsername;
            serializedObject.ApplyModifiedProperties();
         }

         httpProxyPassword = EditorGUILayout.PasswordField(new GUIContent("Password", "User password for the HTTP proxy server (optional)."), script.HTTPProxyPassword);
         if (!httpProxyPassword.Equals(script.HTTPProxyPassword))
         {
            serializedObject.FindProperty("HTTPProxyPassword").stringValue = httpProxyPassword;
            serializedObject.ApplyModifiedProperties();
         }

         httpProxyProtocol = EditorGUILayout.TextField(new GUIContent("Protocol", "Protocol (e.g. 'http://') for the HTTP proxy server (optional)."), script.HTTPProxyURLProtocol);
         if (!httpProxyProtocol.Equals(script.HTTPProxyURLProtocol))
         {
            serializedObject.FindProperty("HTTPProxyURLProtocol").stringValue = httpProxyProtocol;
            serializedObject.ApplyModifiedProperties();
         }

         GUILayout.Space(8);
         GUILayout.Label("HTTPS Settings", EditorStyles.boldLabel);

         httpsProxyURL = EditorGUILayout.TextField(new GUIContent("URL", "URL (without protocol) or IP of the HTTPS proxy server."), script.HTTPSProxyURL);
         if (!httpsProxyURL.Equals(script.HTTPSProxyURL))
         {
            serializedObject.FindProperty("HTTPSProxyURL").stringValue = httpsProxyURL;
            serializedObject.ApplyModifiedProperties();
         }

         httpsProxyPort = EditorGUILayout.IntSlider("Port", script.HTTPSProxyPort, 0, 65535);
         if (httpsProxyPort != script.HTTPSProxyPort)
         {
            serializedObject.FindProperty("HTTPSProxyPort").intValue = httpsProxyPort;
            serializedObject.ApplyModifiedProperties();
         }

         httpsProxyUsername = EditorGUILayout.TextField(new GUIContent("User", "Username for the HTTPS proxy server (optional)."), script.HTTPSProxyUsername);
         if (!httpsProxyUsername.Equals(script.HTTPSProxyUsername))
         {
            serializedObject.FindProperty("HTTPSProxyUsername").stringValue = httpsProxyUsername;
            serializedObject.ApplyModifiedProperties();
         }

         httpsProxyPassword = EditorGUILayout.PasswordField(new GUIContent("Password", "User password for the HTTPS proxy server (optional)."), script.HTTPSProxyPassword);
         if (!httpsProxyPassword.Equals(script.HTTPSProxyPassword))
         {
            serializedObject.FindProperty("HTTPSProxyPassword").stringValue = httpsProxyPassword;
            serializedObject.ApplyModifiedProperties();
         }

         httpsProxyProtocol = EditorGUILayout.TextField(new GUIContent("Protocol", "Protocol (e.g. 'https://') for the HTTPS proxy server (optional)."), script.HTTPSProxyURLProtocol);
         if (!httpsProxyProtocol.Equals(script.HTTPSProxyURLProtocol))
         {
            serializedObject.FindProperty("HTTPSProxyURLProtocol").stringValue = httpsProxyProtocol;
            serializedObject.ApplyModifiedProperties();
         }

         GUILayout.Space(8);
         GUILayout.Label("Behaviour Settings", EditorStyles.boldLabel);

         enableOnAwake = EditorGUILayout.Toggle(new GUIContent("Enable On Awake", "Enable the proxy server on awake (default: false)."), script.EnableOnAwake);
         if (enableOnAwake != script.EnableOnAwake)
         {
            serializedObject.FindProperty("EnableOnAwake").boolValue = enableOnAwake;
            serializedObject.ApplyModifiedProperties();
         }

         if (string.IsNullOrEmpty(script.HTTPProxyURL) && string.IsNullOrEmpty(script.HTTPSProxyURL))
            EditorGUILayout.HelpBox("'URL' is empty - proxy pass through not possible!", MessageType.Warning);

         Crosstales.OnlineCheck.EditorUtil.EditorHelper.SeparatorUI();

         if (script.isActiveAndEnabled)
         {
            if (Crosstales.OnlineCheck.Util.Helper.isEditorMode)
            {
               GUILayout.Label("HTTP-Proxy:", EditorStyles.boldLabel);

               if (Proxy.hasHTTPProxy)
               {
                  if (GUILayout.Button(new GUIContent(" Disable", Crosstales.OnlineCheck.EditorUtil.EditorHelper.Icon_Minus, "Disable HTTP-Proxy.")))
                  {
                     Proxy.DisableHTTPProxy();
                  }
               }
               else
               {
                  if (GUILayout.Button(new GUIContent(" Enable", Crosstales.OnlineCheck.EditorUtil.EditorHelper.Icon_Plus, "Enable HTTP-Proxy.")))
                  {
                     script.EnableHTTPProxy();
                  }
               }

               GUILayout.Space(8);

               GUILayout.Label("HTTPS-Proxy:", EditorStyles.boldLabel);

               if (Proxy.hasHTTPSProxy)
               {
                  if (GUILayout.Button(new GUIContent(" Disable", Crosstales.OnlineCheck.EditorUtil.EditorHelper.Icon_Minus, "Disable HTTPS-Proxy.")))
                  {
                     Proxy.DisableHTTPSProxy();
                  }
               }
               else
               {
                  if (GUILayout.Button(new GUIContent(" Enable", Crosstales.OnlineCheck.EditorUtil.EditorHelper.Icon_Plus, "Enable HTTPS-Proxy.")))
                  {
                     script.EnableHTTPSProxy();
                  }
               }
            }
            else
            {
               EditorGUILayout.HelpBox("Disabled in Play-mode!", MessageType.Info);
            }
         }
         else
         {
            EditorGUILayout.HelpBox("Script is disabled!", MessageType.Info);
         }
      }

      #endregion
   }
}
#endif
// © 2017-2024 crosstales LLC (https://www.crosstales.com)