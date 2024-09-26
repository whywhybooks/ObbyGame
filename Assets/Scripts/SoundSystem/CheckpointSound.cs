using FMODUnity;
using UnityEngine;

public class CheckpointSound : MonoBehaviour
{
    [SerializeField] private CheckPoint _checkPoint;
    [SerializeField] private StudioEventEmitter _emitter;
    [SerializeField] private EventReference aSound;

    bool _isActive;

    private void OnEnable()
    {
        _checkPoint.OnCollisionEnter += StartActivateSoundSound;
        _emitter.Activated += OnActivated;
    }

    private void OnDisable()
    {
        _checkPoint.OnCollisionEnter -= StartActivateSoundSound;
        _emitter.Activated -= OnActivated;
    }

    private void OnActivated(bool isActive)
    {
        if (_isActive)
        {
            _emitter.Params[0].Value = 1;
        }
    }

    private void StartActivateSoundSound(CheckPoint checkPoint)
    {
        _emitter.SetParameter(_emitter.Params[0].Name, 1);
        RuntimeManager.PlayOneShotAttached(aSound, gameObject);
        _isActive = true;
    }
}
