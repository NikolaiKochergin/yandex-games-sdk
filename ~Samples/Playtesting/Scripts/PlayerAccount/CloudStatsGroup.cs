using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace VervePlace.YandexGames.Samples
{
    public class CloudStatsGroup : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _setStatsInputField;
        [SerializeField] private TMP_InputField _setKeysStatsInputField;
        [SerializeField] private TMP_Text _getStatsText;

        public void OnSetStatsButtonClicked()
        {
            Dictionary<string, int> stats = new();
            
            string[] pairs = _setStatsInputField.text.Split(' ');
            foreach (string pair in pairs)
            {
                string[] stat = pair.Split('=');
                if(stat.Length == 2)
                    stats.Add(stat[0], int.Parse(stat[1]));
            }
            
            PlayerAccount.SetStats(stats, OnSetStatsSuccess, OnGetStatsError);
        }

        public void OnIncrementStatsButtonClicked()
        {
            Dictionary<string, int> stats = new();
            
            string[] pairs = _setStatsInputField.text.Split(' ');
            foreach (string pair in pairs)
            {
                string[] stat = pair.Split('=');
                if(stat.Length == 2)
                    stats.Add(stat[0], int.Parse(stat[1]));
            }
            
            PlayerAccount.IncrementStats(stats, OnGetKeyStatsSuccess, OnGetStatsError);
        }

        public void OnGetKeyStatsButtonClicked()
        {
            string[] stats = string.IsNullOrEmpty(_setKeysStatsInputField.text) ? null : _setKeysStatsInputField.text.Split(' ');
            PlayerAccount.GetStats(stats, OnGetKeyStatsSuccess, OnGetStatsError);
        }

        private void OnGetKeyStatsSuccess(IReadOnlyDictionary<string, int> keysData)
        {
            _getStatsText.text = "";
            foreach (KeyValuePair<string, int> keyData in keysData) 
                _getStatsText.text += $"<color=\"green\">{keyData.Key}</color>: {keyData.Value}\n";
        }

        private void OnSetStatsSuccess() => 
            _getStatsText.text = "<color=\"green\">Cloud stats successfully set</color>";
        
        private void OnGetStatsError(string error) => 
            _getStatsText.text = "<color=\"red\">" + error + "</color>";
    }
}