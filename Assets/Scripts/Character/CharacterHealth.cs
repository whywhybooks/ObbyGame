using System.Collections;
using Unity.VisualScripting;
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

    private Vector3 _defaultScale;
    private Vector3 _minScale;

    public event UnityAction OnDied;
    public event UnityAction OnDiedOfShock;
    public event UnityAction OnDiedFromFall;
    public event UnityAction OnShieldPickUp;
    public event UnityAction OnShieldOver;

    private Coroutine _stopShieldCoroutine;
    private bool _isShield;
    private bool _isDied;
    private float _shieldTime;
    private float _shieldElapsedTime;

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
            return;

        if (Physics.CheckBox(_collisionPoint.position, _cubeSize / 2, transform.rotation, _enemyLayer))
        {
            if (_isShield)
                return;

            IsDied = true;
            StartCoroutine(StartDiedEvent());
            OnDiedOfShock?.Invoke();
            _animator.SetTrigger("Dead");
            _animator.SetBool("IsRun", false);
        }
        if (Physics.CheckBox(_collisionPoint.position, _cubeSize / 2, transform.rotation, _onlyKillLayer))
        {
            IsDied = true;
            StartCoroutine(StartDiedEvent());
            OnDiedFromFall?.Invoke();
            _animator.SetTrigger("Dead");
            _animator.SetBool("IsRun", false);
            Debug.Log(3456789);
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
        yield return new WaitForSeconds(_restartDelay);
        OnDied?.Invoke();
        IsDied = false;
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