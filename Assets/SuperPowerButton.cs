using UnityEngine;
using UnityEngine.UI;

public class SuperPowerButton : MonoBehaviour
{
    [Header("SuperPower")]
    [SerializeField] private SuperpowerController _superpowerController;
    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _iconNormal;
    [SerializeField] private Sprite _iconLock;

    private void OnEnable()
    {
        _superpowerController.FillSuperItemCount += ActivateButton;
        _superpowerController.OverSuperItemCount += DeactivateButton;
    }

    private void OnDisable()
    {
        _superpowerController.FillSuperItemCount -= ActivateButton;
        _superpowerController.OverSuperItemCount -= DeactivateButton;
    }

    private void DeactivateButton()
    {
        _icon.sprite = _iconLock;
    }

    public void ChangePress()
    {
        _superpowerController.PlayPower();
    }

    private void ActivateButton()
    {
        _icon.sprite = _iconNormal;
    }
}
