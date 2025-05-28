using System;
using UnityEngine.Scripting;

namespace VervePlace.YandexGames
{
    [Serializable]
    public class GameInfo
    {
        [field: Preserve] 
        public int appID;

        [field: Preserve] 
        public string title;
        
        [field: Preserve] 
        public string url;
        
        [field: Preserve] 
        public string coverURL;
        
        [field: Preserve] 
        public string iconURL;
    }
}