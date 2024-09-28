using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChangerController : MonoBehaviour
{
    [SerializeField] private BueCharacterPanel _autoBueCharacterPanel;
    [SerializeField] private BueCharacterPanel _bueCharacterPanel;
    [SerializeField] private SelectCharacterPanel _selectCharacterPanel;
    [SerializeField] private CharacterTypeChanger _characterTypeChanger;
    [SerializeField] private List<SelectCharacterButton> _selectCharacterButtons = new List<SelectCharacterButton>();

    private void OnEnable()
    {
        _autoBueCharacterPanel.OnBue += UnlockedCharacter;
        _bueCharacterPanel.OnBue += UnlockedCharacter;
        _characterTypeChanger.OnChangeOpenValueForCharacter += LockButton;
        _characterTypeChanger.OnChangeCharacter += ChangeButtonState;
        _selectCharacterPanel.OnSelectCharacter += UnlockedCharacter;
    }

    private void OnDisable()
    {
        _autoBueCharacterPanel.OnBue -= UnlockedCharacter;
        _bueCharacterPanel.OnBue -= UnlockedCharacter;
        _characterTypeChanger.OnChangeOpenValueForCharacter -= LockButton;
        _characterTypeChanger.OnChangeCharacter -= ChangeButtonState;
    }

    private void ChangeButtonState(CharacterType chatacterType)
    {
        if (chatacterType == CharacterType.Man)
        {
            _selectCharacterButtons[0].SetActivate(false);
            _selectCharacterButtons[1].SetActivate(true);
        }
        else
        {
            _selectCharacterButtons[0].SetActivate(true);
            _selectCharacterButtons[1].SetActivate(false);
        }
    }

    public bool SetCharacter(CharacterType characterType)
    {
        return _characterTypeChanger.SetCharacter(characterType);
    }

    private void UnlockedCharacter(CharacterType type)
    {
        foreach (var button in _selectCharacterButtons)
        {
            if (button.CharacterType == type)
            {
                button.Unlock();
            }
        }

        ChangeButtonState(type);
    }

    private void LockButton()
    {
        CharacterType lockCharaterType = CharacterType.Girl;

        foreach (var c in _characterTypeChanger.ConfiguresCharacter)
        {
            if (c.IsOpen == false)
            {
                lockCharaterType = c.CharacterType;

                foreach (var button in _selectCharacterButtons)
                {
                    if (button.CharacterType == lockCharaterType)
                    {
                        button.Lock();
                        break;
                    }
                }
            }
        }
    }
}
