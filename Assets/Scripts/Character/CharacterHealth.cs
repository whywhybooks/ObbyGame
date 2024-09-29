using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CharacterHealth : MonoBehaviour
{
    [Header("Collision Parametrs")]
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] private Material _defaultMaterila;
    [SerializeField] private Material _shiledMaterial;
    [SerializeField] private ParticleSystem _particleSystem;

    private bool _isShield;
    private float _shieldTime;
    private float _shieldElapsedTime;
    private Coroutine _stopShieldCoroutine;

    public event UnityAction OnShieldOver;

    public float ShieldTime { get => _shieldTime; private set => _shieldTime = value; }
    public float ShieldElapsedTime { get => _shieldElapsedTime; private set => _shieldElapsedTime = value; }
    public float ShieldLeftTime => ShieldTime - ShieldElapsedTime;

    [Header("Collision Parametrs")]
    [SerializeField] private Transform _collisionPoint;
    [SerializeField] private Vector3 _cubeSize;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private LayerMask _onlyKillLayer;

    [Header("Other Parametrs")]
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

    public event UnityAction OnRevive;

    private Coroutine _diedCoroutine;
    private bool _isDied;
    private bool _isFastKill;

    private float _maxNotGroundTime = 2f;
    private float _elapsedNotGroundTime;
    public bool IsDied { get => _isDied; private set => _isDied = value; }

    private void Start()
    {
        _defaultScale = _cubeSize;
        _minScale = new Vector3(_cubeSize.x, _cubeSize.y / 7, _cubeSize.z) ;
    }

    private void Update()
    {
        CheckFreez();

        ChangeSizeCollisionCubeForJump();

        CheckCollision();
    }

    public void Restart()
    {
        OnRevive?.Invoke();
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
            DiedForShock();
        }
        if (Physics.CheckBox(_collisionPoint.position, _cubeSize / 2, transform.rotation, _onlyKillLayer))
        {
            DiedForOnlyKill();
        }
    }

    private void DiedForOnlyKill()
    {
        IsDied = true;
        _diedCoroutine = StartCoroutine(StartDiedEvent());
        OnDiedFromFall?.Invoke();
        // _animator.SetTrigger("Dead");
        _animator.SetBool("IsRun", false);
    }

    private void DiedForShock()
    {
        if (_isShield)
            return;

        IsDied = true;
        _diedCoroutine = StartCoroutine(StartDiedEvent());
        OnDiedOfShock?.Invoke();
        _animator.SetTrigger("Dead");
        _animator.SetBool("IsRun", false);
    }

    private void CheckFreez()
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
    }

    private void ChangeSizeCollisionCubeForJump()
    {
        if (_physicalCC.isGround == true)
        {
            _cubeSize = _defaultScale;
        }
        else
        {
            _cubeSize = _minScale;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(_collisionPoint.position, _cubeSize);
    }

    private void ShiledActivate(Shield shield)
    {
        OnShieldPickUp?.Invoke();
        _isShield = true;
        _shieldTime += shield.Duration;
        shield.Disable();
        _skinnedMeshRenderer.material = _shiledMaterial;
        _particleSystem.gameObject.SetActive(true);

        if (_stopShieldCoroutine == null)
            _stopShieldCoroutine = StartCoroutine(StopShieldTimer());
    }

    private void ShiledDeactivate()
    {
        if (_isShield == false)
            return;

        StopCoroutine(StopShieldTimer());
        OnShieldOver?.Invoke();
        _shieldTime = 0;
        _shieldElapsedTime = 0;
        _isShield = false;
        _stopShieldCoroutine = null;
        _skinnedMeshRenderer.material = _defaultMaterila;
        _particleSystem.gameObject.SetActive(false);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.TryGetComponent(out Shield shield))
        {
            ShiledActivate(shield);
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

        OnDied?.Invoke();
        IsDied = false;
        _isFastKill = false;
        _animator.Play("Idle");
        ShiledDeactivate();
    }

    private IEnumerator StopShieldTimer()
    {
        while (_shieldElapsedTime < _shieldTime)
        {
            _shieldElapsedTime += Time.deltaTime;
            yield return null;
        }

        ShiledDeactivate();
    }
}