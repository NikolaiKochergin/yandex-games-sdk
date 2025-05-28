using System;
using UnityEngine.Scripting;

namespace VervePlace.YandexGames
{
    [Serializable]
    public class ProductsCatalogResponse
    {
        [field: Preserve]
        public CatalogProduct[] products;
    }
}