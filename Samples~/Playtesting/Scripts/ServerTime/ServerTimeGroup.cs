using TMPro;
using UnityEngine;

namespace VervePlace.YandexGames.Samples
{
    public class ServerTimeGroup : MonoBehaviour
    {
        [SerializeField] private TMP_Text _serverTimeText;

        public void OnGetServerTimeButtonClicked()
        {
            _serverTimeText.text = $"Milliseconds: {ServerTime.Milliseconds}";
            _serverTimeText.text += $"\nDate: {ServerTime.Date}";
        }
    }
}