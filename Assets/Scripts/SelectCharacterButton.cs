using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacterButton : MonoBehaviour
{
    [SerializeField] private CharacterType _characterType;
    [SerializeField] private CharacterChangerController _characterChangerController;
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _lockSprite;
    [SerializeField] private Sprite _normalSprite;
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private BueCharacterPanel _bueCharacterPanel;

    private bool _changePosible;
    private bool _unlocked;

    public CharacterType CharacterType { get => _characterType; private set => _characterType = value; }

    public void PointerDown()
    {
        if (_characterChangerController.SetCharacter(_characterType))
        {
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
            _changePosible = false;
        }
    }

    public void SetActivate(bool isActive)
    {
        if (_unlocked == false)
            return;

        if (isActive)
        {
            _image.sprite = _activeSprite;
        }
        else
        {
            _image.sprite = _normalSprite;
        }
    }

    internal void Lock()
    {
        _image.sprite = _lockSprite;
    }

    internal void Unlock()
    {
        _image.sprite = _normalSprite;
        _unlocked = true;
    }
}