using UnityEngine;
using UnityEngine.UI;

public class SuperPowerButton : MonoBehaviour
{
    [SerializeField] private ButtonIconChanger _buttonIconChanger;

    [Header("SuperPower")]
    [SerializeField] private SuperpowerController _superpowerController;
    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _iconNormal;
    [SerializeField] private Sprite _iconLock;

    private bool _changePosible;

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
        if (_superpowerController.PlayPower())
        {
            _buttonIconChanger.ChangePress();
            _changePosible = true;
        }
    }

    public void ChangeNormal()
    {
        if (_changePosible == true)
        {
            _buttonIconChanger.ChangeNormal();
            _changePosible = false;
        }
    }

    private void ActivateButton()
    {
        _icon.sprite = _iconNormal;
    }
}
