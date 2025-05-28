using UnityEngine;

namespace VervePlace.YandexGames.Samples
{
    public class BillingGroup : MonoBehaviour
    {
        [SerializeField] private ProductsPanel _productsPanel;
        
        public void OnGetProductsCatalogButtonClicked() => 
            Billing.GetProductsCatalog(OnGetProductsCatalogSuccessCallback, OnGetProductsErrorCallback);

        public void OnGetPurchasedProductsButtonClicked() =>
            Billing.GetPurchasedProducts(OnGetPurchasedProductsSuccessCallback, OnGetProductsErrorCallback);
        
        private void OnGetProductsCatalogSuccessCallback(ProductsCatalogResponse response) => 
            _productsPanel.Show(response);

        private void OnGetPurchasedProductsSuccessCallback(PurchasedProductsResponse response) => 
            _productsPanel.Show(response);

        private void OnGetProductsErrorCallback(string error) => 
            _productsPanel.ShowError(error);
    }
}