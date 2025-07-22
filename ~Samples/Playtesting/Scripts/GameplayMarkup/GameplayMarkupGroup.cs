using TMPro;
using UnityEngine;

namespace VervePlace.YandexGames.Samples
{
    public class GameplayMarkupGroup : MonoBehaviour
    {
        [SerializeField] private TMP_Text _pauseStatusText;
        [SerializeField] private TMP_Text _musicButtonText;
        [SerializeField] private AudioSource _audioSource;
        
        private void Awake() => 
            GameplayMarkup.GamePaused += OnGamePaused;

        private void OnDestroy() => 
            GameplayMarkup.GamePaused -= OnGamePaused;

        public void OnGameStartButtonClicked() => GameplayMarkup.GameStart();
        public void OnGameStopButtonClicked() => GameplayMarkup.GameStop();

        public void OnMusicButtonClicked()
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
                _musicButtonText.text = "Play Music";
            }
            else
            {
                _audioSource.Play();
                _musicButtonText.text = "Stop Music";
            }
        }
        
        private void OnGamePaused(bool isPaused)
        {
            _pauseStatusText.text = isPaused
                ? "<color=\"red\">Paused"
                : "<color=\"green\">Playing";
            
            if (isPaused)
                _audioSource.Pause();
            else
                _audioSource.UnPause();
        }
    }
}