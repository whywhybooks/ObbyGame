using FMODUnity;
using UnityEngine;

public class CharacterHealthSound : MonoBehaviour
{
    [SerializeField] private CharacterHealth _characterHealth;
    [SerializeField] private EventReference _shokSound;
    [SerializeField] private EventReference _voiceSound;
    [SerializeField] private EventReference _reviveSound;

    private void OnEnable()
    {
        _characterHealth.OnDiedOfShock += PlayShockSound;
        _characterHealth.OnDiedFromFall += PlayVoiceSound;
        _characterHealth.OnDied += Revive;
    }

    private void OnDisable()
    {
        _characterHealth.OnDiedOfShock -= PlayShockSound;
        _characterHealth.OnDiedFromFall -= PlayVoiceSound;
        _characterHealth.OnDied -= Revive;
    }

    private void Revive()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(_reviveSound, gameObject);
    }

    private void PlayShockSound()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(_shokSound, gameObject);
        FMODUnity.RuntimeManager.PlayOneShotAttached(_voiceSound, gameObject);
    }

    private void PlayVoiceSound()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(_voiceSound, gameObject);
    }
}
