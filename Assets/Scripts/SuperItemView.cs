using TMPro;
using UnityEngine;

public class SuperItemView : MonoBehaviour
{
    [SerializeField] private SuperpowerController _superPowerController;
    [SerializeField] private TMP_Text _superItemCountText;

    private void OnEnable()
    {
        _superPowerController.OverSuperItemCount += SuperItemOver;
        _superPowerController.ChangeSuperItemCount += SuperItemPickUp;
    }

    private void OnDisable()
    {
        _superPowerController.OverSuperItemCount -= SuperItemOver;
        _superPowerController.ChangeSuperItemCount -= SuperItemPickUp;
    }

    private void SuperItemOver()
    {
        //Выключение интерактивности кнопки
    }

    private void SuperItemPickUp()
    {
        _superItemCountText.text = _superPowerController.SuperItemCount.ToString();
    }
}
