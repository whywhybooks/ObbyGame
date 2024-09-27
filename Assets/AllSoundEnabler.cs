using FMODUnity;
using UnityEngine;

public class AllSoundEnabler : MonoBehaviour
{
    [SerializeField][FMODUnity.ParamRef] private string aGlobalParameter;

    [SerializeField] private string MusicOn;
    [SerializeField] private string MusicOff;

    public void On()
    {
        RuntimeManager.StudioSystem.setParameterByNameWithLabel(aGlobalParameter, MusicOn);
    }

    public void Off()
    {
        RuntimeManager.StudioSystem.setParameterByNameWithLabel(aGlobalParameter, MusicOff);
    }
}
