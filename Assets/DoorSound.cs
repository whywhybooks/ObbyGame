using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSound : MonoBehaviour
{
    [SerializeField] private Door _doors;
    [SerializeField] private EventReference _lockSound;
    [SerializeField] private EventReference _openSound;

    private void OnEnable()
    {
        _doors.Unlocked += StartActivateSound;
    }

    private void OnDisable()
    {
        _doors.Unlocked -= StartActivateSound;
    }

    private void StartActivateSound()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(_lockSound, _doors.gameObject);
        
        FMODUnity.RuntimeManager.PlayOneShotAttached(_openSound, _doors.LeftDoor.gameObject);
        FMODUnity.RuntimeManager.PlayOneShotAttached(_openSound, _doors.RightDoor.gameObject);
    }
}
