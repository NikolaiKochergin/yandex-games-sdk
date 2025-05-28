using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VervePlace.YandexGames.Samples
{
    public class CatalogProductView : MonoBehaviour
    {
        [SerializeField] private RawImage _icon;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _price;
        [SerializeField] private RawImage _currencyIcon;
        [SerializeField] private Button _buyButton;
        
        private CatalogProduct _product;

        public void Setup(CatalogProduct product, Action<CatalogProduct> onBuyButtonClick)
        {
            _product = product;
            _description.text = $"<b>{_product.title}</b>\n<color=\"green\">id:</color> {_product.id}\n{_product.description}";
            _price.text = _product.price;
            LoadIcons();
            _buyButton.onClick.AddListener(() => onBuyButtonClick(_product));
        }

        private void LoadIcons()
        {
            StartCoroutine(new RemoteImage(_product.imageURI).Download(texture =>
            {
                _icon.gameObject.SetActive(false);
                _icon.texture = texture;
                _icon.gameObject.SetActive(true);
                Canvas.ForceUpdateCanvases();
                LayoutRebuilder.ForceRebuildLayoutImmediate(_icon.rectTransform);
            }));
            StartCoroutine(new RemoteImage(_product.priceCurrencyImage).Download(texture =>
            {
                _currencyIcon.gameObject.SetActive(false);
                _currencyIcon.texture = texture;
                _currencyIcon.gameObject.SetActive(true);
                Canvas.ForceUpdateCanvases();
                LayoutRebuilder.ForceRebuildLayoutImmediate(_currencyIcon.rectTransform);
            }));
        }
    }
}