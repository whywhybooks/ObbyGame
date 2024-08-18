using UnityEngine;

public class AccelerationZone : MonoBehaviour
{
    [SerializeField] private float _multiplier;
    [SerializeField] private float _duration;

    [Header("Collision Parametrs")]
    [SerializeField] private Transform _collisionPoint;
    [SerializeField] private Vector3 _cubeSize;
    [SerializeField] private LayerMask _characterLayer;

    private Collider[] _hits;

    private void FixedUpdate()
    {
        CheckCollision();
    }

    private void CheckCollision()
    {
        _hits = Physics.OverlapBox(_collisionPoint.position, _cubeSize / 2, transform.rotation, _characterLayer);

        if (_hits.Length > 0)
        {
            _hits[0].GetComponent<ChraracterSpeedBooster>().BoosSpeed(_multiplier, _duration);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(_collisionPoint.position, _cubeSize);
    }
}
