using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckPoint : MonoBehaviour
{
    [Header("Collision Parametrs")]
    [SerializeField] private Transform _collisionPoint;
    [SerializeField] private Vector3 _cubeSize;
    [SerializeField] private LayerMask _characterLayer;

    [Header("CheckPoint Parametrs")]
    [SerializeField] private Transform _restartPoint;

    private bool _activated;
    public int Index { get; private set; }
    public Vector3 RestartPosition => _restartPoint.position;

    public event UnityAction<CheckPoint> OnCollisionEnter;

    private void FixedUpdate()
    {
        CheckCollision();
    }

    public void Initialize(int index)
    {
        Index = index;
    }

    private void CheckCollision()
    {
        if (Physics.CheckBox(_collisionPoint.position, _cubeSize / 2, transform.rotation, _characterLayer))
        {

            if (_activated == false)
            {
            Debug.Log(123);
                OnCollisionEnter?.Invoke(this);
                _activated = true;
            }
        }
        else
        {
            _activated = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(_collisionPoint.position, _cubeSize);
    }
}
