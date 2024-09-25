using FMOD;
using FMODUnity;
using UnityEngine;

public class CharacterHealthSound : MonoBehaviour
{
    [SerializeField] private CharacterHealth _characterHealth;
    [SerializeField] private CharacterTypeChanger _characterTypeChanger;
    [SerializeField] private EventReference _shokSound;
    [SerializeField] private EventReference _reviveSound;
    [SerializeField] private EventReference _manVoiceSound;
    [SerializeField] private EventReference _girlVoiceSound;
    [SerializeField] string _genderParametrName;

    private void OnEnable()
    {
        _characterHealth.OnDiedOfShock += PlayShockSound;
        _characterHealth.OnDiedFromFall += PlayVoiceSound;
        _characterHealth.OnDied += Revive;
    }

    private void Start()
    {
        //instance.start();
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
        PlayVoice();
    }

    private void PlayVoiceSound()
    {
        PlayVoice();
    }

    private void PlayVoice()
    {
        if (_characterTypeChanger.CharacterType == CharacterType.Man)
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached(_manVoiceSound, gameObject);
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached(_girlVoiceSound, gameObject);
        }
    }
}
