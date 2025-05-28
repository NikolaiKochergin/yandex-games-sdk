using TMPro;
using UnityEngine;

namespace VervePlace.YandexGames.Samples
{
    public class GameplayMarkupGroup : MonoBehaviour
    {
        [SerializeField] private TMP_Text _pauseStatusText;
        
        private void Awake() => 
            GameplayMarkup.GamePaused += OnGamePaused;

        private void OnDestroy() => 
            GameplayMarkup.GamePaused -= OnGamePaused;

        public void OnGameStartButtonClicked() => GameplayMarkup.GameStart();
        public void OnGameStopButtonClicked() => GameplayMarkup.GameStop();
        
        private void OnGamePaused(bool isPaused) =>
            _pauseStatusText.text = isPaused
                ? "<color=\"red\">Paused"
                : "<color=\"green\">Playing";
    }
}