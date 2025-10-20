namespace Crosstales.OnlineCheck.Model
{
   /// <summary>Model for a network interface.</summary>
   [System.Serializable]
   public class NetworkInterface
   {
#if (!UNITY_WSA && !UNITY_XBOXONE) || UNITY_EDITOR

      #region Variables

      /// <summary>Id of the network interface.</summary>
      public readonly string Id;

      /// <summary>Name of the network interface.</summary>
      public readonly string Name;

      /// <summary>Type of the network interface.</summary>
      public readonly System.Net.NetworkInformation.NetworkInterfaceType Type;

      /// <summary>Address of the network interface.</summary>
      public readonly System.Net.IPAddress Address;

      /// <summary>Mask of the network interface.</summary>
      public readonly System.Net.IPAddress Mask;

      /// <summary>MAC address of the network interface.</summary>
      public readonly string MacAddress;

      /// <summary>Gateway of the network interface.</summary>
      public readonly System.Net.IPAddress Gateway;

      /// <summary>Speed of the network interface in bits-per-second (bps).</summary>
      public readonly long Speed;

      /// <summary>Status of the network interface.</summary>
      public readonly System.Net.NetworkInformation.OperationalStatus Status;

      #endregion


      #region Constructor

      public NetworkInterface(string id, string name, System.Net.NetworkInformation.NetworkInterfaceType type,
         System.Net.IPAddress address, System.Net.IPAddress mask, string macAddress, System.Net.IPAddress gateway,
         long speed, System.Net.NetworkInformation.OperationalStatus status)
      {
         Id = id;
         Name = name;
         Type = type;
         Address = address;
         Mask = mask;
         MacAddress = macAddress;
         Gateway = gateway;
         Speed = speed;
         Status = status;
      }

      #endregion


      #region Overridden methods

      public override string ToString()
      {
         System.Text.StringBuilder result = new System.Text.StringBuilder();

         result.Append("• ");
         result.Append(Name);

         result.Append(" (");
         result.Append(Type);
         result.Append("), ");

         result.Append("Address: ");
         result.Append(Address);

         if (Status == System.Net.NetworkInformation.OperationalStatus.Up)
         {
            result.Append(" (");
            result.Append(Mask);
            result.Append("), ");
         }
         else
         {
            result.Append(", ");
         }

         result.Append("Mac: ");
         result.Append(MacAddress);
         result.Append(", ");

         if (Status == System.Net.NetworkInformation.OperationalStatus.Up)
         {
            result.Append("Gateway: ");
            result.Append(Gateway);
            result.Append(", ");

            result.Append("Speed: ");
            result.Append(Speed / 1000000);
            result.Append(" Mbps, ");
         }

         result.Append("Status: ");
         result.Append(Status);

         return result.ToString();
      }

/*
        public override string ToString()
        {
            System.Text.StringBuilder result = new System.Text.StringBuilder();

            result.Append(GetType().Name);
            result.Append(Util.Constants.TEXT_TOSTRING_START);

            result.Append("Id='");
            result.Append(Id);
            result.Append(Util.Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("Name='");
            result.Append(Name);
            result.Append(Util.Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("Type='");
            result.Append(Type);
            result.Append(Util.Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("Address='");
            result.Append(Address);
            result.Append(Util.Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("Mask='");
            result.Append(Mask);
            result.Append(Util.Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("MacAddress='");
            result.Append(MacAddress);
            result.Append(Util.Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("Gateway='");
            result.Append(Gateway);
            result.Append(Util.Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("Speed='");
            result.Append(Speed);
            result.Append(Util.Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("Status='");
            result.Append(Status);
            result.Append(Util.Constants.TEXT_TOSTRING_DELIMITER_END);

            result.Append(Util.Constants.TEXT_TOSTRING_END);

            return result.ToString();
        }
*/

      #endregion

#endif
   }
}
// © 2020-2024 crosstales LLC (https://www.crosstales.com)