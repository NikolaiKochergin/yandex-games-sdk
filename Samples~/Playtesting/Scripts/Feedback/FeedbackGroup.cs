using TMPro;
using UnityEngine;

namespace VervePlace.YandexGames.Samples
{
    public class FeedbackGroup : MonoBehaviour
    {
        [SerializeField] private TMP_Text _resultText;

        public void OnCanReviewButtonClicked() => Feedback.CanReview(OnGetCanReviewResultCallback);

        public void OnRequestReviewButtonClicked() => Feedback.RequestReview(OnGetRequestReviewResultCallback);

        private void OnGetCanReviewResultCallback(bool canReview, string reason) =>
            _resultText.text = canReview 
                ? $"Can review: <color=\"green\">True</color>\nReason: <color=\"green\">{reason}</color>"
                : $"Can review: <color=\"red\">False</color>\nReason: <color=\"red\">{reason}</color>";

        private void OnGetRequestReviewResultCallback(bool isFeedbackSent) =>
            _resultText.text = isFeedbackSent 
                ? "Feedback sent: <color=\"green\">True</color>"
                : "Feedback sent: <color=\"red\">False</color>";
    }
}