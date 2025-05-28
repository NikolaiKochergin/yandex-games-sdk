using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace VervePlace.YandexGames
{
    public static class GamesAPI
    {
        private static Action<AllGameInfosResponse> s_onGetAllGameInfosResponseCallback;
        private static Action<string> s_onGetAllGameInfosErrorCallback;
        
        private static Action<GameInfoResponse> s_onGetGameInfoByIdResponseCallback;
        private static Action<string> s_onGetGameInfoByIdErrorCallback;

        public static void GetAllGames(Action<AllGameInfosResponse> onSuccessCallback, Action<string> onErrorCallback = null)
        {
            s_onGetAllGameInfosResponseCallback = onSuccessCallback;
            s_onGetAllGameInfosErrorCallback = onErrorCallback;

            GamesAPIGetAllGames(OnGetAllGameInfosResponseCallback, OnGetAllGameInfosErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern void GamesAPIGetAllGames(Action<string> onSuccessCallback, Action<string> onErrorCallback);

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetAllGameInfosResponseCallback(string gameInfosResponseJson)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(GamesAPI)}.{nameof(OnGetAllGameInfosResponseCallback)} invoked, {nameof(gameInfosResponseJson)} = {gameInfosResponseJson}");
            
            AllGameInfosResponse gameInfosResponse = gameInfosResponseJson == "null" ? null : JsonUtility.FromJson<AllGameInfosResponse>(gameInfosResponseJson);
            s_onGetAllGameInfosResponseCallback?.Invoke(gameInfosResponse);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetAllGameInfosErrorCallback(string errorMessage)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(GamesAPI)}.{nameof(OnGetAllGameInfosErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onGetAllGameInfosErrorCallback?.Invoke(errorMessage);
        }

        public static void GetGameByID(int id, Action<GameInfoResponse> onSuccessCallback, Action<string> onErrorCallback = null)
        {
            s_onGetGameInfoByIdResponseCallback = onSuccessCallback;
            s_onGetGameInfoByIdErrorCallback = onErrorCallback;
            
            GamesAPIGetGameByID(id, OnGetGameInfoByIdResponseCallback, OnGetGameInfoByIdErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern void GamesAPIGetGameByID(int id, Action<string> onSuccessCallback, Action<string> onErrorCallback);

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetGameInfoByIdResponseCallback(string gameInfoResponseJson)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(GamesAPI)}.{nameof(OnGetGameInfoByIdResponseCallback)} invoked, {nameof(gameInfoResponseJson)} = {gameInfoResponseJson}");
            
            GameInfoResponse gameInfoResponse = gameInfoResponseJson == "null" ? null : JsonUtility.FromJson<GameInfoResponse>(gameInfoResponseJson);
            s_onGetGameInfoByIdResponseCallback?.Invoke(gameInfoResponse);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetGameInfoByIdErrorCallback(string errorMessage)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(GamesAPI)}.{nameof(OnGetGameInfoByIdErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onGetGameInfoByIdErrorCallback?.Invoke(errorMessage);
        }

        public static void OpenLink(string url) => GamesAPIOpenLink(url);

        [DllImport("__Internal")]
        private static extern void GamesAPIOpenLink(string url);
    }
}