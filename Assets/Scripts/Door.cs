﻿using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    [Header("Door parametrs:")]
    [SerializeField] private int _targetKeys;
    [SerializeField] private Transform _leftDoor;
    [SerializeField] private Transform _rightDoor;
    [SerializeField] private float _openingTime;
    [SerializeField] private float _offset;

    [Header("Collision parametrs:")]
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Transform _triggerCollider;

    private bool _isOpen;
    private bool _isCollision;
    private Animator _animator;
    private CharacterKeys _characterKeys;

    public int TargetKeys { get => _targetKeys; private set => _targetKeys = value; }
    public Transform LeftDoor { get => _leftDoor; private set => _leftDoor = value; }
    public Transform RightDoor { get => _rightDoor; private set => _rightDoor = value; }

    public event UnityAction Unlocked;
    public event UnityAction Show;
    public event UnityAction Hide;

    private void Start()
    {
        _triggerCollider.GetComponent<MeshRenderer>().enabled = false;
    }

    private void FixedUpdate()
    {
        CheckCollision();
    }

    private void CheckCollision()
    {
        if (_isOpen)
            return;

        if (Physics.CheckBox(_triggerCollider.position, _triggerCollider.localScale / 2, transform.rotation, _playerLayer))
        {
            if (_isCollision == false)
            {
                _isCollision = true;

                if (_characterKeys.TryOpenDoor(_targetKeys))
                {
                    StartCoroutine(Open());
                    Unlocked?.Invoke();
                }
                else
                {
                    Show?.Invoke();
                }
            }
        }
        else
        {
            if (_isCollision == true)
            {
                _isCollision = false;
                Hide?.Invoke();
            }
        }
    }

    private IEnumerator Open()
    {
        float elapsedTime = 0;

        Vector3 leftDoorOfsetPosition = new Vector3(_leftDoor.localPosition.x - _offset, _leftDoor.localPosition.y, _leftDoor.localPosition.z);
        Vector3 rightDoorOfsetPosition = new Vector3(_rightDoor.localPosition.x + _offset, _rightDoor.localPosition.y, _rightDoor.localPosition.z);

        Vector3 leftDoorStartPosition = _leftDoor.localPosition;
        Vector3 rightDoorStartPosition = _rightDoor.localPosition;

        _isOpen = true;

        while (elapsedTime <= _openingTime)
        {
            elapsedTime += Time.deltaTime;
            _leftDoor.localPosition = Vector3.Lerp(leftDoorStartPosition, leftDoorOfsetPosition, elapsedTime / _openingTime);
            _rightDoor.localPosition = Vector3.Lerp(rightDoorStartPosition, rightDoorOfsetPosition, elapsedTime / _openingTime);
            yield return null;
        }
    }

    public void Initialize(CharacterKeys characterKeys)
    {
        _characterKeys = characterKeys;
    }
}

