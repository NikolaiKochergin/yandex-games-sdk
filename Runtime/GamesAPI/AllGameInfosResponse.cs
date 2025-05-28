using System;
using UnityEngine.Scripting;

namespace VervePlace.YandexGames
{
    [Serializable]
    public class AllGameInfosResponse
    {
        [field: Preserve]
        public GameInfo[] games;
        
        [field: Preserve]
        public string developerURL;
    }
}