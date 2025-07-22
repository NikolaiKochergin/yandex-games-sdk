using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace VervePlace.YandexGames.Samples
{
    public class RemoteConfigGroup : MonoBehaviour
    {
        [SerializeField] private TMP_Text _remoteFlagsInfoText;
        [SerializeField] private TMP_InputField _defaultFlagsInputField;
        [SerializeField] private TMP_InputField _clientFeaturesInputField;
        
        public void OnGetRemoteConfigurationFlagsButtonClicked()
        {
            Dictionary<string, string> defaultFlags = new();
            Dictionary<string, string> clientFeatures = new();

            ParseInputStringToDictionary(_defaultFlagsInputField.text, defaultFlags);
            ParseInputStringToDictionary(_clientFeaturesInputField.text, clientFeatures);

            if (defaultFlags.Count == 0)
                defaultFlags = null;

            if (clientFeatures.Count == 0)
                clientFeatures = null;
            
            RemoteConfig.GetFlags(OnGetRemoteConfigurationFlagsSuccessCallback, defaultFlags, clientFeatures, OnGetRemoteConfigurationFlagsErrorCallback);
            return;
            
            void ParseInputStringToDictionary(string inputString, Dictionary<string, string> dictionary)
            {
                string[] pairs = inputString.Split('\n');
                foreach (string pair in pairs)
                {
                    string[] split = pair.Replace(" ", "").Split('=');
                    if(split.Length == 2)
                        dictionary.Add(split[0].Trim(), split[1].Trim());
                }
            }
        }
        
        private void OnGetRemoteConfigurationFlagsSuccessCallback(IReadOnlyDictionary<string, string> response)
        {
            StringBuilder builder = new();
            foreach (KeyValuePair<string, string> pair in response)
                builder.Append("<color=\"green\">Key: </color>" + pair.Key + "; <color=\"green\">Value: </color>" + pair.Value + "\n");
            
            _remoteFlagsInfoText.text = builder.ToString();
        }

        private void OnGetRemoteConfigurationFlagsErrorCallback(string error) => 
            _remoteFlagsInfoText.text = "<color=\"red\">" + error + "</color>";
    }
}