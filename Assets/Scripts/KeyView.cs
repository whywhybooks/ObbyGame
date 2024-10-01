using System;
using TMPro;
using UnityEngine;

public class KeyView : MonoBehaviour
{
    [SerializeField] private DoorsManager _doorsManager;
    [SerializeField] private CharacterKeys _characterKeys;
    [SerializeField] private TMP_Text _counter;
    [SerializeField] private CanvasGroup _thisPanel;

    private void OnEnable()
    {
        _characterKeys.OnGetKey += UpdateText;
        _characterKeys.OnOpen += DeactivateDoor;
        _doorsManager.OnOverDoors += ClosePanel;
       // _doorsManager.OnSetNewDoor += UpdateText;

        _doorsManager.TriggerEnterForKey += SetActive;
    }

    private void OnDisable()
    {
        _characterKeys.OnGetKey -= UpdateText;
        _characterKeys.OnOpen -= DeactivateDoor;
        _doorsManager.OnOverDoors -= ClosePanel;
        //  _doorsManager.OnSetNewDoor -= UpdateText;

        _doorsManager.TriggerEnterForKey -= SetActive;
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private void UpdateText()
    {
        _counter.text = $"{_characterKeys.CurrentCount}/{_doorsManager.TargetKeysCount}";
    }

    private void SetActive(bool isActive, int keyCount)
    {
        if (isActive)
        {
            _thisPanel.Activate();
        }
        else
        {
            _thisPanel.Deactivate();
        }

        _counter.text = $"{_characterKeys.CurrentCount}/{keyCount}";
    }

    private void DeactivateDoor()
    {
        _thisPanel.Deactivate();
    }
}
