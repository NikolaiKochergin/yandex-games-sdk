using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace VervePlace.YandexGames
{
    public static class InterstitialAd
    {
        private static Action s_onOpenCallback;
        private static Action<bool> s_onCloseCallback;
        private static Action<string> s_onErrorCallback;
        private static Action s_onOfflineCallback;

        public static void Show(Action onOpenCallback = null, Action<bool> wasShownOnCloseCallback = null,
            Action<string> onErrorCallback = null, Action onOfflineCallback = null)
        {
            s_onOpenCallback = onOpenCallback;
            s_onCloseCallback = wasShownOnCloseCallback;
            s_onErrorCallback = onErrorCallback;
            s_onOfflineCallback = onOfflineCallback;

            InterstitialAdShow(OnOpenCallback, OnCloseCallback, OnErrorCallback, OnOfflineCallback);
        }

        [DllImport("__Internal")]
        private static extern bool InterstitialAdShow(Action openCallback, Action<bool> closeCallback,
            Action<string> errorCallback, Action offlineCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnOpenCallback()
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(InterstitialAd)}.{nameof(OnOpenCallback)} invoked");

            s_onOpenCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<bool>))]
        private static void OnCloseCallback(bool wasShown)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log(
                    $"{nameof(InterstitialAd)}.{nameof(OnCloseCallback)} invoked, {nameof(wasShown)} = {wasShown}");

            s_onCloseCallback?.Invoke(wasShown);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnErrorCallback(string errorMessage)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log(
                    $"{nameof(InterstitialAd)}.{nameof(OnErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onErrorCallback?.Invoke(errorMessage);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnOfflineCallback()
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(InterstitialAd)}.{nameof(OnOfflineCallback)} invoked");

            s_onOfflineCallback?.Invoke();
        }
    }
}