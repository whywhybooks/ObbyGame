using System;
using System.Collections;
using System.Collections.Generic;
using TouchControlsKit;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	[Header("Main parametrs")]
	[SerializeField] private bool _debugMode;
	[SerializeField] private AnimationCurve _jumpCurve;
	public float speed = 5;
	public float jumpHeight = 15;
	public PhysicalCC physicalCC;

	public Transform bodyRender;
	IEnumerator sitCort;
	public bool isSitting;

    void Update()
	{
		if (physicalCC.isGround)
		{
			float horizontalInput = 0;

			float verticalInput = 0;


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


            physicalCC.moveInput = Vector3.ClampMagnitude(transform.forward
							* verticalInput
                            + transform.right
							* horizontalInput, 1f) * speed;
			physicalCC.moveInput.y = 0f;
			Debug.Log(physicalCC.moveInput.y);

            if (Input.GetKeyDown(KeyCode.Space) || TCKInput.GetAction("jumpBtn", EActionEvent.Down))
            {
                physicalCC.inertiaVelocity.y = 0f;
                physicalCC.inertiaVelocity.y += jumpHeight;
            }

            if (Input.GetKeyDown(KeyCode.C) && sitCort == null)
			{
				sitCort = sitDown();
				StartCoroutine(sitCort);
			}
		}
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
	}

    public void RemoveAcceleration(float multiplier)
    {
        speed /= multiplier;
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
