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

    public event UnityAction OnDied;
    public event UnityAction OnShieldPickUp;
    public event UnityAction OnShieldOver;

    private Coroutine _stopShieldCoroutine;
    private bool _isShield;
    private float _shieldTime;
    private float _shieldElapsedTime;

    public float ShieldTime { get => _shieldTime; private set => _shieldTime = value; }
    public float ShieldElapsedTime { get => _shieldElapsedTime; private set => _shieldElapsedTime = value; }
    public float ShieldLeftTime => ShieldTime - ShieldElapsedTime;

    private void FixedUpdate()
    {
        CheckCollision();
    }

    private void CheckCollision()
    {
        if (Physics.CheckBox(_collisionPoint.position, _cubeSize / 2, transform.rotation, _enemyLayer))
        {
            if (_isShield)
                return;

            OnDied?.Invoke();
        }
        if (Physics.CheckBox(_collisionPoint.position, _cubeSize / 2, transform.rotation, _onlyKillLayer))
        {
            OnDied?.Invoke();
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