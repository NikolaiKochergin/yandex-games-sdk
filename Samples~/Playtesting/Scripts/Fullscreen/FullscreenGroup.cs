using TMPro;
using UnityEngine;

namespace VervePlace.YandexGames.Samples
{
    public class FullscreenGroup : MonoBehaviour
    {
        [SerializeField] private TMP_Text _fullscreenStatusText;

        public void OnGetFullscreenStatusButtonClicked() => 
            _fullscreenStatusText.text = $"Fullscreen: {Fullscreen.IsFullscreen}";

        public void OnChangeFullscreenButtonClicked() => 
            Fullscreen.SetFullscreen(!Fullscreen.IsFullscreen, OnSuccessCallback, OnErrorCallback);

        private void OnSuccessCallback(bool isFullscreen) => 
            _fullscreenStatusText.text = $"Is Fullscreen: {isFullscreen}";

        private void OnErrorCallback(string error) => 
            _fullscreenStatusText.text = $"Error: <color=\"red\">{error}</color>";
    }
}