using TMPro;
using UnityEngine;

namespace VervePlace.YandexGames.Samples
{
    public class MetricaGroup : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _eventNameInputField;
        [SerializeField] private TMP_InputField _eventDataJsonInputField;

        private void Awake() => 
            YandexMetrica.CallbackLogging = true;

        public void OnSendEventButtonClicked() => 
            YandexMetrica.Send(_eventNameInputField.text, _eventDataJsonInputField.text);
    }
}
