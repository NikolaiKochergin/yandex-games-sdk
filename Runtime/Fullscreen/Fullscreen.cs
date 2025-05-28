using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace VervePlace.YandexGames
{
    public static class Fullscreen
    {
        private static Action<bool> s_onGetSuccessCallback;
        private static Action<string> s_onGetErrorCallback;
        
        public static bool IsFullscreen => ScreenIsFullScreen();

        [DllImport("__Internal")]
        private static extern bool ScreenIsFullScreen();

        public static void SetFullscreen(bool isFullscreen, Action<bool> successCallback = null, Action<string> errorCallback = null)
        {
            s_onGetSuccessCallback = successCallback;
            s_onGetErrorCallback = errorCallback;
            
            ScreenSetFullscreen(isFullscreen, OnGetSuccessCallback, OnGetErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern void ScreenSetFullscreen(bool isFullscreen, Action<bool> successCallback = null, Action<string> errorCallback = null);

        [MonoPInvokeCallback(typeof(Action<bool>))]
        private static void OnGetSuccessCallback(bool isFullscreen)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(Fullscreen)}.{nameof(OnGetSuccessCallback)} invoked, {nameof(isFullscreen)} = {isFullscreen}");
            
            s_onGetSuccessCallback?.Invoke(isFullscreen);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetErrorCallback(string errorMessage)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(Fullscreen)}.{nameof(OnGetErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onGetErrorCallback?.Invoke(errorMessage);
        }
    }
}