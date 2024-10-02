using UnityEngine;
using UnityEngine.Events;

public class CharacterKeys : MonoBehaviour
{
    [Header("Collision Parametrs")]
    [SerializeField] private Transform _collisionPoint;
    [SerializeField] private Vector3 _cubeSize;
    [SerializeField] private LayerMask _keyMask;

    private Collider[] _hits;

    private int _currentCount;

    public int CurrentCount { get => _currentCount; private set => _currentCount = value; }

    public event UnityAction OnOpen;
    public event UnityAction OnGetKey;

    private void Start()
    {
       // OnGetKey?.Invoke();
    }

    private void FixedUpdate()
    {
        MyOnTriggerEnter();
    }

    public bool TryOpenDoor(int targetCount)
    {
        if (_currentCount >= targetCount)
        {
            _currentCount -= targetCount;
            OnOpen?.Invoke(); 

            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddKey()
    {
        _currentCount++;
        OnGetKey?.Invoke();
    }

    /*private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.TryGetComponent(out Key key))
        {
            AddKey();
            key.gameObject.SetActive(false);
        }
    }*/

    private void MyOnTriggerEnter()
    {
        _hits = Physics.OverlapBox(_collisionPoint.position, _cubeSize / 2, transform.rotation, _keyMask);

        if (_hits.Length > 0)
        {
            _hits[0].gameObject.SetActive(false);
            AddKey();
        }
    }
}