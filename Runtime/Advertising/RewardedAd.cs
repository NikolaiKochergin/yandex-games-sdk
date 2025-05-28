using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace VervePlace.YandexGames
{
    public static class RewardedAd
    {
        private static Action s_onOpenCallback;
        private static Action s_onRewardedCallback;
        private static Action s_onCloseCallback;
        private static Action<string> s_onErrorCallback;

        private static bool s_isVideoAdOpen;

        public static void Show(Action onOpenCallback = null, Action onRewardedCallback = null,
            Action onCloseCallback = null, Action<string> onErrorCallback = null)
        {
            s_onOpenCallback = onOpenCallback;
            s_onRewardedCallback = onRewardedCallback;
            s_onCloseCallback = onCloseCallback;
            s_onErrorCallback = onErrorCallback;

            RewardedAdShow(OnOpenCallback, OnRewardedCallback, OnCloseCallback, OnErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern bool RewardedAdShow(Action openCallback, Action rewardedCallback, Action closeCallback, Action<string> errorCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnOpenCallback()
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(RewardedAd)}.{nameof(OnOpenCallback)} invoked");

            s_isVideoAdOpen = true;

            s_onOpenCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnRewardedCallback()
        {
            if (!s_isVideoAdOpen)
            {
                if (YandexGamesSDK.CallbackLogging)
                    Debug.Log($"Ignoring {nameof(RewardedAd)}.{nameof(OnRewardedCallback)} because {nameof(s_isVideoAdOpen)} is {s_isVideoAdOpen}");

                return;
            }

            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(RewardedAd)}.{nameof(OnRewardedCallback)} invoked");

            s_onRewardedCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnCloseCallback()
        {
            if (!s_isVideoAdOpen)
            {
                if (YandexGamesSDK.CallbackLogging)
                    Debug.Log($"Ignoring {nameof(RewardedAd)}.{nameof(OnCloseCallback)} because {nameof(s_isVideoAdOpen)} is {s_isVideoAdOpen}");

                return;
            }

            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(RewardedAd)}.{nameof(OnCloseCallback)} invoked");

            s_isVideoAdOpen = false;

            s_onCloseCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnErrorCallback(string errorMessage)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(RewardedAd)}.{nameof(OnErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onErrorCallback?.Invoke(errorMessage);
        }
    }
}