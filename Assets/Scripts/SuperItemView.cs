using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SuperItemView : MonoBehaviour
{
    [SerializeField] private SuperpowerController _superPowerController;
    [SerializeField] private TMP_Text _superItemCountText;
    [SerializeField] private Image _barImage;

    private void OnEnable()
    {
        _superPowerController.ChangeSuperItemCount += SuperItemPickUp;
    }

    private void OnDisable()
    {
        _superPowerController.ChangeSuperItemCount -= SuperItemPickUp;
    }

    private void SuperItemPickUp()
    {
        _superItemCountText.text = _superPowerController.SuperItemCount.ToString();
        _barImage.fillAmount = ((float)_superPowerController.SuperItemCount / _superPowerController.TargetCountSuperItem) - 0.0165f * (float)_superPowerController.SuperItemCount;
    }
}
