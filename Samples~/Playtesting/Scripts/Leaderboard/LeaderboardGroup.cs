using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VervePlace.YandexGames.Samples
{
    public class LeaderboardGroup : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _leaderboardDropdown;
        [SerializeField] private TMP_Text _leaderboardDescriptionText;
        [SerializeField] private Toggle _includeSelfToggle;
        [SerializeField] private TMP_InputField _leaderboardNameInputField;
        [SerializeField] private TMP_InputField _topPlayersCountInputField;
        [SerializeField] private TMP_InputField _competingPlayersCountInputField;
        
        public void OnGetLeaderboardDescriptionButtonClicked() => 
            Leaderboard
                .GetDescription(_leaderboardDropdown.options[_leaderboardDropdown.value].text, 
                    OnGetLeaderboardDescriptionSuccessCallback,
                    OnGetLeaderboardErrorCallback);

        public void OnSetLeaderboardScoreButtonClicked() => 
            Leaderboard.SetScore(_leaderboardDropdown.options[_leaderboardDropdown.value].text, Random.Range(0, 100));

        public void OnGetLeaderboardPlayerEntryButtonClicked() =>
            Leaderboard.GetPlayerEntry(_leaderboardDropdown.options[_leaderboardDropdown.value].text, 
                OnGetLeaderboardPlayerEntrySuccessCallback,
                OnGetLeaderboardErrorCallback);

        public void OnGetLeaderboardEntriesButtonClicked() =>
            Leaderboard.GetEntries(
                leaderboardName: _leaderboardNameInputField.text.Length > 0 
                    ? _leaderboardNameInputField.text 
                    : _leaderboardDropdown.options[_leaderboardDropdown.value].text,
                OnGetLeaderboardEntriesSuccessCallback,
                OnGetLeaderboardErrorCallback,
                topPlayersCount: int.TryParse(_topPlayersCountInputField.text, out int topCount)
                    ? topCount
                    : 5,
                competingPlayersCount: int.TryParse(_competingPlayersCountInputField.text, out int competingCount)
                    ? competingCount
                    : 5,
                includeSelf: _includeSelfToggle.isOn);
        
        private void OnGetLeaderboardDescriptionSuccessCallback(LeaderboardDescriptionResponse response) => 
            _leaderboardDescriptionText.text = JsonUtility.ToJson(response, true);

        private void OnGetLeaderboardPlayerEntrySuccessCallback(LeaderboardEntryResponse response) => 
            _leaderboardDescriptionText.text = JsonUtility.ToJson(response, true);
        
        private void OnGetLeaderboardEntriesSuccessCallback(LeaderboardGetEntriesResponse response) => 
            _leaderboardDescriptionText.text = JsonUtility.ToJson(response, true);

        private void OnGetLeaderboardErrorCallback(string error) => 
            _leaderboardDescriptionText.text = "<color=\"red\">" + error + "</color>";
    }
}