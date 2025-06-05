using System;
using UnityEngine.Scripting;

namespace VervePlace.YandexGames
{
    [Serializable]
    public class PurchasedProductsResponse
    {
        [field: Preserve]
        public PurchasedProduct[] products;
        [field: Preserve]
        public string signature;
    }
}