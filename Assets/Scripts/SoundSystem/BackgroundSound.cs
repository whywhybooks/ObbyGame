using FMODUnity;
using UnityEngine;

public class BackgroundSound : MonoBehaviour
{
    [SerializeField] private CharacterHealth _characterHealth;
    [SerializeField] private EmitterRef _emitterRef;
    [SerializeField] private CheckPointController _checkPointController;
    [SerializeField] private RestartController _restartController;

    private void OnEnable()
    {
        _characterHealth.OnDiedFromFall += StopSound;
        _characterHealth.OnDiedOfShock += StopSound;
     //   _checkPointController.OnRestart += StartSound;
        _restartController.Restart += StartSound;
        _restartController.SkipLevel += StartSound;
    }

    private void OnDisable()
    {
        _characterHealth.OnDiedFromFall -= StopSound;
        _characterHealth.OnDiedOfShock -= StopSound;
      //  _checkPointController.OnRestart -= StartSound;
        _restartController.Restart -= StartSound;
        _restartController.SkipLevel -= StartSound;
    }

    private void StartSound()
    {
        _emitterRef.Target.SetParameter(_emitterRef.Params[0].Name, 0);
    }

    private void StopSound()
    {
        _emitterRef.Target.SetParameter(_emitterRef.Params[0].Name, 1);
    }
}
