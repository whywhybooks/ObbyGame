using UnityEngine;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using UnityEngine.Events;

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
        if (PlayerPrefs.GetInt("IsAdsCharacter") == 1)
        {
            // _openButton.gameObject.SetActive(false);
        }

        StandardPurchasingModule.Instance().useFakeStoreAlways = true;

        if (PlayerPrefs.HasKey("FirstStart") == false)
        {
            RestoreMyProduct();
            PlayerPrefs.SetInt("FirstStart", 1);
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
            if (c.IsOpen == false)
            {
                _nameImage.sprite = c.StartNameLabel;
                _superPowerImage.sprite = c.SuperPowerSprite;
                _characterImage.sprite = c.MainCharacterImage;
                _lockedCharacter = c;
                return;
            }
        }
    }

    public void Buy()
    {
        //Покупка. Если покупка прошла, устанавливаем персонажа в управление
        _characterTypeChanger.SetIsOpen(_lockedCharacter.CharacterType, true);
        _characterTypeChanger.SetCharacter(_lockedCharacter.CharacterType);
        OnBue?.Invoke(_lockedCharacter.CharacterType);
        PlayerPrefs.SetInt("IsAdsCharacter", 1);
        // Close();
        _thisPanel.Deactivate();
    }

    public void OnPurchasingComlited(string product)
    {
        if (product == "buy_character")
        {
            Buy();
        }
    }

    public void OnPurchasingComlited(Product product)
    {
        if (product.definition.id == "buy_character")
        {
            Buy();
        }
    }

    public void OnPurchasingFiled(Product product, PurchaseFailureDescription purchaseFailureDescription)
    {
        if (product.definition.id == "buy_character")
        {
            // GameAnalytics.gameAnalytics.LogEvent("buy_noads_failed");
        }
    }

    public void OnPurchasingClick(string productID)
    {
        if (productID == "buy_character")
        {
            //GameAnalytics.gameAnalytics.LogEvent("buy_noads");
            Buy();
        }
    }

    private void RestoreMyProduct()
    {
        if (CodelessIAPStoreListener.Instance.StoreController.products.WithID("buy_character").hasReceipt)
        {
            Buy();
        }
    }

}
