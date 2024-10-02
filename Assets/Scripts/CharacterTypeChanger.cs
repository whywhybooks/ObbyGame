using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterTypeChanger : MonoBehaviour
{
    [SerializeField] CharacterType _characterType;
    [SerializeField] private List<CharacterTypeConfigure> _configuresCharacter = new List<CharacterTypeConfigure>();
    [SerializeField] private SkinnedMeshRenderer _renderer;

    private GameObject _decoreObject;

    public CharacterType CharacterType { get => _characterType; private set => _characterType = value; }
    public List<CharacterTypeConfigure> ConfiguresCharacter { get => _configuresCharacter; private set => _configuresCharacter = value; }

    public event UnityAction OnChangeOpenValueForCharacter;
    public event UnityAction<CharacterType> OnChangeCharacter;

    private void Start()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsParametrs.FirstGameStart) == 1)
        {
            if (PlayerPrefs.GetInt(PlayerPrefsParametrs.IsAdsMan) == 1)
            {
                SetCharacter(CharacterType.Man);
            }
            else if (PlayerPrefs.GetInt(PlayerPrefsParametrs.IsAdsGirl) == 1)
            {
                SetCharacter(CharacterType.Girl);
            }    
        }
    }

    public bool SetCharacter(CharacterType type)
    {
        if (type == _characterType)
            return true;

        if (type == CharacterType.Man)
        {
            if (PlayerPrefs.GetInt(PlayerPrefsParametrs.IsAdsMan) == 0)
            {
                return false;
            }
        }
        else if (type == CharacterType.Girl)
        {
            if (PlayerPrefs.GetInt(PlayerPrefsParametrs.IsAdsGirl) == 0)
            {
                return false;
            }
        }

       /* foreach (CharacterTypeConfigure c in _configuresCharacter)
        {
            if (c.CharacterType == type)
            {
                if (c.IsOpen == false)
                    return false;
            }
        }*/

        if (_decoreObject != null)
        { 
            _decoreObject.SetActive(false); 
            _decoreObject = null;
        }

        foreach(CharacterTypeConfigure c in _configuresCharacter)
        {
            if (c.CharacterType == type)
            {
                _renderer.sharedMesh = c.Mesh;
                _characterType = c.CharacterType;

                if (c.DecoreGameObject != null)
                {
                    c.DecoreGameObject.SetActive(true);
                    _decoreObject = c.DecoreGameObject;
                }

                break;
            }
        }
        Debug.Log(123);
        OnChangeCharacter?.Invoke(_characterType);

        return true;
    }

    public void SetIsOpen(CharacterType type, bool isOpen)
    {
        foreach (CharacterTypeConfigure c in _configuresCharacter)
        {
            if (c.CharacterType == type)
            {
              //  c.IsOpen = isOpen;
            }
        }

        if (type == CharacterType.Man)
        {
            PlayerPrefs.SetInt(PlayerPrefsParametrs.IsAdsMan, 1);
        }
        else
        {
            PlayerPrefs.SetInt(PlayerPrefsParametrs.IsAdsGirl, 1);
        }

        OnChangeOpenValueForCharacter?.Invoke();
    }
}

[Serializable]
public class CharacterTypeConfigure 
{
    [field: SerializeField] public CharacterType CharacterType;
    [field: SerializeField] public Mesh Mesh;
    [field: SerializeField] public GameObject DecoreGameObject;
   // [field: SerializeField] public bool IsOpen;
    [field: SerializeField] public Sprite MainCharacterImage;
    [field: SerializeField] public Sprite NextCharacterImage;
    [field: SerializeField] public Sprite StartNameLabel;
    [field: SerializeField] public Sprite SideNameLabel;
    [field: SerializeField] public Sprite SuperPowerSprite;

    public void SetIsOpen(bool isOpen)
    {
       // IsOpen = isOpen;
    }
}
