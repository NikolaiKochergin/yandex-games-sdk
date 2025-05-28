using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace VervePlace.YandexGames
{
    public static class Clipboard
    {
        private static Action<string> s_onReadSuccessCallback;
        private static Action<string> s_onReadErrorCallback;

        #region Write

        public static void Write(string text) => ClipboardWrite(text);

        [DllImport("__Internal")]
        private static extern void ClipboardWrite(string text);

        #endregion

        #region Read

        // Doesn't work on Yandex Games
        
        // public static void Read(Action<string> onSuccessCallback, Action<string> onErrorCallback = null)
        // {
        //     s_onReadSuccessCallback = onSuccessCallback;
        //     s_onReadErrorCallback = onErrorCallback;
        //
        //     ClipboardRead(OnReadSuccessCallback, OnReadErrorCallback);
        // }
        //
        // [DllImport("__Internal")]
        // private static extern void ClipboardRead(Action<string> successCallback, Action<string> errorCallback);
        //
        // [MonoPInvokeCallback(typeof(Action<string>))]
        // private static void OnReadSuccessCallback(string text)
        // {
        //     if (YandexGamesSDK.CallbackLogging)
        //         Debug.Log($"{nameof(Clipboard)}.{nameof(OnReadSuccessCallback)} invoked, {nameof(text)} = {text}");
        //
        //     s_onReadSuccessCallback?.Invoke(text);
        // }
        //
        // [MonoPInvokeCallback(typeof(Action<string>))]
        // private static void OnReadErrorCallback(string errorMessage)
        // {
        //     if (YandexGamesSDK.CallbackLogging)
        //         Debug.Log($"{nameof(Clipboard)}.{nameof(OnReadErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");
        //
        //     s_onReadErrorCallback?.Invoke(errorMessage);
        // }

        #endregion
    }
}