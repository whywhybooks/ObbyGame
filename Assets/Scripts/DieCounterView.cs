using TMPro;
using UnityEngine;

public class DieCounterView : MonoBehaviour
{
    [SerializeField] private CharacterHealth _characterHealth;
    [SerializeField] private TMP_Text _superItemCountText;

    private void OnEnable()
    {
        _characterHealth.ChangeDieCounter += UpdateText;
    }

    private void OnDisable()
    {
        _characterHealth.ChangeDieCounter -= UpdateText;
    }

    private void UpdateText()
    {
        _superItemCountText.text = _characterHealth.DieCounter.ToString();
    }
}
