using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField] private CharacterShield _characterShield;

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
    public event UnityAction ChangeDieCounter;
    public event UnityAction OnDiedOfShock;
    public event UnityAction OnDiedFromFall;

    public event UnityAction OnRevive;

    private Coroutine _diedCoroutine;
    private bool _isDied;
    private bool _isFastKill;

    private float _maxNotGroundTime = 3f;
    private float _elapsedNotGroundTime;
    public bool IsDied { get => _isDied; private set => _isDied = value; }

    public int DieCounter { get; private set; }

    private void Start()
    {
        DieCounter = PlayerPrefs.GetInt(PlayerPrefsParametrs.DieCounter);
        ChangeDieCounter?.Invoke();
        _defaultScale = _cubeSize;
        _minScale = new Vector3(_cubeSize.x, _cubeSize.y / 7, _cubeSize.z) ;
    }

    private void Update()
    {
        CheckFreez();

        ChangeSizeCollisionCubeForJump();

        CheckKill();
    }

    private void FixedUpdate()
    {
        CheckKillCollision();
    }

    public void Restart()
    {
        OnRevive?.Invoke();
    }

    private void CheckKill()
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
    }

    private void DiedForOnlyKill()
    {
        if (IsDied)
            return;

        IsDied = true;

        DieCounter++;
        ChangeDieCounter?.Invoke();
        PlayerPrefs.SetInt(PlayerPrefsParametrs.DieCounter, DieCounter);
        
        _diedCoroutine = StartCoroutine(StartDiedEvent());
        OnDiedFromFall?.Invoke();
        _animator.SetBool("IsRun", false);
    }

    private void DiedForShock()
    {
        if (IsDied)
            return;

        if (_characterShield.IsShield)
            return;

        IsDied = true;

        DieCounter++;
        ChangeDieCounter?.Invoke();
        PlayerPrefs.SetInt(PlayerPrefsParametrs.DieCounter, DieCounter);

        _diedCoroutine = StartCoroutine(StartDiedEvent());
        OnDiedOfShock?.Invoke();
        _animator.SetTrigger("Dead");
        _animator.SetBool("IsRun", false);
    }

    private void CheckFreez()
    {
        if (IsDied)
            return;

        if (_physicalCC.isGround == false)
        {
            if (_physicalCC.LongJumpActive == true)
                return;
            
            _elapsedNotGroundTime += Time.deltaTime;

            if (_elapsedNotGroundTime > _maxNotGroundTime)
            {
                IsDied = true;
                _diedCoroutine = StartCoroutine(StartDiedEvent());
                OnDiedOfShock?.Invoke();
                _animator.SetTrigger("Dead");
                _animator.SetBool("IsRun", false);
                _elapsedNotGroundTime = 0;

                DieCounter++;
                ChangeDieCounter?.Invoke();
                PlayerPrefs.SetInt(PlayerPrefsParametrs.DieCounter, DieCounter);
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

    private void CheckKillCollision()
    {
        if (Physics.CheckBox(_collisionPoint.position, _cubeSize / 2, transform.rotation, _enemyLayer))
        {
            DiedForShock();
        }
        if (Physics.CheckBox(_collisionPoint.position, _cubeSize / 2, transform.rotation, _onlyKillLayer))
        {
            DiedForOnlyKill();
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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(_collisionPoint.position, _cubeSize);
    }
}
