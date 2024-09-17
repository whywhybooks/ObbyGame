using UnityEngine;
using UnityEngine.Purchasing;

public class InAppButton : MonoBehaviour
{
    public enum TypeInApp
    {
        Rabbit,
        Bum
    }
    [SerializeField] private CanvasGroup _defaultButton;
    [SerializeField] private CanvasGroup _inAppButton;
    [SerializeField] private TypeInApp _type;

    private InAppShop _shop;

    private void Start()
    {
        _shop = FindAnyObjectByType<InAppShop>();
    }

    public TypeInApp Type => _type;

    public void SwitchButton()
    {
      //  _defaultButton.EnableGroup();
       // _inAppButton.DisableGroup();
    }

    public void ClickHandler(Product product)
    {
        _shop.OnPurchasingComlited(product);
    }

    public void ClickHandler(string product)
    {
        _shop.OnPurchasingComlited(product);
    }
}
