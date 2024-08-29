using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrulState : State
{
    [SerializeField] private List<PatrulPoint> _patrulPoints = new List<PatrulPoint>();
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _minDistance;

    private PatrulPoint _currentTargerPoint;
    private int _currentTargerPointIndex;

    private void Start()
    {
        _currentTargerPoint = _patrulPoints[_currentTargerPointIndex];
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _currentTargerPoint.transform.position) > _minDistance)
        {
            _agent.SetDestination(_currentTargerPoint.transform.position);
        }
        else
        {
            SetNextTargetPoint();
        }
    }

    private void SetNextTargetPoint()
    {
        if (_currentTargerPointIndex + 1 ==  _patrulPoints.Count)
        {
            _currentTargerPointIndex = 0;
        }
        else
        {
            _currentTargerPointIndex++;
        }

        _currentTargerPoint = _patrulPoints[_currentTargerPointIndex];
    }
}
