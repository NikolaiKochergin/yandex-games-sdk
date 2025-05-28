using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace VervePlace.YandexGames
{
    public static class Billing
    {
        #region PurchaseProduct
        private static Action<PurchaseProductResponse> s_onPurchaseProductSuccessCallback;
        private static Action<string> s_onPurchaseProductErrorCallback;
        
        public static void PurchaseProduct(string productId, Action<PurchaseProductResponse> onSuccessCallback = null, Action<string> onErrorCallback = null, string developerPayload = "")
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

            PurchaseProductResponse response = JsonUtility.FromJson<PurchaseProductResponse>(purchaseProductResponseJson);

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
        
        #region GetProductsCatalog
        private static Action<ProductsCatalogResponse> s_onGetProductsCatalogSuccessCallback;
        private static Action<string> s_onGetProductsCatalogErrorCallback;
        
        public static void GetProductsCatalog(Action<ProductsCatalogResponse> onSuccessCallback, Action<string> onErrorCallback = null, CurrencyPictureSize currencyPictureSize = CurrencyPictureSize.medium)
        {
            s_onGetProductsCatalogSuccessCallback = onSuccessCallback;
            s_onGetProductsCatalogErrorCallback = onErrorCallback;

            BillingGetProductsCatalog(OnGetProductsCatalogSuccessCallback, OnGetProductsCatalogErrorCallback, currencyPictureSize.ToString());
        }

        [DllImport("__Internal")]
        private static extern void BillingGetProductsCatalog(Action<string> successCallback, Action<string> errorCallback, string currencyPictureSize);

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetProductsCatalogSuccessCallback(string productsCatalogResponseJson)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(Billing)}.{nameof(OnGetProductsCatalogSuccessCallback)} invoked, {nameof(productsCatalogResponseJson)} = {productsCatalogResponseJson}");

            ProductsCatalogResponse response = JsonUtility.FromJson<ProductsCatalogResponse>(productsCatalogResponseJson);

            s_onGetProductsCatalogSuccessCallback?.Invoke(response);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetProductsCatalogErrorCallback(string errorMessage)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(Billing)}.{nameof(OnGetProductsCatalogErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

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