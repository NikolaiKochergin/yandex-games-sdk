using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VervePlace.YandexGames.Samples
{
    public class PurchasedProductView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _purchasedProductInfo;
        [SerializeField] private Button _consumeButton;
        
        private PurchasedProduct _product;

        public void Setup(PurchasedProduct product, Action<PurchasedProduct> onConsumeButtonClick)
        {
            _product = product;
            _purchasedProductInfo.text = $"<b>{product.productID}</b> {product.developerPayload}\n{product.purchaseToken}\n{product.signature}";
            _consumeButton.onClick.AddListener(() => onConsumeButtonClick(_product));
        }
    }
}