using FMODUnity;
using UnityEngine;

public class PickupSound : MonoBehaviour
{
    [SerializeField] private EventReference _pickupSound;
    [SerializeField] private SuperpowerController _superpowerController;
    [SerializeField] private CharacterKeys _characterKeys;
    [SerializeField] private CharacterHealth _characterHealth;

    private void OnEnable()
    {
        _superpowerController.PickupSuperItemCount += PlaySound;
        _characterKeys.OnGetKey += PlaySound;
        _characterHealth.OnShieldPickUp += PlaySound;
    }

    private void OnDisable()
    {
        _superpowerController.PickupSuperItemCount -= PlaySound;
        _characterKeys.OnGetKey -= PlaySound;
        _characterHealth.OnShieldPickUp -= PlaySound;
    }

    private void PlaySound()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(_pickupSound, gameObject);
    }
}