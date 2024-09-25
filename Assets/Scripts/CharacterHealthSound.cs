using FMOD;
using FMODUnity;
using UnityEngine;

public class CharacterHealthSound : MonoBehaviour
{
    [SerializeField] private CharacterHealth _characterHealth;
    [SerializeField] private CharacterTypeChanger _characterTypeChanger;
    [SerializeField] private EventReference _shokSound;
    [SerializeField] private EventReference _reviveSound;
    [SerializeField] private EventReference _voiceSound;
    [SerializeField] string _genderParametrName;

    private FMOD.Studio.EventInstance instance;

    private void OnEnable()
    {
        _characterHealth.OnDiedOfShock += PlayShockSound;
        _characterHealth.OnDiedFromFall += PlayVoiceSound;
        _characterHealth.OnDied += Revive;
    }

    private void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(_voiceSound);
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
        CheckGender();
        FMODUnity.RuntimeManager.PlayOneShotAttached(_voiceSound, gameObject);
    }

    private void PlayVoiceSound()
    {
        CheckGender();
        FMODUnity.RuntimeManager.PlayOneShotAttached(_voiceSound, gameObject);
    }

    private void CheckGender()
    {
        if (_characterTypeChanger.CharacterType == CharacterType.Man)
        {
            instance.setParameterByName(_genderParametrName, 1);
        }
        else
        {
            instance.setParameterByName(_genderParametrName, 0);
        }
    }
}
