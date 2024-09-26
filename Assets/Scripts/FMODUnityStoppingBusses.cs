using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODUnityStoppingBusses : MonoBehaviour
{
    //[SerializeField] EventReference aSound;

    private FMOD.Studio.Bus theBusASoundPassesThrough;

    //private void Awake()
    //{
    //    FMODUnity.RuntimeManager.PlayOneShot(aSound);
    //    theBusASoundPassesThrough = RuntimeManager.GetBus("Bus:/SomeCaseSensitiveBus");
    //}

    private void StopEntireBus()
    {
        theBusASoundPassesThrough.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        theBusASoundPassesThrough.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        theBusASoundPassesThrough.setPaused(true);
    }
}