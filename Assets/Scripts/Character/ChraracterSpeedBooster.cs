using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class ChraracterSpeedBooster : MonoBehaviour
{
    private PlayerInput _playerInput;
    private float _currentMultiplier;
    private Coroutine _removeAccelerationCoroutine;
    private bool _isBoosted;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    public void BoosSpeed(float multiplier, float duration)
    {
        if (_isBoosted)
            return;

        _isBoosted = true;

        if (_removeAccelerationCoroutine != null)
        {
            StopCoroutine(_removeAccelerationCoroutine);
        }

        _currentMultiplier = multiplier;
        _playerInput.BoostSpeed(multiplier); 
        _removeAccelerationCoroutine = StartCoroutine(RemoveAcceleration(duration));
    }

    private IEnumerator RemoveAcceleration(float duration)
    {
        yield return new WaitForSeconds(duration);

        _playerInput.RemoveAcceleration(_currentMultiplier);
        _removeAccelerationCoroutine = null;
        _isBoosted = false;
    }
}
