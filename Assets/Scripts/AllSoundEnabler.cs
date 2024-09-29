using FMODUnity;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AllSoundEnabler : MonoBehaviour
{
    [SerializeField] private Button _muteButton;

    [SerializeField][FMODUnity.ParamRef] private string aGlobalParameter;

    [SerializeField] private string MusicOn;
    [SerializeField] private string MusicOff;

    private bool _isActive;

    public event UnityAction<bool> SwitchMusic;

    private void OnEnable()
    {
        _muteButton.onClick.AddListener(SwitchSound);
    }

    private void OnDisable()
    {
        _muteButton.onClick.RemoveListener(SwitchSound);
    }

    private void SwitchSound()
    {
        if (_isActive)
        {
            Off();
        }
        else
        {
            On();
        }

        SwitchMusic?.Invoke(_isActive);
    }

    private void On()
    {
        RuntimeManager.StudioSystem.setParameterByNameWithLabel(aGlobalParameter, MusicOn);
        _isActive = true;
    }

    private void Off()
    {
        RuntimeManager.StudioSystem.setParameterByNameWithLabel(aGlobalParameter, MusicOff);
        _isActive = false;
    }
}