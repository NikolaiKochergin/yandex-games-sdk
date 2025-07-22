using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace VervePlace.YandexGames
{
    public static class Billing
    {
        #region WarmupBilling

        private static Action s_onWarmupSuccessCallback;
        
        public static void Warmup(Action onSuccessCallback = null)
        {
            s_onWarmupSuccessCallback = onSuccessCallback;
            
            WarmupBilling(OnWarmupSuccessCallback);
        }

        [DllImport("__Internal")]
        private static extern void WarmupBilling(Action successCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnWarmupSuccessCallback()
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(Billing)}.{nameof(OnWarmupSuccessCallback)} invoked");
            
            s_onWarmupSuccessCallback?.Invoke();
        }

        #endregion
        
        
        #region PurchaseProduct
        private static Action<PurchasedProduct> s_onPurchaseProductSuccessCallback;
        private static Action<string> s_onPurchaseProductErrorCallback;
        
        public static void PurchaseProduct(string productId, Action<PurchasedProduct> onSuccessCallback = null, Action<string> onErrorCallback = null, string developerPayload = "")
        {
            s_onPurchaseProductSuccessCallback = onSuccessCallback;
            s_onPurchaseProductErrorCallback = onErrorCallback;

            BillingPurchaseProduct(productId, OnPurchaseProductSuccessCallback, OnPurchaseProductErrorCallback, developerPayload);
        }

        [DllImport("__Internal")]
        private static extern void BillingPurchaseProduct(string productId, Action<string> successCallback, Action<string> errorCallback, string developerPayload);

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnPurchaseProductSuccessCallback(string purchaseProductResponseJson)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(Billing)}.{nameof(OnPurchaseProductSuccessCallback)} invoked, {nameof(purchaseProductResponseJson)} = {purchaseProductResponseJson}");

            PurchasedProduct response = JsonUtility.FromJson<PurchasedProduct>(purchaseProductResponseJson);

            s_onPurchaseProductSuccessCallback?.Invoke(response);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnPurchaseProductErrorCallback(string errorMessage)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(Billing)}.{nameof(OnPurchaseProductErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onPurchaseProductErrorCallback?.Invoke(errorMessage);
        }
        #endregion
        
        #region GetPurchasedProducts
        private static Action<PurchasedProductsResponse> s_onGetPurchasedProductsSuccessCallback;
        private static Action<string> s_onGetPurchasedProductsErrorCallback;
        
        public static void GetPurchasedProducts(Action<PurchasedProductsResponse> onSuccessCallback, Action<string> onErrorCallback = null)
        {
            s_onGetPurchasedProductsSuccessCallback = onSuccessCallback;
            s_onGetPurchasedProductsErrorCallback = onErrorCallback;

            BillingGetPurchasedProducts(OnGetPurchasedProductsSuccessCallback, OnGetPurchasedProductsErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern void BillingGetPurchasedProducts(Action<string> successCallback, Action<string> errorCallback);

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetPurchasedProductsSuccessCallback(string purchasedProductsResponseJson)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(Billing)}.{nameof(OnGetPurchasedProductsSuccessCallback)} invoked, {nameof(purchasedProductsResponseJson)} = {purchasedProductsResponseJson}");

            PurchasedProductsResponse response = JsonUtility.FromJson<PurchasedProductsResponse>(purchasedProductsResponseJson);

            s_onGetPurchasedProductsSuccessCallback?.Invoke(response);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetPurchasedProductsErrorCallback(string errorMessage)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(Billing)}.{nameof(OnGetPurchasedProductsErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onGetPurchasedProductsErrorCallback?.Invoke(errorMessage);
        }
        #endregion
        
        #region GetCatalogProducts
        private static Action<CatalogProductsResponse> s_onGetCatalogProductsSuccessCallback;
        private static Action<string> s_onGetProductsCatalogErrorCallback;
        
        public static void GetProductsCatalog(Action<CatalogProductsResponse> onSuccessCallback, Action<string> onErrorCallback = null, CurrencyPictureSize currencyPictureSize = CurrencyPictureSize.medium)
        {
            s_onGetCatalogProductsSuccessCallback = onSuccessCallback;
            s_onGetProductsCatalogErrorCallback = onErrorCallback;

            BillingGetCatalogProducts(OnGetCatalogProductsSuccessCallback, OnGetCatalogProductsErrorCallback, currencyPictureSize.ToString());
        }

        [DllImport("__Internal")]
        private static extern void BillingGetCatalogProducts(Action<string> successCallback, Action<string> errorCallback, string currencyPictureSize);

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetCatalogProductsSuccessCallback(string productsCatalogResponseJson)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(Billing)}.{nameof(OnGetCatalogProductsSuccessCallback)} invoked, {nameof(productsCatalogResponseJson)} = {productsCatalogResponseJson}");

            CatalogProductsResponse response = JsonUtility.FromJson<CatalogProductsResponse>(productsCatalogResponseJson);

            s_onGetCatalogProductsSuccessCallback?.Invoke(response);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetCatalogProductsErrorCallback(string errorMessage)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(Billing)}.{nameof(OnGetCatalogProductsErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onGetProductsCatalogErrorCallback?.Invoke(errorMessage);
        }
        #endregion
        
        #region ConsumeProduct
        private static Action s_onConsumeProductSuccessCallback;
        private static Action<string> s_onConsumeProductErrorCallback;
        
        public static void ConsumeProduct(string purchasedProductToken, Action onSuccessCallback = null, Action<string> onErrorCallback = null)
        {
            s_onConsumeProductSuccessCallback = onSuccessCallback;
            s_onConsumeProductErrorCallback = onErrorCallback;

            BillingConsumeProduct(purchasedProductToken, OnConsumeProductSuccessCallback, OnConsumeProductErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern void BillingConsumeProduct(string purchasedProductToken, Action successCallback, Action<string> errorCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnConsumeProductSuccessCallback()
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(Billing)}.{nameof(OnConsumeProductSuccessCallback)} invoked");

            s_onConsumeProductSuccessCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnConsumeProductErrorCallback(string errorMessage)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(Billing)}.{nameof(OnConsumeProductErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onConsumeProductErrorCallback?.Invoke(errorMessage);
        }
        #endregion
    }
}