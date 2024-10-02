using Analytics;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;

public class InAppRemoveAd : MonoBehaviour
{
    [SerializeField] private CanvasGroup _removeAdPanel;
    [SerializeField] private Button  _openButton;
    [SerializeField] private Button _closeButton;

    private void OnEnable()
    {
        _openButton.onClick.AddListener(Open);
        _closeButton.onClick.AddListener(Close);
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsParametrs.IsAdsRemove) == 1)
        {
            _openButton.gameObject.SetActive(false);
        }

        StandardPurchasingModule.Instance().useFakeStoreAlways = true;

        if (PlayerPrefs.HasKey(PlayerPrefsParametrs.FirstStartForAd) == false)
        {
            RestoreMyProduct();
            PlayerPrefs.SetInt(PlayerPrefsParametrs.FirstStartForAd, 1);
        }
    }

    private void OnDisable()
    {
        _openButton.onClick.RemoveListener(Open);
        _closeButton.onClick.RemoveListener(Close);
    }

    private void Close()
    {
        _removeAdPanel.Deactivate();
    }

    private void Open()
    {
        _removeAdPanel.Activate();
    }

    public void RemoveAds()
    {
        PlayerPrefs.SetInt(PlayerPrefsParametrs.IsAdsRemove, 1);
        _openButton.gameObject.SetActive(false);
        _removeAdPanel.Deactivate();
        GameAnalytics.gameAnalytics.LogEvent("buy_noads_success");
    }

    public void OnPurchasingComlited(string product)
    {
        if (product == "buy_no_ads")
        {
            RemoveAds();
        }
    }

    public void OnPurchasingComlited(Product product)
    {
        if (product.definition.id == "buy_no_ads")
        {
            RemoveAds();
        }
    }

    public void OnPurchasingFiled(Product product, PurchaseFailureDescription purchaseFailureDescription)
    {
        if (product.definition.id == "buy_no_ads")
        {
             GameAnalytics.gameAnalytics.LogEvent("buy_noads_failed");
        }
    }

    public void OnPurchasingClick(string productID)
    {
        if (productID == "buy_no_ads")
        {
          //  RemoveAds();
            GameAnalytics.gameAnalytics.LogEvent("buy_noads");
        }
    }

    private void RestoreMyProduct()
    {
        if (CodelessIAPStoreListener.Instance.StoreController.products.WithID("buy_no_ads").hasReceipt)
        {
            RemoveAds();
        }
    }
}


