using TMPro;
using UnityEngine;

namespace VervePlace.YandexGames.Samples
{
    public class ShortcutGroup : MonoBehaviour
    {
        [SerializeField] private TMP_Text _resultText;

        public void OnCanShowPromptButtonClicked() => Shortcut.CanShowPrompt(OnGetCanShowPromptResultCallback);

        public void OnShowPromptButtonClicked() => Shortcut.ShowPrompt(OnGetShowPromptResultCallback);

        private void OnGetCanShowPromptResultCallback(bool canShow) =>
            _resultText.text = canShow 
                ? "Can show: <color=\"green\">True</color>"
                : "Can show: <color=\"red\">False</color>";

        private void OnGetShowPromptResultCallback(bool isShortcutCreated) =>
            _resultText.text = isShortcutCreated 
                ? "Shortcut created: <color=\"green\">True</color>"
                : "Shortcut created: <color=\"red\">False</color>";
    }
}