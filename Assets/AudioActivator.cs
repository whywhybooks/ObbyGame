using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioActivator : MonoBehaviour
{
    [SerializeField] private LayerMask _audioSourceMask;
    [SerializeField] private Vector3 _boxSize;
    [SerializeField] private Transform _collisionCenter;

    private Collider[] _hits;

    private void FixedUpdate()
    {
        _hits = Physics.OverlapBox(_collisionCenter.position, _boxSize / 2);

        if ( _hits.Length > 0 )
        {
            foreach ( Collider hit in _hits )
            {
                hit.gameObject.SetActive(true);
            }
        }
    }
}

public class AudioSourceForFMOD : MonoBehaviour
{
    [SerializeField] private LayerMask _audioActivatorMask;
    [SerializeField] private float _radius;
    [SerializeField] private Transform _collisionCenter;

    private Collider[] _hits;

    private void FixedUpdate()
    {
        _hits = Physics.OverlapSphere(_collisionCenter.position, _radius);

        if (Physics.CheckSphere(_collisionCenter.position, _radius, _audioActivatorMask) == false)
        {
            gameObject.SetActive(false);
        }
    }
}

