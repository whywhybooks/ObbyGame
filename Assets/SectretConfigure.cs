using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SectretConfigure : MonoBehaviour
{
    [SerializeField] private int _targetClickCounter;
    [SerializeField] private Button _secretButton;
    [SerializeField] private InAppRemoveAd _inAppRempveAdd;
    [SerializeField] private BueCharacterPanel _bueCharacterPanel;

    private int _currentCounter;

    private void OnEnable()
    {
        _secretButton.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _secretButton.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        _currentCounter++;

        if (_currentCounter >= _targetClickCounter)
        {
            _inAppRempveAdd.RemoveAds();
            _bueCharacterPanel.Buy();
        }
    }
}
