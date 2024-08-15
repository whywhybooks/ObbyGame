using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class CharacterMovementController : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Camera _mainCamera;

    [Header("Controller parameters:")]
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _gravity = 9.8f;
    [SerializeField] private float _jumpForce = 8.0f;
    [SerializeField] private float _slopeForce = 5.0f;
    [SerializeField] private float _slopeRayLenght = 1.5f;
    [SerializeField] private float _rotationSpeed = 300.0f;

    private Vector3 _moveDirection = Vector3.zero;

    [Header("Check ground:")]
    [SerializeField] private Transform _legsPoint;
    [SerializeField] private float _checkGraoundRayLenght;
    [SerializeField] private LayerMask _groundMask;

    private bool _isGrounded;

    private void Update()
    {
        if (_isGrounded)
        {
            SetMoveDirection();

            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
        }

        Rotate();

        Debug.Log(_characterController.contactOffset);
        _moveDirection.y -= _gravity * Time.deltaTime;
        _characterController.Move(_moveDirection * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Slope();
        CheckGround();
    }

    private void CheckGround()
    {
        _isGrounded = Physics.CheckSphere(_legsPoint.position, _checkGraoundRayLenght, _groundMask);
    }

    private void Jump()
    {
        _moveDirection.y = _jumpForce;
    }

    private void Rotate()
    {
        if (_moveDirection.magnitude > 0)
        {
            transform.eulerAngles = new Vector3(0, _mainCamera.transform.eulerAngles.y, 0);
        }
    }

    private void Slope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, _slopeRayLenght) == false)
        {
            return;
        }

        if (Vector3.Angle(hit.normal, Vector3.up) > _characterController.slopeLimit)
        {
            _moveDirection.x += (1f - hit.normal.y) * hit.normal.x * _slopeForce;
            _moveDirection.z += (1f - hit.normal.y) * hit.normal.z * _slopeForce;
            _moveDirection.y -= _slopeForce;
        }
    }

    private void SetMoveDirection()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Math.Abs(horizontalInput) < 1) {
            horizontalInput = 0;
        }

        if (Math.Abs(verticalInput) < 1)
        {
            verticalInput = 0;
        }

        Vector3 inputDirection = new Vector3(horizontalInput, 0, verticalInput);

        inputDirection = transform.TransformDirection(inputDirection);
        _moveDirection = inputDirection * _speed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(_legsPoint.position, _checkGraoundRayLenght);
    }
}