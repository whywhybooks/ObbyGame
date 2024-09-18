using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSound : MonoBehaviour
{
    [SerializeField][FMODUnity.EventRef] private string _eventJump;
    [SerializeField][FMODUnity.EventRef] private string _eventLanding;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private PhysicalCC _physicalCC;

    private void OnEnable()
    {
        _playerInput.OnJump += PlayJump;
        _physicalCC.OnGround += PlayLanding;
    }

    private void OnDisable()
    {
        _playerInput.OnJump -= PlayJump;
        _physicalCC.OnGround -= PlayLanding;
    }

    public void PlayJump()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(_eventJump, gameObject);
    }

    public void PlayLanding()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(_eventLanding, gameObject);
    }
}
