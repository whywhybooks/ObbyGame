using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static MaterialChanger;

public class DestroyableObject : MonoBehaviour
{
    [Header("Effect")]
    [SerializeField] private float _speedMove;
    [SerializeField] private float _amplitude;
    [SerializeField] private Material _destroyMaterial;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Transform _rootGameObject;

    private Coroutine _moveObjectUpDown;
    private Vector3 _startPosition;

    [Header("Destroy Parametrs")]
    [SerializeField] private float _delayBeforeDestruction;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Collider _colliderComponent;

    private bool _readyForDestruction;
    private bool _isDeleted;

    [Header("Recovery Parametrs")]
    [SerializeField] private bool _isRecovery;
    [SerializeField] private float _delayBeforeRecovery;

    private float _elapsedTime;

    [Header("Collision Parametrs")]
    [SerializeField] private Transform _boxCollider;
    [SerializeField] private LayerMask _characterLayer;

    private void Start()
    {
        _boxCollider.GetComponent<MeshRenderer>().enabled = false;
        _startPosition = _rootGameObject.transform.position;
    }

    private void Update()
    {
        if (_readyForDestruction && _isDeleted == false)
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime > _delayBeforeDestruction)
            {
                Destruct();
            }
        }

        if (_isDeleted && _isRecovery)
        {
            _elapsedTime += Time.deltaTime;

            if ( _elapsedTime > _delayBeforeRecovery)
            {
                Recovery();
            }
        }
    }

    private void Destruct()
    {
        StopCoroutine(_moveObjectUpDown);
        SetActive(false);
        _elapsedTime = 0;
        _isDeleted = true;
        _readyForDestruction = false;

        Instantiate(_particleSystem, transform.position, Quaternion.identity);
    }

    private void Recovery()
    {
        _rootGameObject.position = _startPosition;
        _renderer.material = _defaultMaterial;
        SetActive(true);
        _isDeleted = false;
        _elapsedTime = 0;
    }

    private void FixedUpdate()
    {
        if (_readyForDestruction == true)
            return;

        if (_isDeleted == true)
            return;

        CheckCollision();
    }

    private void SetActive(bool isActive)
    {
        _meshRenderer.enabled = isActive;
        _colliderComponent.enabled = isActive;
    }

    private void CheckCollision()
    {
        if (Physics.CheckBox(_boxCollider.position, _boxCollider.lossyScale / 2, transform.rotation, _characterLayer))
        {
            _readyForDestruction = true;
            _renderer.material = _destroyMaterial;
            _moveObjectUpDown = StartCoroutine(MoveObjectUpDown());
        }
    }

    private IEnumerator MoveObjectUpDown()
    {
        Vector3 startPosition = _rootGameObject.position;
        float elapsedTime = 0;

        // Уникальное смещение по времени для каждого объекта
        float timeOffset = Random.Range(0f, 2f); // Случайное смещение от 0 до 2 секунд

        // Бесконечный цикл для движения объекта вверх-вниз
        while (elapsedTime < _delayBeforeDestruction)
        {
            elapsedTime += Time.deltaTime;
            // Рассчитываем новую позицию объекта на основе времени, амплитуды и скорости
            float newY = startPosition.y + Mathf.Sin((Time.time + timeOffset) * _speedMove) * _amplitude;

            // Применяем новую позицию
            _rootGameObject.position = new Vector3(_rootGameObject.position.x, newY, _rootGameObject.position.z);

            // Ждём до следующего кадра
            yield return null;
        }
    }
}
