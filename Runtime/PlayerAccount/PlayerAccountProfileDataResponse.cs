using System;
using UnityEngine.Scripting;

namespace VervePlace.YandexGames
{
    [Serializable]
    public class PlayerAccountProfileDataResponse
    {
        [field: Preserve]
        public string uniqueID;
        [field: Preserve]
        public string name;
        [field: Preserve]
        public string profilePicture;
        [field: Preserve] 
        public string payingStatus;
        [field: Preserve]
        public UserIDsPreGame[] userIDsPerGame;
        [field: Preserve] 
        public string signature;
        
        [Serializable]
        public class UserIDsPreGame
        {
            [field: Preserve]
            public int appID;
            [field: Preserve]
            public string userID;
        }
    }
}