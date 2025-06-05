#if UNITY_LOCALIZATION
using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace VervePlace.YandexGames
{
    [Serializable]
    public class YandexGamesLocaleSelector : IStartupLocaleSelector, IInitialize
    {
        private const string LocaleKey = "selected-locale";

        [SerializeField] private LocaleIdentifier _defaultLocal = new(SystemLanguage.English);
        [SerializeField] private bool _isLocaleSaveToPrefs;

#if !UNITY_EDITOR
        public Locale GetStartupLocale(ILocalesProvider availableLocales) =>
            availableLocales
                .GetLocale(new LocaleIdentifier(_isLocaleSaveToPrefs
                    ? PlayerPrefs.GetString(LocaleKey, YandexGamesSDK.Environment.i18n.lang)
                    : YandexGamesSDK.Environment.i18n.lang));
#else
            public Locale GetStartupLocale(ILocalesProvider availableLocales) =>
                availableLocales
                    .GetLocale(new LocaleIdentifier(_isLocaleSaveToPrefs
                        ? PlayerPrefs.GetString(LocaleKey, _defaultLocal.Code)
                        : _defaultLocal.Code));
#endif
        public void PostInitialization(LocalizationSettings settings)
        {
            Locale selectedLocale = settings.GetSelectedLocale();
            if (selectedLocale)
                PlayerPrefs.SetString(LocaleKey, selectedLocale.Identifier.Code);
        }
    }
}
#endif
