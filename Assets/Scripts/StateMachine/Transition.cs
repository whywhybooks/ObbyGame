using System;
using UnityEngine;

public abstract class Transition : MonoBehaviour
{
    [SerializeField] private State _targetState;

    protected Transform Target { get; private set; }

    public State TargetState => _targetState;
    public bool NeedTransit { get; protected set; }

    public void Init(Transform target)
    {
        Target = target;
    }

    private void OnEnable()
    {
        NeedTransit = false;
    }

    public void DeleteTarget()
    {
        Target = null;
    }
}
