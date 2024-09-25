using UnityEngine;

public class InAppBueCharacter : MonoBehaviour
{
    //Все настройки по этому инаппу в скрипте BueCharacterPanel
    [SerializeField] private CheckPointController _checkpointController;
    [SerializeField] private BueCharacterPanel _bueCharacterPanel;

    private void OnEnable()
    {
        _checkpointController.OnReachedUnlockLevel += UnlockCharacter;
    }

    private void OnDisable()
    {
        _checkpointController.OnReachedUnlockLevel -= UnlockCharacter;
    }

    private void UnlockCharacter()
    {
        _bueCharacterPanel.UpdateData();//Костыль
        _bueCharacterPanel.Buy();
        _checkpointController.OnReachedUnlockLevel -= UnlockCharacter;
    }
}
