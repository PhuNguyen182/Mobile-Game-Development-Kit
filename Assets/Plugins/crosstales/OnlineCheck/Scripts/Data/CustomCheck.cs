using UnityEngine;
using Crosstales.OnlineCheck.Util;

namespace Crosstales.OnlineCheck.Data
{
   /// <summary>Data definition of a custom check.</summary>
   [System.Serializable]
   [HelpURL("https://www.crosstales.com/media/data/assets/OnlineCheck/api/class_crosstales_1_1_online_check_1_1_data_1_1_custom_check.html")]
   [CreateAssetMenu(fileName = "New CustomCheck", menuName = Constants.ASSET_NAME + "/CustomCheck", order = 1000)]
   public class CustomCheck : ScriptableObject
   {
      #region Variables

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("URL"), SerializeField, Tooltip("Custom URL to perform the Internet availability tests e.g. https://mydomain.com/connect.txt. The host should be https-based and provide an 'Access-Control-Allow-Origin' header.")]
      private string url = string.Empty;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("ExpectedData"), SerializeField, Tooltip("Expected data from the custom URL (as string).")]
      private string expectedData = "success";

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("DataMustBeEquals"), SerializeField, Tooltip("Compares the custom data with 'equals' to the expected data (default: false, false uses 'contains' as match).")]
      private bool dataMustBeEquals;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("UseOnlyCustom"), SerializeField, Tooltip("Use only the custom url for Internet availability tests and ignores all built-in checks (default: false).")]
      private bool useOnlyCustom;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("ShowErrors"), SerializeField, Tooltip("Displays all connection errors (default: false).")]
      private bool showErrors;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("HeaderSize"), SerializeField, Tooltip("Size of the request header (default: 0).")]
      private int headerSize;

      #endregion


      #region Properties

      /// <summary>Custom URL to perform the Internet availability tests e.g. https://mydomain.com/connect.txt. The host should be https-based and provide an 'Access-Control-Allow-Origin' header.</summary>
      public string URL
      {
         get => url;
         set => url = value;
      }

      /// <summary>Expected data from the custom URL (as string).</summary>
      public string ExpectedData
      {
         get => expectedData;
         set => expectedData = value;
      }

      /// <summary>Compares the custom data with 'equals' to the expected data.</summary>
      public bool DataMustBeEquals
      {
         get => dataMustBeEquals;
         set => dataMustBeEquals = value;
      }

      /// <summary>Use only the custom url for Internet availability tests and ignores all built-in checks.</summary>
      public bool UseOnlyCustom
      {
         get => useOnlyCustom;
         set => useOnlyCustom = value;
      }

      /// <summary>Displays all connection errors.</summary>
      public bool ShowErrors
      {
         get => showErrors;
         set => showErrors = value;
      }

      /// <summary>Size of the request header.</summary>
      public int HeaderSize
      {
         get => headerSize;
         set
         {
            if (value < 0)
            {
               headerSize = 0;
            }
            else
            {
               headerSize = value;
            }
         }
      }

      #endregion


      #region ScriptableObject methods

      private void OnValidate()
      {
         if (headerSize < 0)
            headerSize = 0;

//#if UNITY_EDITOR && UNITY_2019_1_OR_NEWER
//         UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
//#endif
      }

      #endregion


      #region Overridden methods

      public override string ToString()
      {
         System.Text.StringBuilder result = new System.Text.StringBuilder();

         result.Append(GetType().Name);
         result.Append(Constants.TEXT_TOSTRING_START);

         result.Append("URL='");
         result.Append(url);
         result.Append(Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("ExpectedData='");
         result.Append(expectedData);
         result.Append(Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("DataMustBeEquals='");
         result.Append(dataMustBeEquals);
         result.Append(Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("UseOnlyCustom='");
         result.Append(useOnlyCustom);
         result.Append(Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("ShowErrors='");
         result.Append(showErrors);
         result.Append(Constants.TEXT_TOSTRING_DELIMITER_END);

         result.Append(Constants.TEXT_TOSTRING_END);

         return result.ToString();
      }

      public override bool Equals(object obj)
      {
         if (obj == null || GetType() != obj.GetType())
            return false;

         CustomCheck o = (CustomCheck)obj;

         return url == o.url &&
                expectedData == o.expectedData &&
                dataMustBeEquals == o.dataMustBeEquals &&
                useOnlyCustom == o.useOnlyCustom &&
                showErrors == o.showErrors;
      }

      public override int GetHashCode()
      {
         return base.GetHashCode();
      }
/*      
      public override int GetHashCode()
      {
         int hash = 0;

         if (url != null)
            hash += url.GetHashCode();
         if (expectedData != null)
            hash += expectedData.GetHashCode();

         return hash;
      }
*/
      #endregion
   }
}
// © 2018-2024 crosstales LLC (https://www.crosstales.com)