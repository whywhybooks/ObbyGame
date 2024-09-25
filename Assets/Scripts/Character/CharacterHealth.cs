using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CharacterHealth : MonoBehaviour
{
    [Header("Collision Parametrs")]
    [SerializeField] private Transform _collisionPoint;
    [SerializeField] private Vector3 _cubeSize;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private LayerMask _onlyKillLayer;
    [SerializeField] private PhysicalCC _physicalCC;
    [SerializeField] private float _restartDelay;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _playerModel;

    private Vector3 _defaultScale;
    private Vector3 _minScale;

    public event UnityAction OnDied;
    public event UnityAction OnDiedOfShock;
    public event UnityAction OnDiedFromFall;
    public event UnityAction OnShieldPickUp;
    public event UnityAction OnShieldOver;

    private Coroutine _stopShieldCoroutine;
    private Coroutine _diedCoroutine;
    private bool _isShield;
    private bool _isDied;
    private bool _isFastKill;
    private float _shieldTime;
    private float _shieldElapsedTime;

    private float _maxNotGroundTime = 3;
    private float _elapsedNotGroundTime;

    public float ShieldTime { get => _shieldTime; private set => _shieldTime = value; }
    public float ShieldElapsedTime { get => _shieldElapsedTime; private set => _shieldElapsedTime = value; }
    public float ShieldLeftTime => ShieldTime - ShieldElapsedTime;
    public bool IsDied { get => _isDied; private set => _isDied = value; }

    private void Start()
    {
        _defaultScale = _cubeSize;
        _minScale = new Vector3(_cubeSize.x, _cubeSize.y / 7, _cubeSize.z) ;
    }

    private void FixedUpdate()
    {
    }

    private void Update()
    {
        if (_physicalCC.isGround == false)
        {
            _elapsedNotGroundTime += Time.deltaTime;

            if (_elapsedNotGroundTime > _maxNotGroundTime)
            {
                IsDied = true;
                _diedCoroutine = StartCoroutine(StartDiedEvent());
                OnDiedOfShock?.Invoke();
                _animator.SetTrigger("Dead");
                _animator.SetBool("IsRun", false);
                _elapsedNotGroundTime = 0;
            }
        }
        else
        {
            _elapsedNotGroundTime = 0;
        }

        if (_physicalCC.isGround == true)
        {
            _cubeSize = _defaultScale;
        }
        else
        {
            _cubeSize = _minScale;
        }

        CheckCollision();
    }

    private void CheckCollision()
    {
        if (IsDied == true)
        {
           if (_physicalCC.isGround && _isFastKill == false)
            {
                _isFastKill = true;

                if (_diedCoroutine != null)
                {
                    StopCoroutine(_diedCoroutine);
                    _diedCoroutine = null;
                }

                _animator.SetTrigger("Dead");
                _diedCoroutine = StartCoroutine(StartDiedEvent());
            }

            return;
        }

        if (Physics.CheckBox(_collisionPoint.position, _cubeSize / 2, transform.rotation, _enemyLayer))
        {
            if (_isShield)
                return;

            IsDied = true;
            _diedCoroutine = StartCoroutine(StartDiedEvent());
            OnDiedOfShock?.Invoke();
            _animator.SetTrigger("Dead");
            _animator.SetBool("IsRun", false);
        }
        if (Physics.CheckBox(_collisionPoint.position, _cubeSize / 2, transform.rotation, _onlyKillLayer))
        {
            IsDied = true;
            _diedCoroutine = StartCoroutine(StartDiedEvent());
            OnDiedFromFall?.Invoke();
            // _animator.SetTrigger("Dead");
            _animator.SetBool("IsRun", false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(_collisionPoint.position, _cubeSize);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.TryGetComponent(out Shield shield))
        {
            OnShieldPickUp?.Invoke();
            _isShield = true;
            _shieldTime += shield.Duration;
            shield.gameObject.SetActive(false);

            if (_stopShieldCoroutine == null)
                _stopShieldCoroutine = StartCoroutine(StopShield());
        }
    }

    private IEnumerator StartDiedEvent()
    {
        float elapsedTime = 0;
        while (elapsedTime < _restartDelay)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
     //   yield return new WaitForSeconds(_restartDelay);
        OnDied?.Invoke();
        IsDied = false;
        _isFastKill = false;
        _animator.Play("Idle");
      //  _playerModel.transform.eulerAngles = Vector3.zero;
    }

    private IEnumerator StopShield()
    {
        while (_shieldElapsedTime < _shieldTime)
        {
            _shieldElapsedTime += Time.deltaTime; 
            yield return null;
        }

        OnShieldOver?.Invoke();
        _shieldTime = 0;
        _shieldElapsedTime = 0;
        _isShield = false;
        _stopShieldCoroutine = null;
    }
}