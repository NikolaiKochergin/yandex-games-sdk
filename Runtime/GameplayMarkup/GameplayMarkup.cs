using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace VervePlace.YandexGames
{
    public static class GameplayMarkup
    {
        private static Action<bool> s_onGamePausedCallback;
        private static Action s_onHistoryBackEventHappened;
        
        public static event Action<bool> GamePaused
        {
            add
            {
                AddGamePauseListener(OnGamePausedCallback);
                s_onGamePausedCallback += value;
            }
            remove => s_onGamePausedCallback -= value;
        }
        
        public static event Action HistoryBackEventHappened
        {
            add
            {
                AddHistoryBackEventListener(OnHistoryBackCallback);
                s_onHistoryBackEventHappened += value;
            }
            remove => s_onHistoryBackEventHappened -= value;
        }

        public static void GameReady()
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(YandexGamesSDK)}.{nameof(GameReady)} invoked");

            YandexGamesSdkGameReady();
        }

        [DllImport("__Internal")]
        private static extern void YandexGamesSdkGameReady();
        
        public static void GameStart()
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(YandexGamesSDK)}.{nameof(GameStart)} invoked");

            YandexGamesSdkGameStart();
        }

        [DllImport("__Internal")]
        private static extern void YandexGamesSdkGameStart();
        
        public static void GameStop()
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(YandexGamesSDK)}.{nameof(GameStop)} invoked");

            YandexGamesSdkGameStop();
        }

        [DllImport("__Internal")]
        private static extern void YandexGamesSdkGameStop();
        
        [DllImport("__Internal")]
        private static extern void AddGamePauseListener(Action<bool> onGamePausedCallback);
        
        [MonoPInvokeCallback(typeof(Action<bool>))]
        private static void OnGamePausedCallback(bool isPaused)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(GameplayMarkup)}.{nameof(OnGamePausedCallback)} invoked, {nameof(isPaused)} = {isPaused}");
            
            s_onGamePausedCallback?.Invoke(isPaused);
        }

        [DllImport("__Internal")]
        private static extern void AddHistoryBackEventListener(Action onHistoryBackCallback);
        
        [MonoPInvokeCallback(typeof(Action))]
        private static void OnHistoryBackCallback()
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(GameplayMarkup)}.{nameof(OnHistoryBackCallback)} invoked.");
            
            s_onHistoryBackEventHappened?.Invoke();
        }

        public static void DispatchExit() => DispatchExitEvent();

        [DllImport("__Internal")]
        private static extern void DispatchExitEvent();
    }
}