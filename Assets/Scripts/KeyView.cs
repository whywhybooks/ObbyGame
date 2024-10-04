using TMPro;
using UnityEngine;

public class KeyView : MonoBehaviour
{
    [SerializeField] private Door _door;
    [SerializeField] private TMP_Text _counter;
    [SerializeField] private CanvasGroup _thisPanel;

    private CharacterKeys _characterKeys;
    private int _targetKey;

    private void OnEnable()
    {
        _characterKeys = FindObjectOfType<CharacterKeys>();

        _characterKeys.OnOpen += DeactivateDoor;
        _characterKeys.OnGetKey += UpdateText;
        _door.OnTriggerEnterForKey += SetActive;
    }

    private void OnDisable()
    {
        _characterKeys.OnGetKey -= UpdateText;
        _characterKeys.OnOpen -= DeactivateDoor;
        _door.OnTriggerEnterForKey -= SetActive;
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
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

        _targetKey = keyCount;
        UpdateText();
    }

    private void UpdateText()
    {
        _counter.text = $"{_characterKeys.CurrentCount}/{_door.TargetKeys}";
    }

    private void DeactivateDoor()
    {
        _thisPanel.Deactivate();
    }
}
