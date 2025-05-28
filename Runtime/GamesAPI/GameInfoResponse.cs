using System;
using UnityEngine.Scripting;

namespace VervePlace.YandexGames
{
    [Serializable]
    public class GameInfoResponse
    {
        [field: Preserve] 
        public GameInfo game;

        [field: Preserve] 
        public bool isAvailable;
    }
}