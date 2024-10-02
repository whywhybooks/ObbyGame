using TMPro;
using UnityEngine;

public class LevelView : MonoBehaviour
{
    [SerializeField] private CheckPointController _checkpointController;
    [SerializeField] private TMP_Text _counterText;

    private void OnEnable()
    {
        _checkpointController.OnActiveCheckpoint += ChangeText;
    }

    private void OnDisable()
    {
        _checkpointController.OnActiveCheckpoint -= ChangeText;
    }

    private void ChangeText()
    {
        _counterText.text = $"Level {_checkpointController.CurrentCheckPointIndex + 1} / {_checkpointController.AllLevelsCount}";
    }
}
