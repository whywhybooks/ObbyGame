using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Fan : MonoBehaviour
{
    [SerializeField] private float _flowForce;
    [SerializeField] private bool _isRevers;

    private PhysicalCC _player;

    [Header("Collision Parametrs")]
    [SerializeField] private Transform _collisionPoint;
    [SerializeField] private Vector3 _cubeSize;
    [SerializeField] private LayerMask _playerLayer;

    [Header("Gizmos")]
    [SerializeField] private Mesh _arrowDirectionMesh;

    private bool _isCollision;

    private void Awake()
    {
        _player = FindObjectOfType<PhysicalCC>();
    }


    private void FixedUpdate()
    {
        CheckCollision();
    }

    private void CheckCollision()
    {
        if (Physics.CheckBox(_collisionPoint.position, _cubeSize / 2, transform.rotation, _playerLayer))
        {
            _isCollision = true;
            _player.externalVelocity = _isRevers ? -transform.forward.normalized : transform.forward.normalized;
            _player.externalVelocity *= _flowForce;
        }
        else
        {
            if (_isCollision)
            {
                _isCollision = false;
                _player.externalVelocity = Vector3.zero;
            }    
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(_collisionPoint.position, _cubeSize);
        Gizmos.color = new Color(0, 0, 1, 0.7f);

        if (_isRevers)
            Gizmos.DrawMesh(_arrowDirectionMesh, transform.position, Quaternion.Euler(new Vector3(0, transform.eulerAngles.y + 90, 0)), new Vector3(4, 4, 4)) ;
        else
            Gizmos.DrawMesh(_arrowDirectionMesh, transform.position, Quaternion.Euler(new Vector3(0, transform.eulerAngles.y + -90, 0)), new Vector3(4, 4, 4)) ;
    }
}
