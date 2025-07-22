using TMPro;
using UnityEngine;

namespace VervePlace.YandexGames.Samples
{
    public class BillingGroup : MonoBehaviour
    {
        [SerializeField] private ProductsPanel _productsPanel;
        [SerializeField] private TMP_Text _warmupText;

        public void OnWarmupButtonClick() =>
            Billing.Warmup(OnWarmupSuccessCallback);

        public void OnGetProductsCatalogButtonClicked() => 
            Billing.GetProductsCatalog(OnGetProductsCatalogSuccessCallback, OnGetProductsErrorCallback);

        public void OnGetPurchasedProductsButtonClicked() =>
            Billing.GetPurchasedProducts(OnGetPurchasedProductsSuccessCallback, OnGetProductsErrorCallback);

        private void OnWarmupSuccessCallback() => 
            _warmupText.text = "<color=green>Warmup Success";

        private void OnGetProductsCatalogSuccessCallback(CatalogProductsResponse response) => 
            _productsPanel.Show(response);

        private void OnGetPurchasedProductsSuccessCallback(PurchasedProductsResponse response) => 
            _productsPanel.Show(response);

        private void OnGetProductsErrorCallback(string error) => 
            _productsPanel.ShowError(error);
    }
}