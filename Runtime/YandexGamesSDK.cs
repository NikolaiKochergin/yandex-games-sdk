using System;
using System.Collections;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace VervePlace.YandexGames
{
    public static class YandexGamesSDK
    {
        private static Action s_OnInitializeSuccessCallback;

        public static bool CallbackLogging = false;

        public static bool IsInitialized => GetYandexGamesSdkIsInitialized();

        public static YandexGamesEnvironment Environment
        {
            get
            {
                string environmentJson = GetYandexGamesSdkEnvironment();
                return JsonUtility.FromJson<YandexGamesEnvironment>(environmentJson);
            }
        }

        public static YandexGamesDeviceInfo DeviceInfo
        {
            get
            {
                string deviceInfoJson = GetDeviceInfo();
                return JsonUtility.FromJson<YandexGamesDeviceInfo>(deviceInfoJson);
            }
        } 

        public static IEnumerator Initialize(Action onSuccessCallback = null)
        {
            s_OnInitializeSuccessCallback = onSuccessCallback;

            YandexGamesSdkInitialize(OnInitializeSuccessCallback);

            while (!IsInitialized)
                yield return null;
        }

        [DllImport("__Internal")]
        private static extern void YandexGamesSdkInitialize(Action successCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnInitializeSuccessCallback()
        {
            if (CallbackLogging)
                Debug.Log($"{nameof(YandexGamesSDK)}.{nameof(OnInitializeSuccessCallback)} invoked");

            s_OnInitializeSuccessCallback?.Invoke();
        }

        [DllImport("__Internal")]
        private static extern bool GetYandexGamesSdkIsInitialized();
        
        [DllImport("__Internal")]
        private static extern string GetYandexGamesSdkEnvironment();
        
        [DllImport("__Internal")]
        private static extern string GetDeviceInfo();
    }
}