using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VervePlace.YandexGames.Samples
{
    public class CloudDataGroup : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _setDataInputField;
        [SerializeField] private TMP_Text _getDataText;
        [SerializeField] private Toggle _flushToggle;
        
        [SerializeField] private TMP_InputField _setKeysDataInputField;
        [SerializeField] private TMP_Text _getKeysDataText;
        
        public void OnSetDataButtonClicked() => PlayerAccount.SetCloudSaveData(_setDataInputField.text, _flushToggle.isOn, OnSetCloudSaveDataSuccess, OnGetCloudSaveDataError);
        public void OnGetDataButtonClicked() => PlayerAccount.GetCloudSaveData(dataJson => _getDataText.text = dataJson, OnGetCloudSaveDataError);

        public void OnGetKeysDataButtonClicked()
        {
            string[] keys = string.IsNullOrEmpty(_setKeysDataInputField.text) ? null : _setKeysDataInputField.text.Split(' ');
            PlayerAccount.GetKeysCloudSaveData(keys, OnGetKeysDataSuccess, OnGetKeysDataError);
        }

        private void OnGetKeysDataSuccess(IReadOnlyDictionary<string, string> keysData)
        {
            _getKeysDataText.text = "";
            foreach (KeyValuePair<string, string> keyData in keysData) 
                _getKeysDataText.text += $"<color=\"green\">{keyData.Key}</color>: {keyData.Value}\n";
        }

        private void OnGetKeysDataError(string error) => 
            _getKeysDataText.text = "<color=\"red\">" + error + "</color>";

        private void OnGetCloudSaveDataError(string error) => 
            _getDataText.text = "<color=\"red\">" + error + "</color>";

        private void OnSetCloudSaveDataSuccess() => 
            _getDataText.text = "<color=\"green\">Cloud data successfully set</color>";
    }
}