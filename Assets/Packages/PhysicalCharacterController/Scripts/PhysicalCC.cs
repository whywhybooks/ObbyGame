using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PhysicalCC : MonoBehaviour
{
	public CharacterController cc { get; private set; }
	private IEnumerator dampingCor;

	[Header("Fixed check")]
	public LayerMask _fixedMask;
	public Vector3 _boxScale;
	public float _checkFixedDistance;

	[Header("Ground Check")]
	public Transform _legs;
	public float _radius;
	public bool isGround;
	public float groundAngle;
	public Vector3 groundNormal { get; private set; }

	[Header("Movement")]
	public bool ProjectMoveOnGround;
	public Vector3 moveInput;
	private Vector3 moveVelocity;
    public Vector3 externalVelocity;
	public float gravity;

    private bool longJumpActive;
    private bool stopGravity;
	private float longJumpDelayTime;

    [Header("Slope and inertia")]
	public float slopeLimit = 45;
	public float inertiaDampingTime = 0.1f;
	public float slopeStartForce = 3f;
	public float slopeAcceleration = 3f;
	public Vector3 inertiaVelocity;

	[Header("interaction with the platform")]
	public bool platformAction;
	public Vector3 platformVelocity;

	[Header("Collision")]
	public bool applyCollision = true;
	public float pushForce = 55f;
	public bool collisionWithFixator = true;

	private void Start()
	{
		cc = GetComponent<CharacterController>();
	}

	private void FixedUpdate()
	{
		GroundCheck();
		FixedCheck();

		if (isGround)
		{
			moveVelocity = ProjectMoveOnGround? Vector3.ProjectOnPlane (moveInput, groundNormal) : moveInput;

			if (groundAngle < slopeLimit && inertiaVelocity != Vector3.zero) InertiaDamping();
		}

		GravityUpdate();

		Vector3 moveDirection = (moveVelocity + inertiaVelocity + platformVelocity + externalVelocity);

		cc.Move(moveDirection * Time.deltaTime);
	}

	private void GravityUpdate()
	{
		if (isGround && groundAngle > slopeLimit)
		{
			inertiaVelocity += Vector3.ProjectOnPlane(groundNormal.normalized + (Vector3.down * (groundAngle / 30)).normalized * Mathf.Pow(slopeStartForce, slopeAcceleration), groundNormal) * Time.deltaTime;
		}
		else if (!isGround)
		{
			if (!stopGravity)
			{
				float previousInertialVelocityY = 0;
                inertiaVelocity.y -= gravity * Time.deltaTime;

                if (previousInertialVelocityY > cc.velocity.y && longJumpActive == true)
                {
					inertiaVelocity.y = 0;
                    StartCoroutine(LongJump());
                    stopGravity = true;
                }

                previousInertialVelocityY = cc.velocity.y;
            }
		}
	}

	public void PlayLongJump(float longJumpDelayTime)
	{
		this.longJumpDelayTime = longJumpDelayTime;

       // inertiaVelocity.y = 25;
        longJumpActive = true;
    }

	private IEnumerator LongJump()
	{
		float previousInertialVelocityY = inertiaVelocity.y;
        inertiaVelocity.y = 0;

		yield return new WaitForSeconds(longJumpDelayTime);

		inertiaVelocity.y = previousInertialVelocityY;
		stopGravity = false;
		longJumpActive = false;
    }

	private void InertiaDamping()
	{
		var a = Vector3.zero;

		//inertia braking when the force of movement is applied
		var resistanceAngle = Vector3.Angle(Vector3.ProjectOnPlane(inertiaVelocity, groundNormal),
		Vector3.ProjectOnPlane(moveVelocity, groundNormal));

		resistanceAngle = resistanceAngle == 0 ? 90 : resistanceAngle;

		//НИЖЕ ПРОБЛЕМА!!
		inertiaVelocity = (inertiaVelocity + moveVelocity).magnitude <= 0.1f ? Vector3.zero : Vector3.SmoothDamp(inertiaVelocity, Vector3.zero, ref a, inertiaDampingTime / (3 / (180 / resistanceAngle)));
	}

	private void GroundCheck()
	{
		//if (Physics.SphereCast(_legs.position, _radius, Vector3.down, out RaycastHit hit, cc.height / 2 - _radius + 0.01f))

		if (Physics.SphereCast(_legs.position, _radius, Vector3.down, out RaycastHit hit, _radius))
		{
			isGround = true;
			//inertiaVelocity = Vector2.zero;
            groundAngle = Vector3.Angle(Vector3.up, hit.normal);
			groundNormal = hit.normal;

			if (hit.transform.tag == "Platform")
				platformVelocity = hit.collider.attachedRigidbody == null | !platformAction ?
				 Vector3.zero : hit.collider.attachedRigidbody.velocity;

			if (Physics.BoxCast(transform.position, new Vector3(cc.radius / 2.5f, cc.radius / 3f, cc.radius / 2.5f),
						Vector3.down, out RaycastHit helpHit, transform.rotation, cc.height / 2 - cc.radius / 2))
			{
				groundAngle = Vector3.Angle(Vector3.up, helpHit.normal);
			}
		}
		else
		{
			platformVelocity = Vector3.zero;
			isGround = false;
		}

		/*if (collisionWithFixator == false && transform.parent != null)
		{
            transform.parent = null;
        }*/

    }

	private void FixedCheck()
	{
		Collider[] hits = Physics.OverlapBox(transform.position, _boxScale / 2, transform.rotation, _fixedMask);

		if (hits.Length > 0)
		{
			transform.parent = hits[0].transform.parent;
		}
		else if (hits.Length == 0 && transform.parent != null)
		{
			transform.parent = null;
		}
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{/*
		if (hit.collider.TryGetComponent(out Fixator fixator) && hit.normal == Vector3.up)
		{
			transform.parent = fixator.transform.parent;
			collisionWithFixator = true;
        }
		else
		{
            collisionWithFixator = false;
        }*/

        if (!applyCollision) return;

        Rigidbody body = hit.collider.attachedRigidbody;

        // check rigidbody
        if (body == null || body.isKinematic) return;

        //Vector3 pushDir = hit.point - (hit.point + hit.moveDirection.normalized);
        Vector3 pushDir = -hit.moveDirection.normalized;

        // Apply the push
        body.AddForce(pushDir * pushForce, ForceMode.Force);
    }

	private void OnDrawGizmos()
	{
		Gizmos.color = new Color(0, 1, 1, 0.5f);
		Gizmos.DrawSphere(_legs.position, _radius);
	}
}
