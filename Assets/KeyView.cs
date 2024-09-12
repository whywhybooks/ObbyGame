using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyView : MonoBehaviour
{
    [SerializeField] private DoorsManager _doorsManager;
    [SerializeField] private CharacterKeys _characterKeys;
    [SerializeField] private TMP_Text _counter;

    private void OnEnable()
    {
        _characterKeys.OnGetKey += UpdateText;
        _characterKeys.OnOpen += UpdateText;
        _doorsManager.OnOverDoors += ClosePanel;
        _doorsManager.OnSetNewDoor += UpdateText;
    }

    private void OnDisable()
    {
        _characterKeys.OnGetKey -= UpdateText;
        _characterKeys.OnOpen -= UpdateText;
        _doorsManager.OnOverDoors -= ClosePanel;
        _doorsManager.OnSetNewDoor -= UpdateText;
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private void UpdateText()
    {
        _counter.text = $"{_characterKeys.CurrentCount} / {_doorsManager.TargetKeysCount}";
    }
}
