using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

namespace VervePlace.YandexGames.Samples
{
    public class PlaytestCanvas : MonoBehaviour
    {
        [SerializeField] private TMP_Text _initializationStatusText;

        private void Awake() => 
            YandexGamesSDK.CallbackLogging = true;

        private IEnumerator Start()
        {
            _initializationStatusText.text = "SDK Initialization Status: <color=\"red\">Not Initialized";
            
            yield return YandexGamesSDK.Initialize();
            
            YandexGamesEnvironment environment = YandexGamesSDK.Environment;
            YandexGamesDeviceInfo deviceInfo = YandexGamesSDK.DeviceInfo;
            StringBuilder builder = new();
            builder.Append("SDK Initialization Status: <color=\"green\">Initialized</color>\n");
            builder.Append($"\n<color=\"green\">App Id: </color>{environment.app.id}");
            builder.Append($"\n<color=\"green\">Browser lang: </color>{environment.browser.lang}");
            builder.Append($"\n<color=\"green\">i18n lang: </color>{environment.i18n.lang}");
            builder.Append($"\n<color=\"green\">i18n tld: </color>{environment.i18n.tld}");
            builder.Append($"\n<color=\"green\">Payload: </color>{environment.payload}\n");
            builder.Append($"\n<color=\"green\">Device Type: </color>{deviceInfo.deviceType}");
            builder.Append($"\n<color=\"green\">Is Mobile: </color>{deviceInfo.isMobile}");
            builder.Append($"\n<color=\"green\">Is Tablet: </color>{deviceInfo.isTablet}");
            builder.Append($"\n<color=\"green\">Is Desktop: </color>{deviceInfo.isDesktop}");
            builder.Append($"\n<color=\"green\">Is TV: </color>{deviceInfo.isTV}");
            _initializationStatusText.text = builder.ToString();
            
            GameplayMarkup.GameReady();
        }
    }
}