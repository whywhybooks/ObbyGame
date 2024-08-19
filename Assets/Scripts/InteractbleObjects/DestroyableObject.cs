using UnityEditor;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
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
    }

    private void Update()
    {
        if (_readyForDestruction)
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime > _delayBeforeDestruction)
            {
                SetActive(false);
                _elapsedTime = 0;
                _isDeleted = true;
                _readyForDestruction = false;
            }
        }

        if (_isDeleted && _isRecovery)
        {
            _elapsedTime += Time.deltaTime;

            if ( _elapsedTime > _delayBeforeRecovery)
            {
                SetActive(true);
                _isDeleted = false;
                _elapsedTime = 0;
            }
        }
    }

    private void FixedUpdate()
    {
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
        }
    }
}
