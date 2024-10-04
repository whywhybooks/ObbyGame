using FMODUnity;
using UnityEngine;

public class SuperPowerSound : MonoBehaviour
{
    [SerializeField] private SuperpowerController _superPowerController;
    [SerializeField] private PhysicalCC _physicalCC;

    [Header("Sound Parametrs")]
    [SerializeField] private EventReference _eventHasAbility;
    [SerializeField] private EventReference _eventAbilityStart;
    [SerializeField] private EventReference _eventAbilityEnd;

    private void OnEnable()
    {
        _physicalCC.OnLongJumpEnd += OnLongJumpEnd;
        _physicalCC.OnLongJumpStart += OnLongJumpStart;
        _superPowerController.AbilityActivate += OnAbilityActivate;
    }

    private void OnDisable()
    {
        _physicalCC.OnLongJumpEnd -= OnLongJumpEnd;
        _physicalCC.OnLongJumpStart -= OnLongJumpStart;
        _superPowerController.AbilityActivate -= OnAbilityActivate;
    }

    private void OnLongJumpStart()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(_eventAbilityStart, gameObject);
    }

    private void OnLongJumpEnd()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(_eventAbilityEnd, gameObject);
    }

    private void OnAbilityActivate()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(_eventHasAbility, gameObject);
    }
}
