using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace VervePlace.YandexGames
{
    public static class Leaderboard
    {
        #region Get Description
        private static Action<LeaderboardDescriptionResponse> s_onGetDescriptionSuccessCallback;
        private static Action<string> s_onGetDescriptionErrorCallback;

        public static void GetDescription(string leaderboardName, Action<LeaderboardDescriptionResponse> onSuccessCallback, Action<string> onErrorCallback = null)
        {
            if (leaderboardName == null)
                throw new ArgumentNullException(nameof(leaderboardName));
            
            s_onGetDescriptionSuccessCallback = onSuccessCallback;
            s_onGetDescriptionErrorCallback = onErrorCallback;

            LeaderboardGetDescription(leaderboardName, OnGetDescriptionSuccessCallback, OnGetDescriptionErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern void LeaderboardGetDescription(string leaderboardName, Action<string> onSuccessCallback, Action<string> onErrorCallback);

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetDescriptionSuccessCallback(string descriptionResponseJson)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(Leaderboard)}.{nameof(OnGetDescriptionSuccessCallback)} invoked, {nameof(descriptionResponseJson)} = {descriptionResponseJson}");
            
            LeaderboardDescriptionResponse descriptionResponse = descriptionResponseJson == "null" ? null : JsonUtility.FromJson<LeaderboardDescriptionResponse>(descriptionResponseJson);

            s_onGetDescriptionSuccessCallback?.Invoke(descriptionResponse);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetDescriptionErrorCallback(string errorMessage)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(Leaderboard)}.{nameof(OnGetDescriptionErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onGetDescriptionErrorCallback?.Invoke(errorMessage);
        }
        #endregion

        #region Set Score
        private static Action s_onSetScoreSuccessCallback;
        private static Action<string> s_onSetScoreErrorCallback;

        public static void SetScore(string leaderboardName, int score, Action onSuccessCallback = null, Action<string> onErrorCallback = null, string extraData = "")
        {
            if (leaderboardName == null)
                throw new ArgumentNullException(nameof(leaderboardName));

            extraData ??= string.Empty;

            s_onSetScoreSuccessCallback = onSuccessCallback;
            s_onSetScoreErrorCallback = onErrorCallback;

            LeaderboardSetScore(leaderboardName, score, OnSetScoreSuccessCallback, OnSetScoreErrorCallback, extraData);
        }

        [DllImport("__Internal")]
        private static extern void LeaderboardSetScore(string leaderboardName, int score, Action successCallback, Action<string> errorCallback, string extraData);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnSetScoreSuccessCallback()
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(Leaderboard)}.{nameof(OnSetScoreSuccessCallback)} invoked");

            s_onSetScoreSuccessCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnSetScoreErrorCallback(string errorMessage)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(Leaderboard)}.{nameof(OnSetScoreErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onSetScoreErrorCallback?.Invoke(errorMessage);
        }
        #endregion
        
        #region GetPlayerEntry
        private static Action<LeaderboardEntryResponse> s_onGetPlayerEntrySuccessCallback;
        private static Action<string> s_onGetPlayerEntryErrorCallback;
        
        public static void GetPlayerEntry(string leaderboardName, Action<LeaderboardEntryResponse> onSuccessCallback, Action<string> onErrorCallback = null, ProfilePictureSize pictureSize = ProfilePictureSize.medium)
        {
            if (leaderboardName == null)
                throw new ArgumentNullException(nameof(leaderboardName));

            s_onGetPlayerEntrySuccessCallback = onSuccessCallback;
            s_onGetPlayerEntryErrorCallback = onErrorCallback;

            LeaderboardGetPlayerEntry(leaderboardName, OnGetPlayerEntrySuccessCallback, OnGetPlayerEntryErrorCallback, pictureSize.ToString());
        }

        [DllImport("__Internal")]
        private static extern void LeaderboardGetPlayerEntry(string leaderboardName, Action<string> successCallback, Action<string> errorCallback, string pictureSize);

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetPlayerEntrySuccessCallback(string entryResponseJson)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(Leaderboard)}.{nameof(OnGetPlayerEntrySuccessCallback)} invoked, {nameof(entryResponseJson)} = {entryResponseJson}");

            LeaderboardEntryResponse entryResponse = entryResponseJson == "null" ? null : JsonUtility.FromJson<LeaderboardEntryResponse>(entryResponseJson);

            s_onGetPlayerEntrySuccessCallback?.Invoke(entryResponse);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetPlayerEntryErrorCallback(string errorMessage)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(Leaderboard)}.{nameof(OnGetPlayerEntryErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onGetPlayerEntryErrorCallback?.Invoke(errorMessage);
        }
        #endregion
        
        #region GetPlayerEntries
        private static Action<LeaderboardGetEntriesResponse> s_onGetEntriesSuccessCallback;
        private static Action<string> s_onGetEntriesErrorCallback;
        
        public static void GetEntries(string leaderboardName, Action<LeaderboardGetEntriesResponse> onSuccessCallback, Action<string> onErrorCallback = null, int topPlayersCount = 5, int competingPlayersCount = 5, bool includeSelf = true, ProfilePictureSize pictureSize = ProfilePictureSize.medium)
        {
            if (leaderboardName == null)
                throw new ArgumentNullException(nameof(leaderboardName));

            s_onGetEntriesSuccessCallback = onSuccessCallback;
            s_onGetEntriesErrorCallback = onErrorCallback;

            LeaderboardGetEntries(leaderboardName, OnGetEntriesSuccessCallback, OnGetEntriesErrorCallback, topPlayersCount, competingPlayersCount, includeSelf, pictureSize.ToString());
        }

        [DllImport("__Internal")]
        private static extern void LeaderboardGetEntries(string leaderboardName, Action<string> successCallback, Action<string> errorCallback, int topPlayersCount, int competingPlayersCount, bool includeSelf, string pictureSize);

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetEntriesSuccessCallback(string entriesResponseJson)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(Leaderboard)}.{nameof(OnGetEntriesSuccessCallback)} invoked, {nameof(entriesResponseJson)} = {entriesResponseJson}");

            LeaderboardGetEntriesResponse entriesResponse = JsonUtility.FromJson<LeaderboardGetEntriesResponse>(entriesResponseJson);

            s_onGetEntriesSuccessCallback?.Invoke(entriesResponse);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetEntriesErrorCallback(string errorMessage)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(Leaderboard)}.{nameof(OnGetEntriesErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onGetEntriesErrorCallback?.Invoke(errorMessage);
        }
        #endregion
    }
}