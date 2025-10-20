using System.Linq;
using UnityEngine;
using Crosstales.OnlineCheck.Model;

namespace Crosstales.OnlineCheck.Util
{
   /// <summary>Provides extra information about the network environment.</summary>
   public static class NetworkInfo
   {
      #region Variables

      private static System.Collections.Generic.List<NetworkInterface> interfaceCache;
      private static string publicIpCache;

      #endregion


      #region Properties

      /// <summary>Returns the public IP of the Internet connection.</summary>
      /// <returns>Public IP of the Internet connection.</returns>
      public static string PublicIP
      {
         get
         {
#if (!UNITY_WEBGL && !UNITY_WSA && !UNITY_XBOXONE) || UNITY_EDITOR
            System.Net.ServicePointManager.ServerCertificateValidationCallback = Crosstales.Common.Util.NetworkHelper.RemoteCertificateValidationCallback;

            try
            {
               //string url = "https://ipinfo.io/ip";
               //string url = "https://icanhazip.com";
               const string url = "https://checkip.amazonaws.com/";

               using (System.Net.WebClient client = new Crosstales.Common.Util.CTWebClient())
               {
                  string content = client.DownloadString(url);

                  if (Constants.DEV_DEBUG)
                     Debug.LogWarning($"Content: {content}");

                  return content.Trim();
               }
            }
            catch (System.Exception ex)
            {
               Debug.LogWarning($"Could not determine the public IP: {ex}");
            }
#else
			   Debug.LogWarning("'NetworkInfo' is not supported under the current platform!");
#endif
            return "unknown";
         }
      }

      /// <summary>Returns the last list of network interfaces.</summary>
      /// <returns>Last list of network interfaces.</returns>
      public static System.Collections.Generic.List<NetworkInterface> LastNetworkInterfaces
      {
         get
         {
#if (!UNITY_WEBGL && !UNITY_WSA && !UNITY_XBOXONE) || UNITY_EDITOR
            if (interfaceCache == null)
               getNetworkInterfaces();
#else
			   Debug.LogWarning("'NetworkInfo' is not supported under the current platform!");
#endif
            return interfaceCache;
         }
      }

      /// <summary>Returns the last public IP.</summary>
      /// <returns>Last public IP.</returns>
      public static string LastPublicIP => publicIpCache ?? (publicIpCache = PublicIP);

      #endregion


      #region Public methods

      /// <summary>Refresh the network information.</summary>
      public static void Refresh()
      {
         interfaceCache = null;
         publicIpCache = null;
      }

      /// <summary>Returns a list of all available network interfaces.</summary>
      /// <param name="activeOnly">Search only for active network interfaces (optional)</param>
      /// <returns>List of network interfaces.</returns>
      public static System.Collections.Generic.List<NetworkInterface> getNetworkInterfaces(bool activeOnly = true)
      {
         System.Collections.Generic.List<NetworkInterface> interfaces = new System.Collections.Generic.List<NetworkInterface>();

#if (!UNITY_WEBGL && !UNITY_WSA && !UNITY_XBOXONE) || UNITY_EDITOR
         System.Net.NetworkInformation.NetworkInterface[] adapters;

         if (activeOnly)
         {
            adapters = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces().Where(ni =>
               ni.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up).ToArray();
         }
         else
         {
            adapters = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
         }

         foreach (System.Net.NetworkInformation.NetworkInterface adapter in adapters)
         {
            System.Net.NetworkInformation.PhysicalAddress physicalAddress = adapter.GetPhysicalAddress();

            string macAddress = string.Join(":", physicalAddress.GetAddressBytes().Select(delegate(byte val)
            {
               string sign = val.ToString("X");
               if (sign.Length == 1)
                  sign = $"0{sign}";

               return sign;
            }).ToArray());

            System.Net.NetworkInformation.IPInterfaceProperties ipInterfaceProperties = adapter.GetIPProperties();
            System.Net.NetworkInformation.UnicastIPAddressInformation unicastAddressIP =
               ipInterfaceProperties.UnicastAddresses.FirstOrDefault(ua =>
                  ua.Address?.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

            System.Net.IPAddress gateway = ipInterfaceProperties.GatewayAddresses.Select(g => g.Address)
               .FirstOrDefault(a => a != null);

            if (unicastAddressIP != null)
            {
               interfaces.Add(new Model.NetworkInterface(adapter.Id, adapter.Name, adapter.NetworkInterfaceType,
                  unicastAddressIP.Address, unicastAddressIP.IPv4Mask, macAddress, gateway, adapter.Speed,
                  adapter.OperationalStatus));
            }
         }

         interfaceCache = interfaces;
#else
			Debug.LogWarning("'NetworkInfo' is not supported under the current platform!");
#endif
         return interfaces;
      }

      /// <summary>Indicates if NetworkInfo is supporting the current platform.</summary>
      /// <returns>True if NetworkInfo supports current platform.</returns>
      public static bool isPlatformSupported => !Helper.isWebPlatform && !Helper.isWSABasedPlatform;

      #endregion
   }
}
// © 2020-2024 crosstales LLC (https://www.crosstales.com)