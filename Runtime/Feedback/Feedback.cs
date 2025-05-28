using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace VervePlace.YandexGames
{
    public static class Feedback
    {
        private static Action<bool, string> s_onGetCanReviewResultCallback;
        private static Action<bool> s_onGetRequestReviewResultCallback;

        public static void CanReview(Action<bool, string> resultCallback)
        {
            s_onGetCanReviewResultCallback = resultCallback;
            FeedbackCanReview(OnGetCanReviewResultCallback);
        }

        [DllImport("__Internal")]
        private static extern bool FeedbackCanReview(Action<bool, string> canReviewResultCallback);

        [MonoPInvokeCallback(typeof(Action<bool, string>))]
        private static void OnGetCanReviewResultCallback(bool canReview, string reason)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(Feedback)}.{nameof(OnGetCanReviewResultCallback)} invoked, {nameof(canReview)} = {canReview}, {nameof(reason)} = {reason}");
            
            s_onGetCanReviewResultCallback?.Invoke(canReview, reason);
        }

        public static void RequestReview(Action<bool> resultCallback)
        {
            s_onGetRequestReviewResultCallback = resultCallback;
            FeedbackRequestReview(OnGetRequestReviewCallback);
        }

        [DllImport("__Internal")]
        private static extern bool FeedbackRequestReview(Action<bool> requestReviewResultCallback);

        [MonoPInvokeCallback(typeof(Action<bool>))]
        private static void OnGetRequestReviewCallback(bool isFeedbackSent)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(Feedback)}.{nameof(OnGetRequestReviewCallback)} invoked, {nameof(isFeedbackSent)} = {isFeedbackSent}");
            
            s_onGetRequestReviewResultCallback?.Invoke(isFeedbackSent);
        }
    }
}