using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CheckPoint : MonoBehaviour
{
    [Header("Collision Parametrs")]
    [SerializeField] private Transform _boxCollider;
    [SerializeField] private LayerMask _characterLayer;

    [Header("CheckPoint Parametrs")]
    [SerializeField] private Transform _restartPoint;
    [SerializeField] private GameObject _effectPrefab;
    [SerializeField] private GameObject _defaultObject;

    private GameObject _soholdingZone;
    private bool _activated;
    public int Index { get; private set; }
    public Vector3 RestartPosition => _restartPoint.position;

    public event UnityAction<CheckPoint> OnCollisionEnter;

    private const float _delayForDeleteEffects = 0.11f;

    private void FixedUpdate()
    {
        CheckCollision();
    }

    private void OnEnable()
    {
        _soholdingZone = transform.parent.gameObject;
    }

    private void Start()
    {
        _boxCollider.GetComponent<MeshRenderer>().enabled = false;
    }

    public void Initialize(int index)
    {
        Index = index;
    }

    public void SetActiveSoholdingZone(bool isActive)
    {
        _soholdingZone.SetActive(isActive);
        
    }

    private void CheckCollision()
    {
        if (Physics.CheckBox(_boxCollider.position, _boxCollider.lossyScale / 2, transform.rotation, _characterLayer))
        {
            if (_activated == false)
            {
                OnCollisionEnter?.Invoke(this);
                _activated = true;
                StartCoroutine(Activate());
            }
        }
    }

    private IEnumerator Activate()
    {
        yield return new WaitForSeconds(_delayForDeleteEffects);

        // Если префаб указан, создаём его на месте удалённого объекта
        if (_effectPrefab != null)
        {
            Instantiate(_effectPrefab, _defaultObject.transform.position, _defaultObject.transform.rotation, transform);
        }

        // Удаляем объект
        Destroy(_defaultObject);
    }
}
