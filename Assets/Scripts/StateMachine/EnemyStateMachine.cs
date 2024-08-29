using System.Collections.Generic;
using UnityEngine;


public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] private State _firstState;

    [SerializeField] private List<State> _states = new List<State>();

    private State _currentState;
    private Transform _target;

    public State CurrentState => _currentState;

    private void Start()
    {
        Reset(_firstState);
    }

    private void Update()
    {
        if (_currentState == null)
        {
            return;
        }

        var nextState = _currentState.GetNextState();

        if (nextState != null)
        {
            Transit(nextState);
        }
    }

    public void Init(Transform target)
    {
        _target = target;
        Reset(_firstState);
    }

    public void FullReset()
    {
        _target = null;

        foreach (var state in _states)
        {
            state.DeleteTarget();
        }
    }

    private void Reset(State startState)
    {
        _currentState = startState;
        
        if (_currentState != null)
        {
            _currentState.Enter(_target);
        }
    }

    private void Transit(State nextState)
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }

        _currentState = nextState;

        if (_currentState != null)
        {
            _currentState.Enter(_target);
        }
    }
}
