using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSound : MonoBehaviour
{
    [SerializeField] private CharacterHealth _characterHealth;
    [SerializeField] private EmitterRef _emitterRef;
    [SerializeField] private CheckPointController _checkPointController;

    private void OnEnable()
    {
        _characterHealth.OnDiedFromFall += StopSound;
        _characterHealth.OnDiedOfShock += StopSound;
        _checkPointController.OnRestart += StatSound;
    }

    private void OnDisable()
    {
        _characterHealth.OnDiedFromFall -= StopSound;
        _characterHealth.OnDiedOfShock -= StopSound;
        _checkPointController.OnRestart -= StatSound;
    }

    private void StatSound()
    {
        _emitterRef.Target.SetParameter(_emitterRef.Params[0].Name, 0);
    }

    private void StopSound()
    {
        _emitterRef.Target.SetParameter(_emitterRef.Params[0].Name, 1);
    }
}
