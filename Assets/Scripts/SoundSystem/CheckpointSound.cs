using FMODUnity;
using UnityEngine;

public class CheckpointSound : MonoBehaviour
{
    [SerializeField] private CheckPoint _checkPoint;
    [SerializeField] private EmitterRef _emitterRef;
    [SerializeField][FMODUnity.EventRef] private string aSound;

    bool _isActive;

    private void OnEnable()
    {
        _checkPoint.OnCollisionEnter += StartActivateSoundSound;

        if (_isActive)
        {
             _emitterRef.Target.SetParameter(_emitterRef.Params[0].Name, 1);
        }
    }

    private void OnDisable()
    {
        _checkPoint.OnCollisionEnter -= StartActivateSoundSound;
    }

    private void StartActivateSoundSound(CheckPoint checkPoint)
    {
        _emitterRef.Target.SetParameter(_emitterRef.Params[0].Name, 1);
        FMODUnity.RuntimeManager.PlayOneShotAttached(aSound, gameObject);
        _isActive = true;
    }
}
