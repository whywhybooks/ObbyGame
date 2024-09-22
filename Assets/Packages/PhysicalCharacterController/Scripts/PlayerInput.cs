using System;
using System.Collections;
using System.Collections.Generic;
using TouchControlsKit;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
	[Header("Main parametrs")]
	[SerializeField] private bool _debugMode;
	[SerializeField] private Animator _animator;
	[SerializeField] private Transform _camera;
	public float speed = 5;
	public float jumpHeight = 15;
	public PhysicalCC physicalCC;
    float m_TurnAmount;
	float m_ForwardAmount;
    public float turnSpeed = 10f;

    float horizontalInput = 0;
    float verticalInput = 0;

    public Transform bodyRender;
	IEnumerator sitCort;
	public bool isSitting;

	public event UnityAction OnJump;

    void Update()
	{
		if (physicalCC.isGround)
		{
			if (_debugMode)
			{
				horizontalInput = Input.GetAxis("Horizontal");
				verticalInput = Input.GetAxis("Vertical");


                if (Math.Abs(horizontalInput) < 1)
                {
                    horizontalInput = 0;
                }

                if (Math.Abs(verticalInput) < 1)
                {
                    verticalInput = 0;
                }
            }
			else
			{
                Vector2 move = TCKInput.GetAxis("Joystick");


                 if (Input.GetAxis("Horizontal") != 0)
                     horizontalInput = Input.GetAxis("Horizontal");
                 else if (move.x != 0)
                     horizontalInput = move.x;

                 if (Input.GetAxis("Vertical") != 0)
                     verticalInput = Input.GetAxis("Vertical");
                 else if (move.y != 0)
                     verticalInput = move.y;
            }

            physicalCC.moveInput = Vector3.ClampMagnitude(_camera.forward
							* verticalInput
                            + transform.right
							* horizontalInput, 1f) * speed;

	    	if (physicalCC.moveInput.magnitude > 1)
			{
				_animator.SetBool("IsRun", true);
			}
			else
			{
				_animator.SetBool("IsRun", false);
			}
			//physicalCC.moveInput.y = 0f;

            if (Input.GetKeyDown(KeyCode.Space) || TCKInput.GetAction("jumpBtn", EActionEvent.Down))
            {
                physicalCC.inertiaVelocity.y = 0f;
				physicalCC.inertiaVelocity.y += jumpHeight;
				_animator.SetTrigger("IsJump");
				OnJump?.Invoke();

            }

            if (Input.GetKeyDown(KeyCode.C) && sitCort == null)
			{
				sitCort = sitDown();
				StartCoroutine(sitCort);
			
			}
		}
        //	m_ForwardAmount = physicalCC.moveInput.z;
        //  m_TurnAmount = Mathf.Atan2(physicalCC.moveInput.x, physicalCC.moveInput.z);
        //ApplyExtraTurnRotation();


        // Плавное вращение игрока в сторону движения
	/*	float targetAngle = Mathf.Atan2(horizontalInput, verticalInput) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSpeed, 0.1f);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);*/
    }

    void ApplyExtraTurnRotation()
    {
        float turnSpeed = Mathf.Lerp(100, 360, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }

    public void LongJump(float longJumpHeight, float delayTime)
	{
        physicalCC.inertiaVelocity.y = 0f;
        physicalCC.inertiaVelocity.y += longJumpHeight;
        physicalCC.PlayLongJump(delayTime);
    }

	public void BoostSpeed(float multiplier)
	{
		speed *= multiplier;
		_animator.SetFloat("Speed", 1.5f);
	}

    public void RemoveAcceleration(float multiplier)
    {
        speed /= multiplier;
        _animator.SetFloat("Speed", 1);
    }

    IEnumerator sitDown()
	{
		if (isSitting && Physics.Raycast(transform.position, Vector3.up, physicalCC.cc.height * 1.5f))
		{
			sitCort = null;
			yield break;
		}
		isSitting = !isSitting;

		float t = 0;
		float startSize = physicalCC.cc.height;
		float finalSize = isSitting ? physicalCC.cc.height / 2 : physicalCC.cc.height * 2;

		Vector3 startBodySize = bodyRender.localScale;
		Vector3 finalBodySize = isSitting ? bodyRender.localScale - Vector3.up * bodyRender.localScale.y / 2f : bodyRender.localScale + Vector3.up * bodyRender.localScale.y;

		

		speed = isSitting ? speed / 2 : speed * 2;
		jumpHeight = isSitting ? jumpHeight * 3 : jumpHeight / 3;
		
		while (t < 0.2f)
		{
			t += Time.deltaTime;
			physicalCC.cc.height = Mathf.Lerp(startSize, finalSize, t / 0.2f);
			bodyRender.localScale = Vector3.Lerp(startBodySize, finalBodySize, t / 0.2f);
			yield return null;
		}

		sitCort = null;
		yield break;
	}
}
