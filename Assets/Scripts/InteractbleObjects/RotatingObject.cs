using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [SerializeField] private Axis _axis;
    [SerializeField] private Transform _fulcrum;
    [SerializeField] private Transform _rotatedObject;
    [SerializeField] private float _speed;

    private Vector3 _targetRotationAxis;

    private void Start()
    {
        _rotatedObject.parent = _fulcrum;
        _rotatedObject.GetChild(0).AddComponent<Fixator>();

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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 1f);
        Gizmos.DrawSphere(_fulcrum.position, 0.5f);
    }
}
