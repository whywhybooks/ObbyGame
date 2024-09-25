using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacterButton : MonoBehaviour
{
    [SerializeField] private CharacterType _characterType;
    [SerializeField] private CharacterChangerController _characterChangerController;
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _lockSprite;
    [SerializeField] private Sprite _normaSprite;
    [SerializeField] private BueCharacterPanel _bueCharacterPanel;

    [SerializeField] private ButtonIconChanger _buttonIconChanger;

    private bool _changePosible;

    public CharacterType CharacterType { get => _characterType; private set => _characterType = value; }

    public void PointerDown()
    {
        if (_characterChangerController.SetCharacter(_characterType))
        {
            _buttonIconChanger.ChangePress();
            _changePosible = true;
        }
        else
        {
            _bueCharacterPanel.UpdateData();
        }
    }

    public void PointerUp()
    {
        if (_changePosible)
        {
            _buttonIconChanger.ChangeNormal();
            _changePosible = false;
        }
    }

    internal void Lock()
    {
        _image.sprite = _lockSprite;
    }

    internal void Unlock()
    {
        _image.sprite = _normaSprite;
    }
}