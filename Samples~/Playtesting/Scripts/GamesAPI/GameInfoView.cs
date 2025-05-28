using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VervePlace.YandexGames.Samples
{
    public class GameInfoView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _title;
        [SerializeField] private RawImage _icon;
        
        private GameInfo _info;

        public void Setup(GameInfo info)
        {
            _info = info;
            _title.text = _info.title;
            StartCoroutine(new RemoteImage(_info.iconURL).Download(texture =>
            {
                _icon.texture = texture;
                _icon.gameObject.SetActive(true);
            }));
        }

        public void OnOpenGameButtonClicked() => GamesAPI.OpenLink(_info.url);
    }
}