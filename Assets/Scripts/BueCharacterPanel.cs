using UnityEngine;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using UnityEngine.Events;
using Analytics;

public class BueCharacterPanel : UIPanel
{
    [Header("Side components")]
    [SerializeField] private CharacterTypeChanger _characterTypeChanger;

    [Header("Panel components")]
    [SerializeField] private CanvasGroup _thisPanel;
    [SerializeField] private Image _nameImage;
    [SerializeField] private Image _superPowerImage;
    [SerializeField] private Image _characterImage;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _bueButton;

    public event UnityAction<CharacterType> OnBue; // Лишняя возмонжо. Убрать, если не пригодится

    CharacterTypeConfigure _lockedCharacter;


    private void Start()
    {
     /*   if (PlayerPrefs.GetInt("IsAdsCharacter") == 1)
        {
            // _openButton.gameObject.SetActive(false);
        }*/

        StandardPurchasingModule.Instance().useFakeStoreAlways = true;

        if (PlayerPrefs.HasKey(PlayerPrefsParametrs.FirstStartForPers) == false)
        {
            RestoreMyProduct();
            PlayerPrefs.SetInt(PlayerPrefsParametrs.FirstStartForPers, 1);
        }
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(Close);
        //_bueButton.onClick.RemoveListener(Buy);
    }

    public void UpdateData()
    {
        _thisPanel.Activate();
        _closeButton.onClick.AddListener(_thisPanel.Deactivate);

        foreach (var c in _characterTypeChanger.ConfiguresCharacter)
        {
            if (c.CharacterType == CharacterType.Man)
            {
                if (PlayerPrefs.GetInt(PlayerPrefsParametrs.IsAdsMan) == 0)
                {
                    _nameImage.sprite = c.StartNameLabel;
                    _superPowerImage.sprite = c.SuperPowerSprite;
                    _characterImage.sprite = c.MainCharacterImage;
                    _lockedCharacter = c;
                }
            }
            else
            {
                if (PlayerPrefs.GetInt(PlayerPrefsParametrs.IsAdsGirl) == 0)
                {
                    _nameImage.sprite = c.StartNameLabel;
                    _superPowerImage.sprite = c.SuperPowerSprite;
                    _characterImage.sprite = c.MainCharacterImage;
                    _lockedCharacter = c;
                }
            }

          /* if (c.IsOpen == false)
            {
                _nameImage.sprite = c.StartNameLabel;
                _superPowerImage.sprite = c.SuperPowerSprite;
                _characterImage.sprite = c.MainCharacterImage;
                _lockedCharacter = c;
                return;
            }*/
        }
    }

    public void Buy()
    {
        GameAnalytics.gameAnalytics.LogEvent("buy_pers_success");
        CharacterType bueType = CharacterType.Man;

        if (PlayerPrefs.GetInt(PlayerPrefsParametrs.IsAdsMan) == 0)
        {
            bueType = CharacterType.Man;
        }

        if (PlayerPrefs.GetInt(PlayerPrefsParametrs.IsAdsGirl) == 0)
        {
            bueType = CharacterType.Girl;
        }

        //Покупка. Если покупка прошла, устанавливаем персонажа в управление
        _characterTypeChanger.SetIsOpen(bueType, true);
        _characterTypeChanger.SetCharacter(bueType);
        OnBue?.Invoke(bueType);
     //   PlayerPrefs.SetInt("IsAdsCharacter", 1);
        // Close();
        _thisPanel.Deactivate();
    }

    public void OnPurchasingComlited(string product)
    {
        if (product == "buy_pers")
        {
            Buy();
        }
    }

    public void OnPurchasingComlited(Product product)
    {
        if (product.definition.id == "buy_pers")
        {
            Buy();
        }
    }

    public void OnPurchasingFiled(Product product, PurchaseFailureDescription purchaseFailureDescription)
    {
        GameAnalytics.gameAnalytics.LogEvent("buy_pers_failed");
    }

    public void OnPurchasingClick(string productID)
    {
        if (PlayerPrefs.GetInt(PlayerPrefsParametrs.IsAdsMan) == 0)
        {
            GameAnalytics.gameAnalytics.LogEvent("buy_pers_baddy");
        }
        else if (PlayerPrefs.GetInt(PlayerPrefsParametrs.IsAdsGirl) == 0)
        {
            GameAnalytics.gameAnalytics.LogEvent("buy_pers_chica");
        }

        if (productID == "buy_pers")
        {
            //GameAnalytics.gameAnalytics.LogEvent("buy_noads");
           // Buy();
        }
    }

    private void RestoreMyProduct()
    {
        if (CodelessIAPStoreListener.Instance.StoreController.products.WithID("buy_pers").hasReceipt)
        {
            Buy();
        }
    }

}
