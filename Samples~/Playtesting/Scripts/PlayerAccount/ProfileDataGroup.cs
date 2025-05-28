using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VervePlace.YandexGames.Samples
{
    public class ProfileDataGroup : MonoBehaviour
    {
        [SerializeField] private Image _profileImage;
        [SerializeField] private TMP_Text _profileInfoText;
        
        public void OnGetProfileDataButtonClicked() => PlayerAccount.GetProfileData(OnGetProfileDataSuccessCallback, OnGetProfileDataErrorCallback);
        
        private void OnGetProfileDataSuccessCallback(PlayerAccountProfileDataResponse response)
        {
            RemoteImage image = new(response.profilePicture);
            StartCoroutine(image.Download(texture =>
            {
                _profileImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                _profileImage.color = Color.white;
            }));
            StringBuilder builder = new();
            builder.Append("<color=\"green\">Unique ID: </color>" + response.uniqueID + "\n");
            builder.Append("<color=\"green\">Name: </color>" + response.name + "\n");
            builder.Append("<color=\"green\">Paying Status: </color>" + response.payingStatus + "\n");
            builder.Append("\n<color=\"green\">User IDs Per Game </color>" + "\n");
            foreach (PlayerAccountProfileDataResponse.UserIDsPreGame id in response.userIDsPerGame) 
                builder.Append("<color=\"orange\">App ID: </color>" + id.appID + "<color=\"orange\"> User ID: </color>" + id.userID + "\n");
            
            _profileInfoText.text = builder.ToString();
        }
        
        private void OnGetProfileDataErrorCallback(string error) => 
            _profileInfoText.text = "<color=\"red\">Error:\n" + error;
    }
}