using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [SerializeField] private Axis _axis;
    [SerializeField] private Transform _fulcrum;
    [SerializeField] private Transform _rotatedObject;
    [SerializeField] private float _speed;

    [Header("Collision Parametrs")]
    [SerializeField] private Transform _collisionPoint;
    [SerializeField] private Vector3 _cubeSize;
    [SerializeField] private LayerMask _characterLayer;

    private Transform _childObject;
    private Collider[] _collision;
    private Vector3 _targetRotationAxis;

    private void Start()
    {
        _rotatedObject.parent = _fulcrum;

        switch (_axis)
        {
            case Axis.X:
                _targetRotationAxis = new Vector3(_speed, 0, 0);
                break;

            case Axis.Y:
                _targetRotationAxis = new Vector3(0, _speed, 0);
                break;

            case Axis.Z:
                _targetRotationAxis = new Vector3(0, 0, _speed);
                break;
        }
    }

    private void FixedUpdate()
    {
        _fulcrum.Rotate(_targetRotationAxis * Time.fixedDeltaTime);
         CheckCollision();
    }

    private void CheckCollision()
    {
        _collision = Physics.OverlapBox(_collisionPoint.position, _cubeSize / 2, _rotatedObject.rotation, _characterLayer);

        if (_collision.Length > 0)
        {
            _collision[0].transform.parent = _rotatedObject.transform;
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
        Gizmos.color = new Color(0, 0, 1, 1f);
        Gizmos.DrawSphere(_fulcrum.position, 0.5f);
        
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(_collisionPoint.position, _cubeSize);
    }
}
