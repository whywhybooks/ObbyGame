using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckPoint : MonoBehaviour
{
    [Header("Collision Parametrs")]
    [SerializeField] private Transform _boxCollider;
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

    private void Start()
    {
        _boxCollider.GetComponent<MeshRenderer>().enabled = false;
    }

    public void Initialize(int index)
    {
        Index = index;
    }

    private void CheckCollision()
    {
        if (Physics.CheckBox(_boxCollider.position, _boxCollider.lossyScale / 2, transform.rotation, _characterLayer))
        {

            if (_activated == false)
            {
                OnCollisionEnter?.Invoke(this);
                _activated = true;
            }
        }
        else
        {
            _activated = false;
        }
    }
}
