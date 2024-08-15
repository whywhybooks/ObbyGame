using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CharacterHealth : MonoBehaviour
{
    [Header("Collision Parametrs")]
    [SerializeField] private Transform _collisionPoint;
    [SerializeField] private Vector3 _cubeSize;
    [SerializeField] private LayerMask _enemyLayer;

    public event UnityAction OnDied;

    private void FixedUpdate()
    {
        CheckCollision();
    }

    private void CheckCollision()
    {
        if (Physics.CheckBox(_collisionPoint.position, _cubeSize / 2, transform.rotation, _enemyLayer))
        {
            OnDied?.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(_collisionPoint.position, _cubeSize);
    }
}
