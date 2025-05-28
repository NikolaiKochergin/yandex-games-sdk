using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace VervePlace.YandexGames
{
    public static class Shortcut
    {
        private static Action<bool> s_onGetCanShowPromptResultCallback;
        private static Action<bool> s_onGetShowPromptResultCallback;

        public static void CanShowPrompt(Action<bool> resultCallback)
        {
            s_onGetCanShowPromptResultCallback = resultCallback;
            ShortcutCanShowPrompt(OnGetCanShowPromptResultCallback);
        }

        [DllImport("__Internal")]
        private static extern bool ShortcutCanShowPrompt(Action<bool> canReviewResultCallback);

        [MonoPInvokeCallback(typeof(Action<bool>))]
        private static void OnGetCanShowPromptResultCallback(bool canShow)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(Feedback)}.{nameof(OnGetCanShowPromptResultCallback)} invoked, {nameof(canShow)} = {canShow}");
            
            s_onGetCanShowPromptResultCallback?.Invoke(canShow);
        }

        public static void ShowPrompt(Action<bool> resultCallback)
        {
            s_onGetShowPromptResultCallback = resultCallback;
            ShortcutShowPrompt(OnGetShowPromptCallback);
        }

        [DllImport("__Internal")]
        private static extern bool ShortcutShowPrompt(Action<bool> showPromptResultCallback);

        [MonoPInvokeCallback(typeof(Action<bool>))]
        private static void OnGetShowPromptCallback(bool isShortcutCreated)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(Feedback)}.{nameof(OnGetShowPromptCallback)} invoked, {nameof(isShortcutCreated)} = {isShortcutCreated}");
            
            s_onGetShowPromptResultCallback?.Invoke(isShortcutCreated);
        }
    }
}