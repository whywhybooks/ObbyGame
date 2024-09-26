using System.Collections.Generic;
using UnityEngine;

public class CharacterChangerController : MonoBehaviour
{
    [SerializeField] private BueCharacterPanel _autoBueCharacterPanel;
    [SerializeField] private BueCharacterPanel _bueCharacterPanel;
    [SerializeField] private CharacterTypeChanger _characterTypeChanger;
    [SerializeField] private List<SelectCharacterButton> _selectCharacterButtons = new List<SelectCharacterButton>();

    private void OnEnable()
    {
        _autoBueCharacterPanel.OnBue += UnlockedCharacter;
        _bueCharacterPanel.OnBue += UnlockedCharacter;
        _characterTypeChanger.OnChangeOpenValueForCharacter += LockButton;
    }

    private void OnDisable()
    {
        _autoBueCharacterPanel.OnBue -= UnlockedCharacter;
        _bueCharacterPanel.OnBue -= UnlockedCharacter;
        _characterTypeChanger.OnChangeOpenValueForCharacter -= LockButton;
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
