using TMPro;
using UnityEngine;

namespace VervePlace.YandexGames.Samples.Advertising
{
    public class AdvertisingGroup : MonoBehaviour
    {
        [SerializeField] private TMP_Text _advertisingInfoText;
        
        public void OnInterstitialAdButtonClicked() => InterstitialAd.Show(wasShownOnCloseCallback: WasShownOnCloseCallback, onErrorCallback: OnErrorCallback);

        public void OnRewardedAdButtonClicked() => RewardedAd.Show(onRewardedCallback: OnRewardedCallback, onErrorCallback: OnErrorCallback);

        public void OnGetStickyAdStatusButtonClicked() => StickyAd.GetStatus(OnGetStickyAdStatusCallback);

        public void OnStickyAdShowButtonClicked() => StickyAd.Show();

        public void OnStickyAdHideButtonClicked() => StickyAd.Hide();

        private void WasShownOnCloseCallback(bool wasShown) => 
            _advertisingInfoText.text = $"Interstitial wasShown: {wasShown}";

        private void OnRewardedCallback() => 
            _advertisingInfoText.text = "Reward has been received";

        private void OnGetStickyAdStatusCallback(bool isStickyAdShowing, string reason) => 
            _advertisingInfoText.text = $"Sticky ad showing: {isStickyAdShowing}, reason: {reason}";

        private void OnErrorCallback(string errorMessage) => 
            _advertisingInfoText.text = $"<color=\"red\">{errorMessage}</color>";
    }
}