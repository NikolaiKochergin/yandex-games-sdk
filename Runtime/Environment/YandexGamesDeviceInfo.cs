using UnityEngine.Scripting;

namespace VervePlace.YandexGames
{
    public class YandexGamesDeviceInfo
    {
        [field: Preserve] 
        public string deviceType;
        
        [field: Preserve] 
        public bool isMobile;
        
        [field: Preserve] 
        public bool isTablet;
        
        [field: Preserve] 
        public bool isDesktop;
        
        [field: Preserve] 
        public bool isTV;
    }
}