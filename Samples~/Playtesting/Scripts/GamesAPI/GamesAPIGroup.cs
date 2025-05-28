using TMPro;
using UnityEngine;

namespace VervePlace.YandexGames.Samples
{
    public class GamesAPIGroup : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _gameIdInputField;
        [SerializeField] private TMP_Text _responseText;
        [SerializeField] private GameCatalog _gameCatalog;

        private void Awake() => 
            _gameCatalog.OnCloseButtonClicked();

        public void OnGetAllGamesButtonClicked() => 
            GamesAPI.GetAllGames(OnGetAllGamesResponseCallback, OnGetErrorCallback);

        public void OnGetGameByIdButtonClicked()
        {
            if (int.TryParse(_gameIdInputField.text, out int gameId))
                GamesAPI.GetGameByID(gameId, OnGetGameByIdResponseCallback, OnGetErrorCallback);
            else
                _gameIdInputField.text = "Please enter \na valid game id";
        }

        public void OnOpenGameCatalogButtonClicked() => 
            GamesAPI.GetAllGames(gamesInfo => _gameCatalog.Display(gamesInfo));

        private void OnGetAllGamesResponseCallback(AllGameInfosResponse response) => 
            _responseText.text = JsonUtility.ToJson(response, true);

        private void OnGetGameByIdResponseCallback(GameInfoResponse response) => 
            _responseText.text = JsonUtility.ToJson(response, true);

        private void OnGetErrorCallback(string error) => 
            _responseText.text = $"Error: <color=\"red\">{error}</color>";
    }
}