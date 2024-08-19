using UnityEngine;

public class AccelerationZone : MonoBehaviour
{
    [SerializeField] private float _multiplier;
    [SerializeField] private float _duration;

    [Header("Collision Parametrs")]
    [SerializeField] private Transform _boxCollider;
    [SerializeField] private LayerMask _characterLayer;

    private Collider[] _hits;

    private void Start()
    {
        _boxCollider.GetComponent<MeshRenderer>().enabled = false;
    }

    private void FixedUpdate()
    {
        CheckCollision();
    }

    private void CheckCollision()
    {
        _hits = Physics.OverlapBox(_boxCollider.position, _boxCollider.lossyScale / 2, transform.rotation, _characterLayer);

        if (_hits.Length > 0)
        {
            _hits[0].GetComponent<ChraracterSpeedBooster>().BoosSpeed(_multiplier, _duration);
        }
    }
}
