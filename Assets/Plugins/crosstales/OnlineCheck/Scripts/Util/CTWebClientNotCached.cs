#if (!UNITY_WSA && !UNITY_XBOXONE) || UNITY_EDITOR
namespace Crosstales.OnlineCheck.Util
{
   /// <summary>Specialized WebClient.</summary>
   public class CTWebClientNotCached : Crosstales.Common.Util.CTWebClient
   {
      #region Constructors

      public CTWebClientNotCached() : base(5000)
      {
      }

      public CTWebClientNotCached(int timeout, int connectionLimit = 20) : base(timeout, connectionLimit)
      {
      }

      #endregion


      #region Overriden methods

      protected override System.Net.WebRequest GetWebRequest(System.Uri uri)
      {
         System.Net.WebRequest request = base.GetWebRequest(uri);
         if (request != null)
         {
            request.Timeout = Timeout;

            // disable caching
            System.Net.Cache.HttpRequestCachePolicy noCachePolicy = new System.Net.Cache.HttpRequestCachePolicy(System.Net.Cache.HttpRequestCacheLevel.NoCacheNoStore);
            request.CachePolicy = noCachePolicy;
         }


         return request;
      }

      #endregion
   }
}
#endif
// © 2017-2024 crosstales LLC (https://www.crosstales.com)