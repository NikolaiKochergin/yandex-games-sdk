using System.Collections;
using TMPro;
using UnityEngine;

namespace VervePlace.YandexGames.Samples
{
    public class AuthorizationGroup : MonoBehaviour
    {
        [SerializeField] private TMP_Text _authorizationStatusText;
        
        private void Awake()
        {
            PlayerAccount.Authorized += OnAuthorized;
            PlayerAccount.AuthorizationDialogOpened += OnAuthorizationDialogOpened;
            PlayerAccount.AuthorizationDialogClosed += OnAuthorizationDialogClosed;
        }

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => YandexGamesSDK.IsInitialized);
            OnAuthorized();
        }

        private void OnDestroy()
        {
            PlayerAccount.Authorized -= OnAuthorized;
            PlayerAccount.AuthorizationDialogOpened -= OnAuthorizationDialogOpened;
            PlayerAccount.AuthorizationDialogClosed -= OnAuthorizationDialogClosed;
        }

        public void OnAuthorizeButtonClicked() => PlayerAccount.Authorize();

        private void OnAuthorized() =>
            _authorizationStatusText.text = PlayerAccount.IsAuthorized
                ? "<color=\"green\">Authorized"
                : "<color=\"red\">Not authorized";

        private void OnAuthorizationDialogOpened() => 
            _authorizationStatusText.text = "<color=\"orange\">Authorization dialog opened";

        private void OnAuthorizationDialogClosed() => 
            _authorizationStatusText.text = "<color=\"orange\">Authorization dialog closed";
    }
}