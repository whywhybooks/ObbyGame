using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] private Axis _axis;
    [SerializeField] private bool _isRevers;
    [SerializeField] private float _delay;
    [SerializeField] private Transform _movementObject;
    [SerializeField] private float _offset;
    [SerializeField] private float _speed;

    private const float tau = Mathf.PI * 2;

    private Vector3 _targetPositionAxis;
    float _cycles, _rawSinWave, _movementFactor;
    Vector3 _startPos, _offsetPosition;
    private Vector3 _startPositionGizmos;
    private Vector3 _finalPositionGizmos;
    private float _elapsedTime;

    private void OnValidate()
    {
        Initialize();
    }

    private void Initialize()
    {
        switch (_axis)
        {
            case Axis.X:
                _targetPositionAxis = new Vector3(_offset, 0, 0);
                break;

            case Axis.Y:
                _targetPositionAxis = new Vector3(0, _offset, 0);
                break;

            case Axis.Z:
                _targetPositionAxis = new Vector3(0, 0, _offset);
                break;
        }

        _startPositionGizmos = transform.position;
        _finalPositionGizmos = transform.position + _targetPositionAxis;

        if (_isRevers)
        {
            _movementObject.transform.position = _finalPositionGizmos;
        }
        else
        {
            _movementObject.transform.position = _startPositionGizmos;
        }
    }


    private void Start()
    {
        Initialize();
        if (_isRevers)
        {
            _startPos = _finalPositionGizmos;
        }
        else
        {
            _startPos = _startPositionGizmos;
        }

        _movementObject.GetChild(0).AddComponent<Fixator>();

        switch (_axis)
        {
            case Axis.X:
                _targetPositionAxis = new Vector3(_offset, 0, 0);
                break;

            case Axis.Y:
                _targetPositionAxis = new Vector3(0, _offset, 0);
                break;

            case Axis.Z:
                _targetPositionAxis = new Vector3(0, 0, _offset);
                break;
        }

        _startPositionGizmos = transform.position;
        _finalPositionGizmos = transform.position + _targetPositionAxis;
    }

    private void FixedUpdate()
    {
        if (_delay > 0)
        {
            _delay -= Time.deltaTime;
            return;
        }

        _elapsedTime += Time.deltaTime;

        if (_speed <= 0) return;
        _cycles = _elapsedTime / _speed;
        _rawSinWave = Mathf.Sin(_cycles * tau);
        _movementFactor = _rawSinWave / 2f + 0.5f;
        _offsetPosition = _movementFactor * _targetPositionAxis;
        if (_isRevers)
            _movementObject.transform.position = _startPos - _offsetPosition;
        else
            _movementObject.transform.position = _startPos + _offsetPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 1f);
        Gizmos.DrawSphere(_startPositionGizmos, 0.5f);

        switch (_axis)
        {
            case Axis.X:
                _targetPositionAxis = new Vector3(_offset, 0, 0);
                break;

            case Axis.Y:
                _targetPositionAxis = new Vector3(0, _offset, 0);
                break;

            case Axis.Z:
                _targetPositionAxis = new Vector3(0, 0, _offset);
                break;
        }

        Gizmos.DrawSphere(_finalPositionGizmos, 0.5f);
        Gizmos.DrawLine(_finalPositionGizmos, _startPositionGizmos);
    }
}
