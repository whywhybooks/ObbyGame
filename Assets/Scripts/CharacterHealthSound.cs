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
        _characterHealth.OnRevive += Revive;
    }

    private void Start()
    {
        //instance.start();
    }

    private void OnDisable()
    {
        _characterHealth.OnDiedOfShock -= PlayShockSound;
        _characterHealth.OnDiedFromFall -= PlayVoiceSound;
        _characterHealth.OnRevive -= Revive;
    }

    private void Revive()
    {
        RuntimeManager.PlayOneShotAttached(_reviveSound, gameObject);
    }

    private void PlayShockSound()
    {
        RuntimeManager.PlayOneShotAttached(_shokSound, gameObject);
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
            RuntimeManager.PlayOneShotAttached(_manVoiceSound, gameObject);
        }
        else
        {
            RuntimeManager.PlayOneShotAttached(_girlVoiceSound, gameObject);
        }
    }
}
