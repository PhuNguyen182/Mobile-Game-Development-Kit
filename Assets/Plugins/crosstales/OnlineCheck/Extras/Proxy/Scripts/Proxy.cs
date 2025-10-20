using UnityEngine;
using Crosstales.OnlineCheck.Util;

namespace Crosstales.OnlineCheck.Tool
{
   /// <summary>Handles HTTP/HTTPS Internet connections via proxy server.</summary>
   [ExecuteInEditMode]
   [DisallowMultipleComponent]
   [HelpURL("https://crosstales.com/media/data/assets/OnlineCheck/api/class_crosstales_1_1_online_check_1_1_tool_1_1_proxy.html")]
   public class Proxy : MonoBehaviour
   {
      #region Variables

      /// <summary>URL (without protocol) or IP of the proxy server.</summary>
      [Header("HTTP Proxy Settings")] [Tooltip("URL (without protocol) or IP of the proxy server.")] public string HTTPProxyURL;

      /// <summary>Port of the proxy server.</summary>
      [Tooltip("Port of the proxy server.")] [Range(0, 65535)] public int HTTPProxyPort = 8080;

      /// <summary>Username for the proxy server (optional).</summary>
      [Tooltip("Username for the proxy server (optional).")] public string HTTPProxyUsername = string.Empty;

      /// <summary>Password for the proxy server (optional).</summary>
      [Tooltip("Password for the proxy server (optional).")] public string HTTPProxyPassword = string.Empty;

      /// <summary>Protocol (e.g. 'http://') for the proxy server (optional).</summary>
      [Tooltip("Protocol (e.g. 'http://') for the proxy server (optional).")] public string HTTPProxyURLProtocol = string.Empty;

      /// <summary>URL (without protocol) or IP of the proxy server.</summary>
      [Header("HTTPS Proxy Settings")] [Tooltip("URL (without protocol) or IP of the proxy server.")] public string HTTPSProxyURL;

      /// <summary>Port of the proxy server.</summary>
      [Tooltip("Port of the proxy server.")] [Range(0, 65535)] public int HTTPSProxyPort = 8443;

      /// <summary>Username for the proxy server (optional).</summary>
      [Tooltip("Username for the proxy server (optional).")] public string HTTPSProxyUsername = string.Empty;

      /// <summary>Password for the proxy server (optional).</summary>
      [Tooltip("Password for the proxy server (optional).")] public string HTTPSProxyPassword = string.Empty;

      /// <summary>Protocol (e.g. 'http://') for the proxy server (optional).</summary>
      [Tooltip("Protocol (e.g. 'http://') for the proxy server (optional).")] public string HTTPSProxyURLProtocol = string.Empty;


      /// <summary>Enable the proxy on awake (default: false).</summary>
      [Header("Startup Behaviour")] [Tooltip("Enable the proxy on awake (default: false).")] public bool EnableOnAwake;

      private const string HTTPProxyEnvVar = "HTTP_PROXY";
      private const string HTTPSProxyEnvVar = "HTTPS_PROXY";

      #endregion


      #region Static constructor

      static Proxy()
      {
         hasHTTPSProxy = false;
         hasHTTPProxy = false;
      }

      #endregion


      #region Properties

      /// <summary>Is HTTP-proxy enabled?</summary>
      /// <returns>True if the HTTP-proxy is enabled.</returns>
      public static bool hasHTTPProxy { get; private set; }

      /// <summary>Is HTTPS-proxy enabled?</summary>
      /// <returns>True if the HTTPS-proxy is enabled.</returns>
      public static bool hasHTTPSProxy { get; private set; }

      #endregion


      #region MonoBehaviour methods

      private void Awake()
      {
#if (!UNITY_WEBGL && !UNITY_WSA && !UNITY_XBOXONE) || UNITY_EDITOR
         if (EnableOnAwake)
         {
            EnableHTTPProxy();
            EnableHTTPSProxy();
         }
#else
			Debug.LogWarning("'Proxy' is not supported under the current platform!", this);
#endif
      }

      private void Update()
      {
         if (Helper.isEditorMode)
            name = Constants.PROXY_SCENE_OBJECT_NAME; //ensure name
      }

      #endregion


      #region Public methods

      /// <summary>Enables or disables a proxy server for HTTPS connections with the current instance variables as parameters.</summary>
      public void EnableHTTPProxy()
      {
         EnableHTTPProxy(HTTPProxyURL, HTTPProxyPort, HTTPProxyUsername, HTTPProxyPassword, HTTPProxyURLProtocol);
      }

      /// <summary>Enables or disables a proxy server for HTTPS connections with the current instance variables as parameters.</summary>
      public void EnableHTTPSProxy()
      {
         EnableHTTPSProxy(HTTPSProxyURL, HTTPSProxyPort, HTTPSProxyUsername, HTTPSProxyPassword, HTTPSProxyURLProtocol);
      }

      /// <summary>Enables or disables a proxy server for HTTP connections.</summary>
      /// <param name="url">URL (without protocol) or IP of the proxy server</param>
      /// <param name="port">Port of the proxy server</param>
      /// <param name="username">"Username for the proxy server (optional)</param>
      /// <param name="password">Password for the proxy server (optional)</param>
      /// <param name="urlProtocol">Protocol (e.g. 'http://') for the proxy server (optional)</param>
      public static void EnableHTTPProxy(string url, int port, string username = "", string password = "", string urlProtocol = "")
      {
         if (Helper.isEditor || Helper.isStandalonePlatform)
         {
            if (string.IsNullOrEmpty(url))
            {
               Debug.LogError("HTTP proxy url cannot be null or empty!");
               return;
            }

            if (!validPort(port))
            {
               Debug.LogError($"HTTP proxy port must be valid (between 0 - 65535): {port}");
               return;
            }

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
               System.Environment.SetEnvironmentVariable(HTTPProxyEnvVar, $"{urlProtocol}{username}:{password}@{url}:{port}");
            }
            else
            {
               System.Environment.SetEnvironmentVariable(HTTPProxyEnvVar, $"{urlProtocol}{url}:{port}");
            }

            hasHTTPProxy = true;
         }
         else
         {
            Debug.LogWarning("'Proxy' only works under Standalone!");
         }
      }

      /// <summary>Enables or disables a proxy server for HTTPS connections.</summary>
      /// <param name="url">URL (without protocol) or IP of the proxy server</param>
      /// <param name="port">Port of the proxy server</param>
      /// <param name="username">"Username for the proxy server (optional)</param>
      /// <param name="password">Password for the proxy server (optional)</param>
      /// <param name="urlProtocol">Protocol (e.g. 'http://') for the proxy server (optional)</param>
      public static void EnableHTTPSProxy(string url, int port, string username = "", string password = "", string urlProtocol = "")
      {
         if (Helper.isEditor || Helper.isStandalonePlatform)
         {
            if (string.IsNullOrEmpty(url))
            {
               Debug.LogError("HTTPS proxy url cannot be null or empty!");
               return;
            }

            if (!validPort(port))
            {
               Debug.LogError($"HTTPS proxy port must be valid (between 0 - 65535): {port}");
               return;
            }

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
               System.Environment.SetEnvironmentVariable(HTTPSProxyEnvVar, $"{urlProtocol}{username}:{password}@{url}:{port}");
            }
            else
            {
               System.Environment.SetEnvironmentVariable(HTTPSProxyEnvVar, $"{urlProtocol}{url}:{port}");
            }

            hasHTTPSProxy = true;
         }
         else
         {
            Debug.LogWarning("'Proxy' only works under Standalone!");
         }
      }

      /// <summary>Disables the proxy server for HTTP connections.</summary>
      public static void DisableHTTPProxy()
      {
         if (Helper.isEditor || Helper.isStandalonePlatform)
         {
            System.Environment.SetEnvironmentVariable(HTTPProxyEnvVar, string.Empty);

            hasHTTPProxy = false;
         }
         else
         {
            Debug.LogWarning("'Proxy' only works under Standalone!");
         }
      }

      /// <summary>Disables the proxy server for HTTPS connections.</summary>
      public static void DisableHTTPSProxy()
      {
         if (Helper.isEditor || Helper.isStandalonePlatform)
         {
            System.Environment.SetEnvironmentVariable(HTTPSProxyEnvVar, string.Empty);

            hasHTTPSProxy = false;
         }
         else
         {
            Debug.LogWarning("'Proxy' only works under Standalone!");
         }
      }

      #endregion


      #region Private methods

      private static bool validPort(int port)
      {
         return port >= 0 && port <= 65535;
      }

      #endregion
   }
}
// © 2016-2024 crosstales LLC (https://www.crosstales.com)