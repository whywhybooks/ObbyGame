using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllSoundEnabler : MonoBehaviour
{
    [SerializeField][FMODUnity.ParamRef] private string aGlobalParameter;

    [SerializeField] private string MusicOn;
    [SerializeField] private string MusicOff;

    private void Start()
    {
        RuntimeManager.StudioSystem.setParameterByNameWithLabel(aGlobalParameter, MusicOff);
    }
}
