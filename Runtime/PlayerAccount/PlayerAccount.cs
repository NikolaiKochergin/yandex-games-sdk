using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;
using UnityEngine.Scripting;

namespace VervePlace.YandexGames
{
    public static class PlayerAccount
    {
        #region AuthorizationDialogEvents

        private static Action s_onAccountSelectionDialogOpenedCallback;
        private static Action s_onAccountSelectionDialogClosedCallback;
        
        public static event Action AuthorizationDialogOpened
        {
            add
            {
                AddAccountSelectionOpenDialogListener(OnAccountSelectionDialogOpenedCallback);
                s_onAccountSelectionDialogOpenedCallback += value;
            }
            remove => s_onAccountSelectionDialogOpenedCallback -= value;
        }

        [DllImport("__Internal")]
        private static extern void AddAccountSelectionOpenDialogListener(Action accountSelectionDialogOpenedCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnAccountSelectionDialogOpenedCallback()
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnAccountSelectionDialogOpenedCallback)} invoked");
            
            s_onAccountSelectionDialogOpenedCallback?.Invoke();
        }

        public static event Action AuthorizationDialogClosed
        {
            add
            {
                AddAccountSelectionCloseDialogListener(OnAccountSelectionDialogClosedCallback);
                s_onAccountSelectionDialogClosedCallback += value;
            }
            remove => s_onAccountSelectionDialogClosedCallback -= value;
        }

        [DllImport("__Internal")]
        private static extern void AddAccountSelectionCloseDialogListener(Action accountSelectionDialogClosedCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnAccountSelectionDialogClosedCallback()
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnAccountSelectionDialogClosedCallback)} invoked");
            
            s_onAccountSelectionDialogClosedCallback?.Invoke();
        }

        #endregion
        
        #region Authorization
        private static Action s_onAuthorizedCallback;
        private static Action s_onAuthorizeSuccessCallback;
        private static Action<string> s_onAuthorizeErrorCallback;
        
        public static bool IsAuthorized => GetPlayerAccountIsAuthorized();
        public static event Action Authorized
        {
            add
            {
                AddPlayerAuthorizationListener(OnAuthorizedCallback);
                s_onAuthorizedCallback += value;
            }
            remove => s_onAuthorizedCallback -= value;
        }

        [DllImport("__Internal")]
        private static extern bool GetPlayerAccountIsAuthorized();
        
        public static void Authorize(Action onSuccessCallback = null, Action<string> onErrorCallback = null)
        {
            s_onAuthorizeSuccessCallback = onSuccessCallback;
            s_onAuthorizeErrorCallback = onErrorCallback;

            PlayerAccountAuthorize(OnAuthorizeSuccessCallback, OnAuthorizeErrorCallback);
        }
        
        [DllImport("__Internal")]
        private static extern void PlayerAccountAuthorize(Action onSuccessCallback, Action<string> onErrorCallback);
        
        [MonoPInvokeCallback(typeof(Action))]
        private static void OnAuthorizeSuccessCallback()
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnAuthorizeSuccessCallback)} invoked");

            s_onAuthorizeSuccessCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnAuthorizeErrorCallback(string errorMessage)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnAuthorizeErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onAuthorizeErrorCallback?.Invoke(errorMessage);
        }
        
        [DllImport("__Internal")]
        private static extern void AddPlayerAuthorizationListener(Action onAuthorizedCallback);
        
        [MonoPInvokeCallback(typeof(Action))]
        private static void OnAuthorizedCallback()
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnAuthorizedCallback)} invoked");
            
            s_onAuthorizedCallback?.Invoke();
        }
        #endregion
        
        #region Cloud Data
        private static Action s_onSetCloudSaveDataSuccessCallback;
        private static Action<string> s_onSetCloudSaveDataErrorCallback;
        private static Action<string> s_onGetCloudSaveDataSuccessCallback;
        private static Action<string> s_onGetCloudSaveDataErrorCallback;
        
        public static void SetCloudSaveData(string cloudSaveDataJson, bool flush = false, Action onSuccessCallback = null, Action<string> onErrorCallback = null)
        {
            if (cloudSaveDataJson == null)
                throw new ArgumentNullException(nameof(cloudSaveDataJson));

            if (string.IsNullOrEmpty(cloudSaveDataJson))
                cloudSaveDataJson = "{}";

            s_onSetCloudSaveDataSuccessCallback = onSuccessCallback;
            s_onSetCloudSaveDataErrorCallback = onErrorCallback;

            PlayerAccountSetCloudSaveData(cloudSaveDataJson, flush, OnSetCloudSaveDataSuccessCallback, OnSetCloudSaveDataErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern void PlayerAccountSetCloudSaveData(string cloudSaveDataJson, bool flush, Action onSuccessCallback, Action<string> onErrorCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnSetCloudSaveDataSuccessCallback()
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnSetCloudSaveDataSuccessCallback)} invoked");

            s_onSetCloudSaveDataSuccessCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnSetCloudSaveDataErrorCallback(string errorMessage)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnSetCloudSaveDataErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onSetCloudSaveDataErrorCallback?.Invoke(errorMessage);
        }
        
        public static void GetCloudSaveData(Action<string> onSuccessCallback = null, Action<string> onErrorCallback = null)
        {
            s_onGetCloudSaveDataSuccessCallback = onSuccessCallback;
            s_onGetCloudSaveDataErrorCallback = onErrorCallback;

            PlayerAccountGetCloudSaveData(OnGetCloudSaveDataSuccessCallback, OnGetCloudSaveDataErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern void PlayerAccountGetCloudSaveData(Action<string> successCallback, Action<string> errorCallback);

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetCloudSaveDataSuccessCallback(string playerDataJson)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnGetCloudSaveDataSuccessCallback)} invoked, {nameof(playerDataJson)} = {playerDataJson}");

            s_onGetCloudSaveDataSuccessCallback?.Invoke(playerDataJson);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetCloudSaveDataErrorCallback(string errorMessage)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnGetCloudSaveDataErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onGetCloudSaveDataErrorCallback?.Invoke(errorMessage);
        }

        private static Action<IReadOnlyDictionary<string, string>> s_onGetKeysDataSuccessCallback;
        private static Action<string> s_onGetKeysDataErrorCallback;

        public static void GetKeysCloudSaveData(IReadOnlyList<string> keys = null, Action<IReadOnlyDictionary<string, string>> onSuccessCallback = null, Action<string> onErrorCallback = null)
        {
            s_onGetKeysDataSuccessCallback = onSuccessCallback;
            s_onGetKeysDataErrorCallback = onErrorCallback;

            SerializableDictionary<string, string> keysMap = new();
            if(keys != null)
                foreach (string key in keys) keysMap.Dictionary.Add(key, key);
            
            string keysJson = JsonUtility.ToJson(keysMap);
            PlayerAccountGetKeysCloudSaveData(keysJson, OnGetKeysDataSuccessCallback, OnGetKeysDataErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern void PlayerAccountGetKeysCloudSaveData(string keysJson, Action<string> successCallback, Action<string> errorCallback);

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetKeysDataSuccessCallback(string keysDataJson)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnGetKeysDataSuccessCallback)} invoked");
            
            s_onGetKeysDataSuccessCallback?.Invoke(JsonUtility.FromJson<SerializableDictionary<string, string>>(keysDataJson).Dictionary);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetKeysDataErrorCallback(string errorMessage)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnGetKeysDataErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onGetKeysDataErrorCallback?.Invoke(errorMessage);
        }

        #endregion

        #region Personal Profile Data
        private static Action<PlayerAccountProfileDataResponse> s_onGetProfileDataSuccessCallback;
        private static Action<string> s_onGetProfileDataErrorCallback;

        public static void GetProfileData(Action<PlayerAccountProfileDataResponse> onSuccessCallback, Action<string> onErrorCallback = null, ProfilePictureSize pictureSize = ProfilePictureSize.medium)
        {
            s_onGetProfileDataSuccessCallback = onSuccessCallback;
            s_onGetProfileDataErrorCallback = onErrorCallback;

            PlayerAccountGetProfileData(OnGetProfileDataSuccessCallback, OnGetProfileDataErrorCallback, pictureSize.ToString());
        }

        [DllImport("__Internal")]
        private static extern void PlayerAccountGetProfileData(Action<string> successCallback, Action<string> errorCallback, string pictureSize);

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetProfileDataSuccessCallback(string profileDataResponseJson)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnGetProfileDataSuccessCallback)} invoked, {nameof(profileDataResponseJson)} = {profileDataResponseJson}");

            PlayerAccountProfileDataResponse profileDataResponse = JsonUtility.FromJson<PlayerAccountProfileDataResponse>(profileDataResponseJson);

            s_onGetProfileDataSuccessCallback?.Invoke(profileDataResponse);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetProfileDataErrorCallback(string errorMessage)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnGetProfileDataErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onGetProfileDataErrorCallback?.Invoke(errorMessage);
        }
        #endregion

        #region Cloud Stats

        private static Action s_onSetCloudStatsSuccessCallback;
        private static Action<string> s_onSetCloudStatsErrorCallback;

        public static void SetStats(IReadOnlyDictionary<string, int> stats, Action onSuccessCallback = null, Action<string> onErrorCallback = null)
        {
            s_onSetCloudStatsSuccessCallback = onSuccessCallback;
            s_onSetCloudStatsErrorCallback = onErrorCallback;

            SerializableDictionary<string, int> statsMap = new() { Dictionary = new Dictionary<string, int>(stats) };
            string statsJson = JsonUtility.ToJson(statsMap);
            PlayerAccountSetStats(statsJson, OnSetStatsSuccessCallback, OnSetStatsErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern void PlayerAccountSetStats(string statsJson, Action successCallback, Action<string> errorCallbackPtr);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnSetStatsSuccessCallback()
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnSetStatsSuccessCallback)} invoked");

            s_onSetCloudStatsSuccessCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnSetStatsErrorCallback(string errorMessage)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnSetStatsErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onSetCloudStatsErrorCallback?.Invoke(errorMessage);
        }

        
        private static Action<IReadOnlyDictionary<string, int>> s_onGetIncrementedStatsSuccessCallback;
        private static Action<string> s_onGetIncrementedStatsErrorCallback;

        public static void IncrementStats(IReadOnlyDictionary<string, int> stats, Action<IReadOnlyDictionary<string, int>> successCallbackPtr, Action<string> errorCallbackPtr)
        {
            s_onGetIncrementedStatsSuccessCallback = successCallbackPtr;
            s_onGetIncrementedStatsErrorCallback = errorCallbackPtr;
            
            SerializableDictionary<string, int> statsMap = new(){ Dictionary = new Dictionary<string, int>(stats) };
            
            string statsJson = JsonUtility.ToJson(statsMap);
            PlayerAccountIncrementStats(statsJson, OnGetIncrementedStatsSuccessCallback, OnGetIncrementedStatsErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern void PlayerAccountIncrementStats(string statsJson, Action<string> successCallbackPtr, Action<string> errorCallbackPtr);

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetIncrementedStatsSuccessCallback(string incrementedStatsJson)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnGetIncrementedStatsSuccessCallback)} invoked");
            
            s_onGetIncrementedStatsSuccessCallback?.Invoke(JsonUtility.FromJson<SerializableDictionary<string, int>>(incrementedStatsJson).Dictionary);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetIncrementedStatsErrorCallback(string errorMessage)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnGetIncrementedStatsErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onGetIncrementedStatsErrorCallback?.Invoke(errorMessage);
        }
        
        
        private static Action<IReadOnlyDictionary<string, int>> s_onGetStatsSuccessCallback;
        private static Action<string> s_onGetStatsErrorCallback;
        
        public static void GetStats(IReadOnlyList<string> statKeys = null, Action<IReadOnlyDictionary<string, int>> successCallbackPtr = null, Action<string> errorCallbackPtr = null)
        {
            s_onGetStatsSuccessCallback = successCallbackPtr;
            s_onGetStatsErrorCallback = errorCallbackPtr;
            
            SerializableDictionary<string, int> statsMap = new();
            if (statKeys != null)
                foreach (string key in statKeys)
                    statsMap.Dictionary.Add(key, 0);

            string statsJson = JsonUtility.ToJson(statsMap);
            PlayerAccountGetStats(statsJson, OnGetStatsSuccessCallback, OnGetStatsErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern void PlayerAccountGetStats(string keysJson, Action<string> successCallbackPtr, Action<string> errorCallbackPtr);

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetStatsSuccessCallback(string statsJson)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnGetStatsSuccessCallback)} invoked");
            
            s_onGetStatsSuccessCallback?.Invoke(JsonUtility.FromJson<SerializableDictionary<string, int>>(statsJson).Dictionary);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetStatsErrorCallback(string errorMessage)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnGetStatsErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onGetStatsErrorCallback?.Invoke(errorMessage);
        }

        #endregion
        
                
        [Serializable]
        private class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
        {
            public Dictionary<TKey, TValue> Dictionary = new();

            [Preserve] public TKey[] keys;
            [Preserve] public TValue[] values;

            public void OnBeforeSerialize()
            {
                keys = Dictionary.Keys.ToArray();
                values = Dictionary.Values.ToArray();
            }

            public void OnAfterDeserialize()
            {
                for (int i = 0; i < keys.Length; i++)
                    Dictionary.Add(keys[i], values[i]);
            }
        }
    }
}