using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] private Axis _axis;
    [SerializeField] private Transform _movementObject;
    [SerializeField] private float _offset;
    [SerializeField] private float _speed;

    private const float tau = Mathf.PI * 2;

    [Header("Collision Parametrs")]
    [SerializeField] private Transform _collisionPoint;
    [SerializeField] private Vector3 _cubeSize;
    [SerializeField] private LayerMask _characterLayer;

    private Transform _childObject;
    private Collider[] _collision;
    private Vector3 _targetPositionAxis;
    float _cycles, _rawSinWave, _movementFactor;
    Vector3 _startPos, _offsetPosition;
    private Vector3 _startPositionGizmos;
    private Vector3 _finalPositionGizmos;

    private void OnValidate()
    {
        _startPositionGizmos = transform.position;

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

        _finalPositionGizmos = transform.position + _targetPositionAxis;
    }


    private void Start()
    {
        _startPos = transform.position;

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
        if (_speed <= 0) return;
        _cycles = Time.time / _speed;
        _rawSinWave = Mathf.Sin(_cycles * tau);
        _movementFactor = _rawSinWave / 2f + 0.5f;
        _offsetPosition = _movementFactor * _targetPositionAxis;
        transform.position = _startPos + _offsetPosition;

        CheckCollision();
    }

    private void CheckCollision()
    {
        _collision = Physics.OverlapBox(_collisionPoint.position, _cubeSize / 2, _movementObject.rotation, _characterLayer);

        if (_collision.Length > 0)
        {
            _collision[0].transform.parent = _movementObject.transform;
            _childObject = _collision[0].transform;
        }
        else
        {
            if (_childObject != null)
            {
                _childObject.transform.parent = null;
                _childObject = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(_collisionPoint.position, _cubeSize);

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
