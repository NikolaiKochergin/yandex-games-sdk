using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace VervePlace.YandexGames
{
    public static class StickyAd
    {
        private static Action<bool, string> s_onGetStickyAdStatusCallback;
        
        public static void Show() => StickyAdShow();

        [DllImport("__Internal")]
        private static extern void StickyAdShow();

        public static void Hide() => StickyAdHide();

        [DllImport("__Internal")]
        private static extern void StickyAdHide();

        public static void GetStatus(Action<bool, string> statusCallback)
        {
            s_onGetStickyAdStatusCallback = statusCallback;
            StickyAdGetStatus(OnGetStickyAdStatusCallback);
        }

        [DllImport("__Internal")]
        private static extern void StickyAdGetStatus(Action<bool, string> onGetStickyAdStatusCallback);

        [MonoPInvokeCallback(typeof(Action<bool, string>))]
        private static void OnGetStickyAdStatusCallback(bool stickyAdvIsShowing, string reason)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(StickyAd)}.{nameof(OnGetStickyAdStatusCallback)} invoked, {nameof(stickyAdvIsShowing)} = {stickyAdvIsShowing}, {nameof(reason)} = {reason}");
            
            s_onGetStickyAdStatusCallback?.Invoke(stickyAdvIsShowing, reason);
        }
    }
}