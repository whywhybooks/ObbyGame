using Analytics;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CharacterShield : MonoBehaviour
{
    [SerializeField] private CharacterHealth _characterHealth;

    [Header("Collision Parametrs")]
    [SerializeField] private Transform _collisionPoint;
    [SerializeField] private Vector3 _cubeSize;

    [Header("Shield Parametrs")]
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] private Material _defaultMaterila;
    [SerializeField] private Material _shiledMaterial;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private LayerMask _shieldMask;


    private bool _isShield;
    private float _shieldTime;
    private float _shieldElapsedTime;
    private Coroutine _stopShieldCoroutine;
    private Collider[] _hits;

    public event UnityAction OnShieldOver;

    public event UnityAction OnShieldPickUp;

    public float ShieldTime { get => _shieldTime; private set => _shieldTime = value; }
    public float ShieldElapsedTime { get => _shieldElapsedTime; private set => _shieldElapsedTime = value; }
    public float ShieldLeftTime => ShieldTime - ShieldElapsedTime;

    public bool IsShield { get => _isShield; private set => _isShield = value; }

    private void OnEnable()
    {
        _characterHealth.OnDied += ShiledDeactivate;
    }

    private void OnDisable()
    {
        _characterHealth.OnDied -= ShiledDeactivate;
    }

    private void FixedUpdate()
    {
        MyOnTriggerEnter();
    }

    private void ShiledActivate(Shield shield)
    {
        OnShieldPickUp?.Invoke();
        IsShield = true;
        _shieldTime += shield.Duration;
        shield.Disable();
        _skinnedMeshRenderer.material = _shiledMaterial;
        _particleSystem.gameObject.SetActive(true);

        if (_stopShieldCoroutine == null)
            _stopShieldCoroutine = StartCoroutine(StopShieldTimer());
    }

    private void ShiledDeactivate()
    {
        if (IsShield == false)
            return;

        StopCoroutine(StopShieldTimer());
        OnShieldOver?.Invoke();
        _shieldTime = 0;
        _shieldElapsedTime = 0;
        IsShield = false;
        _stopShieldCoroutine = null;
        _skinnedMeshRenderer.material = _defaultMaterila;
        _particleSystem.gameObject.SetActive(false);
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

    private void MyOnTriggerEnter()
    {
        if (_characterHealth.IsDied == true)
            return;

        _hits = Physics.OverlapBox(_collisionPoint.position, _cubeSize / 2, transform.rotation, _shieldMask);

        if (_hits.Length > 0)
        {
            ShiledActivate(_hits[0].GetComponent<Shield>());
            GameAnalytics.gameAnalytics.LogEvent("pick_up_guard");
        }
    }
}