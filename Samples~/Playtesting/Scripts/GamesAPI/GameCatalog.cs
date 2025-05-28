using System.Collections.Generic;
using UnityEngine;

namespace VervePlace.YandexGames.Samples
{
    public class GameCatalog : MonoBehaviour
    {
        [SerializeField] private GameInfoView _gameInfoViewPrefab;
        [SerializeField] private Transform _container;
        
        private AllGameInfosResponse _gamesInfo;
        private readonly List<GameInfoView> _cachedViews = new();

        public void Display(AllGameInfosResponse gamesInfo)
        {
            _gamesInfo = gamesInfo;
            gameObject.SetActive(true);

            foreach (GameInfo info in gamesInfo.games)
            {
                GameInfoView view = Instantiate(_gameInfoViewPrefab, _container);
                view.Setup(info);
                _cachedViews.Add(view);
            }
        }
        
        public void OnOpenDeveloperPageButtonClicked() => 
            GamesAPI.OpenLink(_gamesInfo.developerURL);

        public void OnCloseButtonClicked()
        {
            foreach (GameInfoView view in _cachedViews) 
                Destroy(view.gameObject);
            
            _cachedViews.Clear();
            gameObject.SetActive(false);
        }
    }
}