using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class InAppShop : MonoBehaviour
{
    [SerializeField] private CanvasGroup _removeAdsButton;
    [SerializeField] private CanvasGroup _addBumButton;
    [SerializeField] private CanvasGroup _addRabbitButton;

    private InAppButton _bumInAppButton;
    private InAppButton _rabbitInAppButton;

    private void Start()
    {
        var inAppButtons = FindObjectsOfType<InAppButton>();

        foreach (var inAppButton in inAppButtons)
        {
            if (inAppButton.Type == InAppButton.TypeInApp.Bum)
            {
                _bumInAppButton = inAppButton;
            }
            else
            {
                _rabbitInAppButton = inAppButton;
            }
        }

        if (PlayerPrefs.GetInt("IsAdsRemove") == 1)
        {
           // _removeAdsButton.DisableGroup();
        }
        if (PlayerPrefs.GetInt("IsAddBum") == 1)
        {
            _bumInAppButton.SwitchButton();
           // _addBumButton.DisableGroup();
        }
        if (PlayerPrefs.GetInt("IsAddRabbit") == 1)
        {
            _rabbitInAppButton.SwitchButton();
           // _addRabbitButton.DisableGroup();
        }
        StandardPurchasingModule.Instance().useFakeStoreAlways = true;

        if (PlayerPrefs.HasKey("FirstStart") == false)
        {
            RestoreMyProduct();
            PlayerPrefs.SetInt("FirstStart", 1);
        }
    }

    public void OnPurchasingComlited(Product product)
    {
        switch (product.definition.id)
        {
            case "buy_no_ads":
                RemoveAds();
                break;

            case "buy_pers_super_bum":
                AddBum();
                break;

            case "buy_pers_pink_rabbit":
                AddRabbit();
                break;
        }
    }

    public void OnPurchasingComlited(string product)
    {
        switch (product)
        {
            case "buy_no_ads":
                RemoveAds();
                break;

            case "buy_pers_super_bum":
                AddBum();
                break;

            case "buy_pers_pink_rabbit":
                AddRabbit();
                break;
        }
    }

    public void OnPurchasingFiled(Product product, PurchaseFailureDescription purchaseFailureDescription)
    {
        switch (product.definition.id)
        {
            case "buy_no_ads":
             //   GameAnalytics.gameAnalytics.LogEvent("buy_noads_failed");
                break;

            case "buy_pers_super_bum":
                //GameAnalytics.gameAnalytics.LogEvent("buy_bum_failed");
                break;

            case "buy_pers_pink_rabbit":
               // GameAnalytics.gameAnalytics.LogEvent("buy_rabbit_failed");
                break;
        }
    }

    private void RemoveAds()
    {
        PlayerPrefs.SetInt("IsAdsRemove", 1);
      //  _removeAdsButton.DisableGroup();
       // GameAnalytics.gameAnalytics.LogEvent("buy_noads_success");
    }

    private void AddRabbit()
    {
        PlayerPrefs.SetInt("IsAddRabbit", 1);
        _rabbitInAppButton.SwitchButton();
       // _addRabbitButton.DisableGroup();
      //  GameAnalytics.gameAnalytics.LogEvent("buy_rabbit_success");
    }

    private void AddBum()
    {
        PlayerPrefs.SetInt("IsAddBum", 1);
        _bumInAppButton.SwitchButton();
      //  _addBumButton.DisableGroup();
      //  GameAnalytics.gameAnalytics.LogEvent("buy_bum_success");
    }

    public void OnPurchasingClick(string productID)
    {
        switch (productID)
        {
            case "buy_no_ads":
            //    GameAnalytics.gameAnalytics.LogEvent("buy_noads");
                break;

            case "buy_pers_super_bum":
            //    GameAnalytics.gameAnalytics.LogEvent("buy_bum");
                break;

            case "buy_pers_pink_rabbit":
             //   GameAnalytics.gameAnalytics.LogEvent("buy_rabbit");
                break;
        }
    }

    private void RestoreMyProduct()
    {
        if (CodelessIAPStoreListener.Instance.StoreController.products.WithID("buy_no_ads").hasReceipt)
        {
            RemoveAds();
        }

        if (CodelessIAPStoreListener.Instance.StoreController.products.WithID("buy_pers_super_bum").hasReceipt)
        {
            AddBum();
        }

        if (CodelessIAPStoreListener.Instance.StoreController.products.WithID("buy_pers_pink_rabbit").hasReceipt)
        {
            AddRabbit();
        }
    }
}
