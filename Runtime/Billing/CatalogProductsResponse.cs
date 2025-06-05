using System;
using UnityEngine.Scripting;

namespace VervePlace.YandexGames
{
    [Serializable]
    public class CatalogProductsResponse
    {
        [field: Preserve]
        public CatalogProduct[] products;
    }
}