using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace VervePlace.YandexGames.Samples
{
    public class ProductsPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _responseText;
        [SerializeField] private CatalogProductView _catalogProductPrefab;
        [SerializeField] private PurchasedProductView _purchasedProductPrefab;
        [SerializeField] private Transform _purchasesContainer;

        private readonly List<GameObject> _cachedViews = new();

        public void Show(CatalogProductsResponse response)
        {
            _titleText.text = "Products Catalog";
            _responseText.text = JsonUtility.ToJson(response, true);
            gameObject.SetActive(true);

            foreach (CatalogProduct product in response.products)
            {
                CatalogProductView view = Instantiate(_catalogProductPrefab, _purchasesContainer);
                view.Setup(product, OnBuyButtonClick);
                _cachedViews.Add(view.gameObject);
            }
        }

        public void Show(PurchasedProductsResponse response)
        {
            _titleText.text = "Purchased Products";
            _responseText.text = JsonUtility.ToJson(response, true);
            gameObject.SetActive(true);

            foreach (PurchasedProduct product in response.products)
            {
                PurchasedProductView view = Instantiate(_purchasedProductPrefab, _purchasesContainer);
                view.Setup(product, OnConsumeButtonClick);
                _cachedViews.Add(view.gameObject);
            }
        }

        public void ShowError(string error)
        {
            _titleText.text = "<color=\"red\">Error";
            _responseText.text = "<color=\"red\">" + error + "</color>";
            gameObject.SetActive(true);
        }

        public void Close()
        {
            foreach (GameObject product in _cachedViews) 
                Destroy(product);
            
            _cachedViews.Clear();
            gameObject.SetActive(false);
        }

        private void OnBuyButtonClick(CatalogProduct product) =>
            Billing.PurchaseProduct(product.id, OnPurchaseProductSuccess, ShowProductError, ServerTime.Date.ToString(CultureInfo.InvariantCulture));

        private void OnConsumeButtonClick(PurchasedProduct product) => 
            Billing.ConsumeProduct(product.purchaseToken, OnConsumeSuccess, ShowProductError);

        private void OnConsumeSuccess() => 
            _responseText.text = "<color=green>Purchase consumed successfully</color>";

        private void OnPurchaseProductSuccess(PurchasedProduct response) => 
            _responseText.text = JsonUtility.ToJson(response, true);
        
        private void ShowProductError(string error) => 
            _responseText.text = "<color=\"red\">" + error + "</color>";
    }
}