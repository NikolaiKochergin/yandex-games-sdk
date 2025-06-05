using System;
using UnityEngine.Scripting;

namespace VervePlace.YandexGames
{
    [Serializable]
    public class PurchasedProduct
    {
        [field: Preserve]
        public string productID;

        [field: Preserve]
        public string purchaseToken;

        [field: Preserve]
        public string developerPayload;
        
        [field: Preserve]
        public string signature;
    }
}