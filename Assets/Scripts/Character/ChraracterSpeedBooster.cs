using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class ChraracterSpeedBooster : MonoBehaviour
{
    [SerializeField] private CharacterHealth _characterHealth;

    private PlayerInput _playerInput;
    private float _currentMultiplier;
    private Coroutine _removeAccelerationCoroutine;
    private bool _isBoosted;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        _characterHealth.OnDied += BoostReset;
    }

    private void OnDisable()
    {
        _characterHealth.OnDied -= BoostReset;
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

    private void BoostReset()
    {
        if (_isBoosted)
        {
            _playerInput.RemoveAcceleration(_currentMultiplier);

            if (_removeAccelerationCoroutine != null)
                StopCoroutine(_removeAccelerationCoroutine);

            _removeAccelerationCoroutine = null;
            _isBoosted = false;
        }
    }

    private IEnumerator RemoveAcceleration(float duration)
    {
        yield return new WaitForSeconds(duration);

        _playerInput.RemoveAcceleration(_currentMultiplier);
        _removeAccelerationCoroutine = null;
        _isBoosted = false;
    }
}
