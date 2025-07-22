using TMPro;
using UnityEngine;

namespace VervePlace.YandexGames.Samples
{
    public class ClipboardGroup : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _clipboardInputField;
        
        public void OnCopyToClipboardButtonClick() => 
            Clipboard.Write(_clipboardInputField.text);
    }
}