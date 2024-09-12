using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShiledTimeView : MonoBehaviour
{
    [SerializeField] private GameObject _shieldTimePanel;
    [SerializeField] private CharacterHealth _chatacterHealth;
    [SerializeField] private TMP_Text _shieldTimeText;

    private bool _isView;

    private void OnEnable()
    {
        _chatacterHealth.OnShieldOver += ShieldOver;
        _chatacterHealth.OnShieldPickUp += ShieldPickUp;
    }

    private void OnDisable()
    {
        _chatacterHealth.OnShieldOver -= ShieldOver;
        _chatacterHealth.OnShieldPickUp -= ShieldPickUp;
    }

    private void Update()
    {
        if (_isView == false)
            return;

        _shieldTimeText.text = Mathf.Round(_chatacterHealth.ShieldLeftTime).ToString();
    }

    private void ShieldOver()
    {
        _isView = false;
        _shieldTimePanel.SetActive(false);
    }

    private void ShieldPickUp()
    {
        _isView = true;
        _shieldTimePanel.SetActive(true);
    }
}
