using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Crosstales.OnlineCheck.Util;
using Crosstales.OnlineCheck.Data;

namespace Crosstales.OnlineCheck
{
   [System.Serializable]
   public class StatusChangeEvent : UnityEngine.Events.UnityEvent<bool>
   {
   }

   /// <summary>Checks the Internet availability.</summary>
   [ExecuteInEditMode]
   [DisallowMultipleComponent]
   [HelpURL("https://crosstales.com/media/data/assets/OnlineCheck/api/class_crosstales_1_1_online_check_1_1_online_check.html")]
   public class OnlineCheck : Crosstales.Common.Util.Singleton<OnlineCheck>
   {
      #region Variables

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("EndlessMode")] [Header("General Settings")] [Tooltip("Continuously check for Internet availability within given intervals (default: true)."), SerializeField]
      private bool endlessMode = true;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("IntervalMin")] [Tooltip("Minimum delay between checks in seconds (default: 8, range: 3 - 59)."), Range(3, 59), SerializeField]
      private int intervalMin = 8;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("IntervalMax")] [Tooltip("Maximum delay between checks in seconds (default: 12, range: 4 - 60)."), Range(4, 60), SerializeField]
      private int intervalMax = 12;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("Timeout")] [Tooltip("Timeout for every check in seconds (default: 2, range: 1 - 10)."), Range(1, 10), SerializeField]
      private int timeout = 2;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("ForceWWW")] [Tooltip("Force UnityWebRequest instead of WebClient (default: false)."), SerializeField]
      private bool forceWWW;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("CustomCheck")] [Tooltip("Use a custom configuration for the checks."), SerializeField] [ContextMenuItem("Create CustomCheck", "createCustomCheck")]
      private CustomCheck customCheck;

      [Header("Active Checks")] [Tooltip("Enable or disable the 'Microsoft NCSI' check (default: true, 184 Bytes)."), SerializeField]
      private bool microsoft = true;

      [Tooltip("Enable or disable the 'Google 204' check (default: true, 279 Bytes)."), SerializeField] private bool google204 = true;

      [Tooltip("Enable or disable the 'Google Blank' check (default: true, 831 Bytes)."), SerializeField] private bool googleBlank = true;

      [Tooltip("Enable or disable the 'Apple' check (default: true, 503 Bytes)."), SerializeField] private bool apple = true;

      [Tooltip("Enable or disable the 'Ubuntu' check (default: true, 1001 Bytes)."), SerializeField] private bool ubuntu = true;
      //[Tooltip("Enable or disable the 'Fallback' check (default: true, 366 Bytes)."), SerializeField] private bool fallback = true;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("RunOnStart")] [Header("Behaviour Settings"), Tooltip("Start at runtime (default: true)."), SerializeField]
      private bool runOnStart = true;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("Delay")] [Tooltip("Delay in seconds until the OnlineCheck starts checking (default: 0, range: 0 - 120)."), Range(0, 120), SerializeField]
      private float delay;

      private bool isFirsttime = true;
      private bool lastInternetAvailable;
      private NetworkReachability lastNetworkReachability;

      private bool isRunning;

      private float checkTime = 9999f;
      private float checkTimeCounter;

      private float burstTime = 9999f;
      private float burstTimeCounter;
      private bool available;

      private NetworkReachability networkReachability;

      //private long lastCheckTime = long.MinValue;
      private float lastCheckTime = float.MinValue;

      private const float burstIntervalMin = 2f;
      private const float burstIntervalMax = 5f;

      private const string microsoftUrl = "http://www.msftncsi.com/ncsi.txt";
      private const string appleUrl = "https://www.apple.com/library/test/success.html";
      private const string ubuntuUrl = "https://start.ubuntu.com/connectivity-check";
      private const string fallbackUrl = "https://crosstales.com/media/downloads/up.txt";

      private const int google204HeaderSize = 279;
      private const int googleBlankHeaderSize = 831;
      private const int microsoftHeaderSize = 170;
      private const int appleHeaderSize = 376;
      private const int ubuntuHeaderSize = 425;
      private const int fallbackHeaderSize = 349;

      private const string microsoftText = "Microsoft NCSI";
      private const string appleText = "<TITLE>Success</TITLE>";
      private const string ubuntuText = "<TITLE>Lorem Ipsum</TITLE>";
      private const string fallbackText = "crosstales rulez!";

      private const string windowsDesc = "Microsoft";
      private const string appleDesc = "Apple";
      private const string ubuntuDesc = "Ubuntu";
      private const string fallbackDesc = "Fallback (crosstales)";
      private const string customDesc = "custom URL";
      private const string testingDesc = "Testing the Internet availability with:";

      private const bool microsoftEquals = true;
      private const bool appleEquals = false;
      private const bool ubuntuEquals = false;
      private const bool fallbackEquals = true;

#if (!UNITY_WSA && !UNITY_XBOXONE) || UNITY_EDITOR
      private System.Threading.Thread worker;
#endif

      private bool existsCustomCheck;

      #endregion


      #region Properties

      /// <summary>Continuously check for Internet availability within given intervals.</summary>
      public bool EndlessMode
      {
         get => endlessMode;

         set
         {
            endlessMode = value;

            if (endlessMode)
               checkTimeCounter = checkTime;
         }
      }

      /// <summary>Minimum delay between checks in seconds (range: 3 - 59).</summary>
      public int IntervalMin
      {
         get
         {
            if (intervalMin > intervalMax)
               intervalMin = intervalMax - 1;

            return intervalMin;
         }

         set
         {
            int number = Mathf.Clamp(value, 3, 59);
            intervalMin = number < intervalMax ? number : intervalMax - 1;
         }
      }

      /// <summary>Maximum delay between checks in seconds (range: 4 - 60).</summary>
      public int IntervalMax
      {
         get
         {
            if (intervalMax < intervalMin)
               intervalMax = intervalMin + 1;

            return intervalMax;
         }

         set
         {
            int number = Mathf.Clamp(value, 4, 60);
            intervalMax = number > intervalMin ? number : intervalMin + 1;
         }
      }

      /// <summary>Timeout for every check in seconds (range: 1 - 10).</summary>
      public int Timeout
      {
         get
         {
            if (timeout >= intervalMin)
               timeout = intervalMin - 1;

            return timeout;
         }

         set
         {
            int number = Mathf.Clamp(value, 1, 10);
            timeout = number < intervalMin ? number : intervalMin - 1;
         }
      }

      /// <summary>Force UnityWebRequest instead of WebClient.</summary>
      public bool ForceWWW
      {
         get
         {
            if (Helper.isWebPlatform || Helper.isWSABasedPlatform)
               return true;

            return forceWWW;
         }

         set => forceWWW = value;
      }

      /// <summary>Use a custom configuration for the checks.</summary>
      public CustomCheck CustomCheck
      {
         get => customCheck;

         set
         {
            existsCustomCheck = value != null;
            customCheck = value;
         }
      }

      /// <summary>Enable or disable the 'Google 204' check (279 Bytes).</summary>
      public bool Google204
      {
         get => google204;
         set => google204 = value;
      }

      /// <summary>Enable or disable the 'Google Blank' check (831 Bytes).</summary>
      public bool GoogleBlank
      {
         get => googleBlank;
         set => googleBlank = value;
      }

      /// <summary>Enable or disable the 'Microsoft' check (184 Bytes).</summary>
      public bool Microsoft
      {
         get => microsoft;
         set => microsoft = value;
      }

      /// <summary>Enable or disable the 'Apple' check (??? Bytes).</summary>
      public bool Apple
      {
         get => apple;
         set => apple = value;
      }

      /// <summary>Enable or disable the 'Ubuntu' check (1001 Bytes).</summary>
      public bool Ubuntu
      {
         get => ubuntu;
         set => ubuntu = value;
      }

/*
      /// <summary>Enable or disable the 'Fallback' check (366 Bytes).</summary>
      public bool Fallback
      {
         get => fallback;
         set => fallback = value;
      }
*/
      /// <summary>Start at runtime.</summary>
      public bool RunOnStart
      {
         get => runOnStart;
         set => runOnStart = value;
      }

      /// <summary>Delay in seconds until the OnlineCheck starts checking.</summary>
      public float Delay
      {
         get => delay;
         set => delay = value;
      }

      /// <summary>Returns true if an Internet connection is available.</summary>
      /// <returns>True if an Internet connection is available.</returns>
      public bool isInternetAvailable { get; private set; }

      /// <summary>Returns the network reachability.</summary>
      /// <returns>The Internet reachability.</returns>
      public NetworkReachability NetworkReachability => networkReachability;

      /// <summary>Returns the network reachability in shorter form.</summary>
      /// <returns>The Internet reachability in shorter form.</returns>
      public string NetworkReachabilityShort
      {
         get
         {
            switch (NetworkReachability)
            {
               case NetworkReachability.ReachableViaCarrierDataNetwork:
                  return "Carrier";
               case NetworkReachability.ReachableViaLocalAreaNetwork:
                  return "LAN";
               default:
                  return "None";
            }
         }
      }

      /// <summary>Returns the time of the last availability check.</summary>
      /// <returns>Time of the last availability check.</returns>
      public System.DateTime LastCheck { get; private set; }

      /// <summary>Returns the total download size in bytes for the current session.</summary>
      /// <returns>Download size in bytes.</returns>
      public long DataDownloaded { get; private set; }

      /// <summary>Returns true if OnlineCheck is busy.</summary>
      /// <returns>True if if OnlineCheck is busy.</returns>
      public bool isBusy => isRunning;

      /// <summary>
      /// Returns the round trip time of the last successful availability check in seconds.
      /// Note: This value is only accurate if used with "ForceWWW" disabled.
      /// </summary>
      /// <returns>Round trip time of the last successful availability check in seconds.</returns>
      public float LastCheckRTT => LastCheckRTTMilliseconds / 1000f;

      /// <summary>
      /// Returns the round trip time of the last successful availability check in milliseconds.
      /// Note: This value is only accurate if used with "ForceWWW" disabled.
      /// </summary>
      /// <returns>Round trip time of the last successful availability check in milliseconds.</returns>
      public int LastCheckRTTMilliseconds { get; private set; }

      #endregion


      #region Events

      [Header("Events")] public StatusChangeEvent OnStatusChange;

      /// <summary>Callback to determine whether the online status has changed or not.</summary>
      public delegate void OnlineStatusChange(bool isConnected);

      /// <summary>Callback to determine whether the network reachability has changed or not.</summary>
      public delegate void NetworkReachabilityChange(NetworkReachability networkReachability);

      /// <summary>Callback to determine whether the checks have completed or not.</summary>
      public delegate void OnlineCheckComplete(bool isConnected, NetworkReachability networkReachability);

      /// <summary>An event triggered whenever the Internet connection status changes.</summary>
      public event OnlineStatusChange OnOnlineStatusChange;

      /// <summary>An event triggered whenever the network reachability changes.</summary>
      public event NetworkReachabilityChange OnNetworkReachabilityChange;

      /// <summary>An event triggered whenever the Internet connection check is completed.</summary>
      public event OnlineCheckComplete OnOnlineCheckComplete;

      #endregion


      #region MonoBehaviour methods

      protected override void Awake()
      {
         base.Awake();

         existsCustomCheck = customCheck != null;
         isInternetAvailable = Application.internetReachability != NetworkReachability.NotReachable;
         networkReachability = lastNetworkReachability = Application.internetReachability;

#if !CT_DEVELOP
         if (existsCustomCheck && !Helper.isEditor && !string.IsNullOrEmpty(customCheck.URL) && (customCheck.URL.Contains("crosstales.com") || customCheck.URL.Contains("207.154.226.218")))
         {
            Debug.LogWarning($"'Custom Check' uses 'crosstales.com' for detection: this is only allowed for test-builds and the check interval will be limited!{System.Environment.NewLine}Please use your own URL for detection.", this);
            IntervalMin = Mathf.Clamp(IntervalMin, 30, 119);
            IntervalMax = Mathf.Clamp(IntervalMax, 60, 120);
         }
#endif
      }

      private void Start()
      {
         if ((runOnStart || Helper.isEditorMode) && !isRunning)
            Invoke(nameof(run), delay);
      }

      private void Update()
      {
#if !CT_DEVELOP
         if (existsCustomCheck && !Helper.isEditor && !string.IsNullOrEmpty(customCheck.URL) && (customCheck.URL.Contains("crosstales.com") || customCheck.URL.Contains("207.154.226.218")))
         {
            IntervalMin = 59;
            IntervalMax = 60;
         }
#endif
         if (!Helper.isEditorMode)
         {
            Context.Runtime += Time.unscaledDeltaTime;
            if (isInternetAvailable)
            {
               Context.Uptime += Time.unscaledDeltaTime;
            }
         }

         if (endlessMode && !isRunning)
         {
            checkTimeCounter += Time.unscaledDeltaTime;
            burstTimeCounter += Time.unscaledDeltaTime;

            if (isInternetAvailable)
            {
               if (checkTimeCounter >= checkTime)
               {
                  if (Constants.DEV_DEBUG)
                     Debug.Log("Normal-Mode!", this);

                  checkTimeCounter = 0f;
                  burstTimeCounter = 0f;

                  StartCoroutine(internetCheck());
               }
            }
            else
            {
               if (burstTimeCounter >= burstTime)
               {
                  if (Constants.DEV_DEBUG)
                     Debug.Log("Burst-Mode!", this);

                  checkTimeCounter = 0f;
                  burstTimeCounter = 0f;

                  StartCoroutine(internetCheck());
               }
            }
         }
      }

      protected override void OnApplicationQuit()
      {
#if (!UNITY_WSA && !UNITY_WEBGL && !UNITY_XBOXONE) || UNITY_EDITOR
         worker.CTAbort();
#endif
         base.OnApplicationQuit();
      }

      private void OnValidate()
      {
         if (delay < 0f)
            delay = 0f;
         if (intervalMin < 3)
            intervalMin = 3;
         if (intervalMin > 59)
            intervalMin = 59;
         if (intervalMax < 4)
            intervalMax = 4;
         if (intervalMax > 60)
            intervalMax = 60;
         if (intervalMin <= timeout)
            intervalMin = timeout + 1;
         if (intervalMin >= intervalMax)
            intervalMax = intervalMin + 1;
      }

      #endregion


      #region Public methods

      /// <summary>Resets this object.</summary>
      public static void ResetObject()
      {
         DeleteInstance();
      }

      /// <summary>Checks for Internet availability.</summary>
      /// <param name="triggerCallbacks">Always trigger the callbacks (default: false, optional)</param>
      public void Refresh(bool triggerCallbacks = false)
      {
         if (this != null && !isActiveAndEnabled)
            return;

         //if (!isRunning && lastCheckTime + 10000000 < System.DateTime.Now.Ticks)
         if (!isRunning && lastCheckTime + 1f < Time.realtimeSinceStartup)
         {
            isFirsttime = isFirsttime || triggerCallbacks;
            StartCoroutine(internetCheck());
         }
      }

      /// <summary>Checks for Internet availability as an IEnumerator.</summary>
      /// <param name="triggerCallbacks">Always trigger the callbacks (default: false, optional)</param>
      public IEnumerator RefreshYield(bool triggerCallbacks = false)
      {
         //if (!isRunning && lastCheckTime + 10000000 < System.DateTime.Now.Ticks)
         if (this != null && isActiveAndEnabled && !isRunning && lastCheckTime + 1f < Time.realtimeSinceStartup)
         {
            isFirsttime = isFirsttime || triggerCallbacks;
            yield return internetCheck();
         }
         else
         {
            yield return null;
         }
      }

      #endregion


      #region Private methods

      private void run()
      {
         StartCoroutine(internetCheck());
      }

/*
        private void createCustomCheck()
        {
            Helper.CreateCustomCheck();
        }
*/
      private IEnumerator wwwCheck(string url, string data, bool equals, string type, int header, bool showError = false)
      {
         available = false;

         using (UnityWebRequest www = UnityWebRequest.Get(URLAntiCacheRandomizer(url)))
         {
            float startTime = Time.realtimeSinceStartup;

            www.timeout = timeout;
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();
#if UNITY_2020_1_OR_NEWER
            if (www.result != UnityWebRequest.Result.ProtocolError && www.result != UnityWebRequest.Result.ConnectionError)
#else
            if (!www.isHttpError && !www.isNetworkError)
#endif
            {
               string result = www.downloadHandler.text;

               if (Constants.DEV_DEBUG)
                  Debug.Log($"Content: {result}", this);

               if (equals)
               {
                  available = !string.IsNullOrEmpty(result) && result.CTEquals(data);
               }
               else
               {
                  available = !string.IsNullOrEmpty(result) && result.CTContains(data);
               }

               if (available)
                  LastCheckRTTMilliseconds = (int)((Time.realtimeSinceStartup - startTime) * 1000f);

               //Debug.Log($"wwwCheck '{url}': {(long)www.downloadedBytes + header}");
               DataDownloaded += (long)www.downloadedBytes + header;
            }
            else
            {
               if (Constants.DEV_DEBUG || showError)
                  Debug.LogError($"Error getting content from {type}: {www.error}", this);
            }
         }
      }

      private System.Collections.IEnumerator google204Check(bool showError = false)
      {
         available = false;
         string url = $"https://clients{Random.Range(1, 7)}.google.com/generate_204";

         using (UnityWebRequest www = UnityWebRequest.Head(URLAntiCacheRandomizer(url)))
         {
            float startTime = Time.realtimeSinceStartup;
            yield return www.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (www.result != UnityWebRequest.Result.ProtocolError && www.result != UnityWebRequest.Result.ConnectionError)
#else
            if (!www.isHttpError && !www.isNetworkError)
#endif
            {
               if (Constants.DEV_DEBUG)
                  Debug.Log($"Response from {url}: {www.responseCode}", this);

               available = www.responseCode == 204;

               if (available)
                  LastCheckRTTMilliseconds = (int)((Time.realtimeSinceStartup - startTime) * 1000f);

               DataDownloaded += google204HeaderSize;
            }
            else
            {
               if (Constants.DEV_DEBUG || showError)
                  Debug.LogError("Error getting content from Google204: " + www.error, this);
            }
         }
      }

      private IEnumerator googleBlankCheck(bool showError = false)
      {
         available = false;
         const string url = "https://www.google.com/blank.html";

         using (UnityWebRequest www = UnityWebRequest.Get(URLAntiCacheRandomizer(url)))
         {
            float startTime = Time.realtimeSinceStartup;

            www.timeout = timeout;
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (www.result != UnityWebRequest.Result.ProtocolError && www.result != UnityWebRequest.Result.ConnectionError)
#else
            if (!www.isHttpError && !www.isNetworkError)
#endif
            {
               string result = www.downloadHandler.text;

               if (Constants.DEV_DEBUG)
                  Debug.Log($"Content from GoogleBlank: {result}", this);

               available = www.downloadedBytes == 0;

               if (available)
                  LastCheckRTTMilliseconds = (int)((Time.realtimeSinceStartup - startTime) * 1000f);

               //Debug.Log($"googleBlankCheck: {www.downloadedBytes + googleBlankHeaderSize}");
               DataDownloaded += (long)www.downloadedBytes + googleBlankHeaderSize;
            }
            else
            {
               if (Constants.DEV_DEBUG || showError)
                  Debug.LogError($"Error getting content from GoogleBlank: {www.error}", this);
            }
         }
      }

      private void threadCheck(out bool _available, string url, string data, bool equals, string type, int header, bool showError = false)
      {
         _available = false;

#if (!UNITY_WSA && !UNITY_XBOXONE) || UNITY_EDITOR
         try

         {
            using (System.Net.WebClient client = new CTWebClientNotCached(timeout * 1000))
            {
               System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();

               string content = client.DownloadString(url);

               if (Constants.DEV_DEBUG)
                  Debug.Log($"Content from {type}: {content}", this);

               if (equals)
               {
                  _available = !string.IsNullOrEmpty(content) && content.CTEquals(data);
               }
               else
               {
                  _available = !string.IsNullOrEmpty(content) && content.CTContains(data);
               }

               if (available)
                  LastCheckRTTMilliseconds = (int)watch.ElapsedMilliseconds;

               watch.Stop();

               //Debug.Log($"threadCheck '{url}': {content.Length + header}");
               DataDownloaded += content.Length + header;
            }
         }
         catch (System.Exception ex)
         {
            if (Constants.DEV_DEBUG || showError)
               Debug.LogError($"Error getting content from {type}: {ex}", this);
         }
#endif
      }

      private IEnumerator startWorker(string url, string data, bool equals, string type, int header, bool showError = false)
      {
#if (!UNITY_WSA && !UNITY_XBOXONE) || UNITY_EDITOR
         if (worker?.IsAlive != true)
         {
            worker = new System.Threading.Thread(() => threadCheck(out available, url, data, equals, type, header, showError));
            worker.Start();

            do
            {
               yield return null;
            } while (worker.IsAlive);
         }
#else
            yield return null;
#endif
      }

      private IEnumerator internetCheck()
      {
         if (!isRunning)
         {
            Context.NumberOfChecks++;

            isRunning = true;

            available = false;

            if (Config.DEBUG)
               Debug.Log($"[{System.DateTime.Now}] {Constants.ASSET_NAME} running...", this);

#if OC_UNAVAILABLE
            Debug.LogWarning($"[{System.DateTime.Now}] Compile define 'OC_UNAVAILABLE' enabled. Result of the check is always 'UNAVAILABLE'.", this);

            available = false;

            yield return null;
#elif OC_AVAILABLE
            Debug.LogWarning($"[{System.DateTime.Now}] Compile define 'OC_AVAILABLE' enabled. Result of the check is always 'AVAILABLE'.", this);

            available = true;

            yield return null;
#else
            if (Helper.isEditorMode)
            {
               // Unity check
               if (Constants.DEV_DEBUG)
                  Debug.LogWarning($"[{System.DateTime.Now}] Editor is using Unity check (= unreliable!).", this);

               available = Application.internetReachability != NetworkReachability.NotReachable;
            }
            else if (Helper.isWebPlatform)
            {
               // Custom check
               if (existsCustomCheck && !string.IsNullOrEmpty(customCheck.URL))
               {
                  if (Constants.DEV_DEBUG)
                     Debug.Log($"[{System.DateTime.Now}] {testingDesc} {customDesc}", this);

                  yield return wwwCheck(customCheck.URL.Trim(), customCheck.ExpectedData, customCheck.DataMustBeEquals, customDesc, customCheck.HeaderSize, customCheck.ShowErrors);
               }
               else
               {
                  // Unity check
                  Debug.LogWarning("CustomCheck not set correctly - using Unity check (= unreliable!).", this);

                  available = Application.internetReachability != NetworkReachability.NotReachable;
               }
            }
            else
            {
#if (!UNITY_WSA && !UNITY_XBOXONE) || UNITY_EDITOR
               System.Net.ServicePointManager.ServerCertificateValidationCallback = Crosstales.Common.Util.NetworkHelper.RemoteCertificateValidationCallback;
#endif
               // Custom check
               if (existsCustomCheck && !string.IsNullOrEmpty(customCheck.URL))
               {
                  if (Constants.DEV_DEBUG)
                     Debug.Log($"[{System.DateTime.Now}] {testingDesc} {customDesc}", this);

                  if (ForceWWW)
                  {
                     yield return wwwCheck(customCheck.URL.Trim(), customCheck.ExpectedData, customCheck.DataMustBeEquals, customDesc, customCheck.HeaderSize, customCheck.ShowErrors);
                  }
                  else
                  {
                     yield return startWorker(customCheck.URL.Trim(), customCheck.ExpectedData, customCheck.DataMustBeEquals, customDesc, customCheck.HeaderSize, customCheck.ShowErrors);
                  }
               }
               else
               {
                  // Unity check
                  if (existsCustomCheck && customCheck.UseOnlyCustom)
                  {
                     Debug.LogWarning("CustomCheck not set correctly - using Unity check (= unreliable!).", this);

                     available = Application.internetReachability != NetworkReachability.NotReachable;
                  }
               }

               if (!existsCustomCheck || existsCustomCheck && !customCheck.UseOnlyCustom)
               {
                  // Microsoft check
                  if (microsoft && !available && !Helper.isAppleBasedPlatform)
                  {
                     if (Constants.DEV_DEBUG)
                        Debug.Log($"[{System.DateTime.Now}] {testingDesc} {windowsDesc}", this);

                     if (ForceWWW)
                     {
                        yield return wwwCheck(microsoftUrl, microsoftText, microsoftEquals, windowsDesc, microsoftHeaderSize);
                     }
                     else
                     {
                        yield return startWorker(microsoftUrl, microsoftText, microsoftEquals, windowsDesc, microsoftHeaderSize);
                     }

                     if (Constants.DEV_DEBUG)
                        Debug.Log($"{windowsDesc} check: {available}", this);
                  }

                  // Google204 check
                  if (google204 && !available)
                  {
                     if (Constants.DEV_DEBUG)
                        Debug.Log($"[{System.DateTime.Now}] {testingDesc} Google204", this);

                     yield return google204Check();

                     if (Constants.DEV_DEBUG)
                        Debug.Log($"Google204 check: {available}", this);
                  }

                  // GoogleBlank check
                  if (googleBlank && !available)
                  {
                     if (Constants.DEV_DEBUG)
                        Debug.Log($"[{System.DateTime.Now}] {testingDesc} GoogleBlank", this);

                     yield return googleBlankCheck();

                     if (Constants.DEV_DEBUG)
                        Debug.Log($"GoogleBlank check: {available}", this);
                  }

                  // Apple check
                  if (!available && (apple || Helper.isAppleBasedPlatform))
                  {
                     if (Constants.DEV_DEBUG)
                        Debug.Log($"[{System.DateTime.Now}] {testingDesc} {appleDesc}", this);

                     if (ForceWWW)
                     {
                        yield return wwwCheck(appleUrl, appleText, appleEquals, appleDesc, appleHeaderSize);
                     }
                     else
                     {
                        yield return startWorker(appleUrl, appleText, appleEquals, appleDesc, appleHeaderSize);
                     }

                     if (Constants.DEV_DEBUG)
                        Debug.Log($"{appleDesc} check: {available}", this);
                  }

                  // Ubuntu check
                  if (ubuntu && !available)
                  {
                     if (Constants.DEV_DEBUG)
                        Debug.Log($"[{System.DateTime.Now}] {testingDesc} {ubuntuDesc}", this);

                     if (ForceWWW)
                     {
                        yield return wwwCheck(ubuntuUrl, ubuntuText, ubuntuEquals, ubuntuDesc, ubuntuHeaderSize);
                     }
                     else
                     {
                        yield return startWorker(ubuntuUrl, ubuntuText, ubuntuEquals, ubuntuDesc, ubuntuHeaderSize);
                     }

                     if (Constants.DEV_DEBUG)
                        Debug.Log($"{ubuntuDesc} check: {available}", this);
                  }

                  // Fallback check
                  //if (fallback && !available)
                  if (!available)
                  {
                     if (Constants.DEV_DEBUG)
                        Debug.Log($"[{System.DateTime.Now}] {testingDesc} {fallbackDesc}", this);

                     if (ForceWWW)
                     {
                        yield return wwwCheck(fallbackUrl, fallbackText, fallbackEquals, fallbackDesc, fallbackHeaderSize);
                     }
                     else
                     {
                        yield return startWorker(fallbackUrl, fallbackText, fallbackEquals, fallbackDesc, fallbackHeaderSize);
                     }

                     if (Constants.DEV_DEBUG)
                        Debug.Log($"{fallbackDesc} check: {available}", this);
                  }
               }
            }
#endif
            isInternetAvailable = available;

            if (!isInternetAvailable || Application.internetReachability == NetworkReachability.NotReachable)
            {
               networkReachability = NetworkReachability.NotReachable;
            }
            else
            {
               networkReachability = Application.internetReachability;
            }

            onInternetCheckComplete(isInternetAvailable, networkReachability);

            if (isFirsttime || isInternetAvailable != lastInternetAvailable)
            {
               lastInternetAvailable = isInternetAvailable;

               onInternetStatusChange(isInternetAvailable);
            }

            if (isFirsttime || networkReachability != lastNetworkReachability)
            {
               lastNetworkReachability = networkReachability;

               onNetworkReachabilityChange(networkReachability);
            }

            if (Config.DEBUG)
            {
               if (isInternetAvailable)
               {
                  Debug.Log($"[{System.DateTime.Now}] Internet access AVAILABLE!", this);
               }
               else
               {
                  Debug.LogWarning($"[{System.DateTime.Now}] Internet access UNAVAILABLE!", this);
               }
            }

            checkTime = Random.Range(IntervalMin, IntervalMax);
            burstTime = Random.Range(burstIntervalMin, burstIntervalMax);
            //lastCheckTime = System.DateTime.Now.Ticks;
            lastCheckTime = Time.realtimeSinceStartup;

            isFirsttime = false;
            isRunning = false;
         }
         else
         {
            Debug.LogWarning($"{Constants.ASSET_NAME} already running!", this);
         }
      }

      private static string URLAntiCacheRandomizer(string url)
      {
         return $"{url}?p={Random.Range(1, int.MaxValue)}";
      }

      #endregion


      #region Event-trigger methods

      private void onInternetStatusChange(bool isConnected)
      {
         if (Config.DEBUG)
            Debug.Log($"onInternetStatusChange: {isConnected}", this);
         if (!Helper.isEditorMode)
            OnStatusChange?.Invoke(isConnected);
         OnOnlineStatusChange?.Invoke(isConnected);
      }

      private void onNetworkReachabilityChange(NetworkReachability _networkReachability)
      {
         if (Config.DEBUG)
            Debug.Log($"onNetworkReachabilityChange: {_networkReachability}", this);
         OnNetworkReachabilityChange?.Invoke(_networkReachability);
      }

      private void onInternetCheckComplete(bool isConnected, NetworkReachability _networkReachability)
      {
         LastCheck = System.DateTime.Now;
         if (Config.DEBUG)
            Debug.Log($"onInternetCheckComplete: {isConnected} - {_networkReachability}", this);
         OnOnlineCheckComplete?.Invoke(isConnected, _networkReachability);
      }

      #endregion
   }
}
// © 2017-2024 crosstales LLC (https://www.crosstales.com)