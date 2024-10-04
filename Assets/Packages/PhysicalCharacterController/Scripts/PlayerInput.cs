using FIMSpace.Basics;
using System;
using System.Collections;
using TouchControlsKit;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
	[Header("Main parametrs")]
	[SerializeField] private bool _debugMode;
	[SerializeField] private Animator _animator;
	[SerializeField] private Transform _camera;
	[SerializeField] private CharacterHealth _characterHealth;
	[SerializeField] private TCKJoystick _joystick;
	[SerializeField, Range(0, 1)] private float _joystickDeadZone;
	[SerializeField] private Button _jumpButton;

	public float speed = 5;
    private float defaultSpeed;
    public float floataccelerationReductionInertia;
    public float jumpHeight = 15;
	public PhysicalCC physicalCC;
    float m_TurnAmount;
	float m_ForwardAmount;
    public float turnSpeed = 10f;
	private Vector3 _cameraForward;
	private Vector3 _cameraRight;

    float horizontalInput = 0;
    float verticalInput = 0;

	private bool m_IsMoving = false;
	private bool m_IsJump = true;
    [SerializeField] private float m_TimeForCoyoteJump;
	private float m_elapsedCoyoteJumpTime;

    public Transform bodyRender;
	IEnumerator sitCort;
	public bool isSitting;
	private Coroutine _smoothRemoveAccelerationCoroutine;


    public event UnityAction OnJump;

    private void OnEnable()
    {
		_joystick.OnPointerUpEvent += DisableMove;
		_joystick.OnPointerDownEvent += EnableMove;
    }

    private void Start()
    {
		defaultSpeed = speed;
    }

    private void OnDisable()
    {
        _joystick.OnPointerUpEvent -= DisableMove;
        _joystick.OnPointerDownEvent -= EnableMove;
    }

    private void EnableMove()
    {
		m_IsMoving = true;
    }

    private void DisableMove()
    {
		m_IsMoving = false;
        horizontalInput = 0;
        verticalInput = 0;
    }

	public void Jump()
    {
		if (m_IsJump == false) 
		{
            physicalCC.inertiaVelocity.y = 0f;
			physicalCC.inertiaVelocity.y += jumpHeight;
			_animator.SetTrigger("IsJump");
			OnJump?.Invoke();
			m_IsJump = true;
        }
    }

    void Update()
	{
		if (_characterHealth.IsDied)
		{
			physicalCC.moveInput = Vector3.zero;
			_joystick.ResetAxes();
			horizontalInput = 0;
			verticalInput = 0;
			return;
		}

        if (physicalCC.isGround == false && physicalCC.LongJumpActive == false)
        {
            m_elapsedCoyoteJumpTime += Time.deltaTime;


            if (m_elapsedCoyoteJumpTime > m_TimeForCoyoteJump)
            {
                m_IsJump = true;
                m_elapsedCoyoteJumpTime = 0;
            }
        }

        if (m_IsJump == true)
		{
			if (physicalCC.isGround && physicalCC.inertiaVelocity.y < 0)
			{
                m_IsJump = false;
                m_elapsedCoyoteJumpTime = 0;

            }
		}

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

            if (m_IsMoving)
			{
				if (new Vector2 (move.x, move.y).magnitude > _joystickDeadZone * _joystick.sensitivity)
				{
                    horizontalInput = move.x;
                    verticalInput = move.y;
                }
                else
                {
                    horizontalInput = 0;
                    verticalInput = 0;
                }
            }
        }

			_cameraForward = new Vector3(_camera.transform.forward.x, 0, _camera.transform.forward.z);
			_cameraRight = new Vector3(_camera.transform.right.x, 0, _camera.transform.right.z);

            physicalCC.moveInput =  Vector3.ClampMagnitude(_cameraForward.normalized
                            * verticalInput
                            + _cameraRight
                            * horizontalInput, 1f) * speed;

            if (physicalCC.moveInput.magnitude > 1)
			{
				_animator.SetBool("IsRun", true);
				_animator.SetFloat("Speed", Math.Min(1.5f, speed * physicalCC.moveInput.magnitude / 40));//Это для спид бустера, чтобы скорость анимации была максимум 1.5
			}
			else
			{
				_animator.SetBool("IsRun", false);
			}
            //physicalCC.moveInput.y = 0f;

            if (Input.GetKeyDown(KeyCode.Space) || TCKInput.GetAction("jumpBtn", EActionEvent.Down))
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.C) && sitCort == null)
			{
				sitCort = sitDown();
				StartCoroutine(sitCort);
			
			}
	//	}

        physicalCC.SetRotation(physicalCC.moveInput);
    }


    public void LongJump(float longJumpHeight, float delayTime)
	{
        physicalCC.inertiaVelocity.y = 0f;
        physicalCC.inertiaVelocity.y += longJumpHeight;
        physicalCC.PlayLongJump(delayTime);
    }

	public void BoostSpeed(float multiplier)
	{
		if (_smoothRemoveAccelerationCoroutine != null)
		{
			StopCoroutine(_smoothRemoveAccelerationCoroutine);
			_smoothRemoveAccelerationCoroutine = null;
		}

		if (speed > defaultSpeed)
		{
			speed = defaultSpeed;
		}

		speed *= multiplier;
	//	_animator.SetFloat("Speed", 1.5f);
	}

	public void SmoothRemoveAcceleration(float multiplier)
	{
        _smoothRemoveAccelerationCoroutine = StartCoroutine(SmoothRemoveAccelerationCoroutine(multiplier));
        _animator.SetFloat("Speed", 1);
    }

    public void RemoveAcceleration(float multiplier)
    {
        speed /= multiplier;
       // _animator.SetFloat("Speed", 1);
    }

	private IEnumerator SmoothRemoveAccelerationCoroutine(float multiplier)
	{
		float targetSpeed = speed / multiplier;
		float currentSpeed = speed;
		float elapsedTime = 0;

		while (speed > targetSpeed)
		{
			elapsedTime += Time.deltaTime;
			speed = Mathf.MoveTowards(currentSpeed, targetSpeed, elapsedTime * floataccelerationReductionInertia);

			yield return null;
		}

		speed = targetSpeed;
		_smoothRemoveAccelerationCoroutine = null;
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

//class DF
//{
//	[SerializeField] private float m_TimeForCoyoteJump; //Устанавливаем время, за которое можно будет прожать Койот-джамп с момента схода с земли.

//	private float m_elapsedCoyoteJumpTime; //Прошедшее время с момента схода с земли.
//	private bool m_IsJump = true; //Становится false, когда мы стоим на земле. True когда нажали на кнопку прыжка
	
//	//Jump - Срабатывает, когда кликаем на кнопку прыжка
//	public void Jump()
//	{
//		if (m_IsJump == false)
//		{
//			//Ваша логика прыжка
//			m_IsJump = true;
//		}
//	}

//	public void Update()
//	{
//        if (/*игрок НЕ стоит на земле*/)
//        {
//            m_elapsedCoyoteJumpTime += Time.deltaTime; //считаем время для койот-джампа

//            if (m_elapsedCoyoteJumpTime > m_TimeForCoyoteJump) //Если время вышло, то 
//            {
//                m_IsJump = true; //Говорим, что мы прыгнули, тем самым забирая возможность прожать прыжок
//                m_elapsedCoyoteJumpTime = 0;//Обнуляем прошедшее время для койот джампа
//            }
//        }

//        if (m_IsJump == true) //Если было нажатие на кнопку прыжка
//        {
//            if (/*Игрок стоит на земле*/ && /*Velocity по Y меньше 0 (что сигнализирует о том, что игрок именно приземлился на платформу)*/) //Если ваш Velocity.Y обнуляется в момент приземления, то вам надо будет запоминать его значение с прыдыдущего кадра
//            {
//                m_IsJump = false; //Даём возможность прожать прыжок
//                m_elapsedCoyoteJumpTime = 0; //Обнуляем прошедшее время для койот джампа
//            }
//        }
//    }
//}


