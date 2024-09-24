using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonIconChanger : MonoBehaviour
{
    [Header("Icon")]
    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _iconNormal;
    [SerializeField] private Sprite _iconPress;
    [Header("Background")]
    [SerializeField] private Image _background;
    [SerializeField] private Sprite _backgroundNormal;
    [SerializeField] private Sprite _backgroundPress;

    public void ChangePress()
    {
        _icon.sprite = _iconPress;
        _background.sprite = _backgroundPress;
    }

    public void ChangeNormal()
    {
        _icon.sprite = _iconNormal;
        _background.sprite = _backgroundNormal;
    }    
}
