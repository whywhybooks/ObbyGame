using UnityEngine;

public class StepSound : MonoBehaviour
{
    [SerializeField][FMODUnity.EventRef] private string _eventStep;

    public void PlayStep()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(_eventStep, gameObject);
    }
}
