using UnityEngine;

public class CollapsedObject : MonoBehaviour
{
    [SerializeField] private float _delayBeforeDestruction;

    private bool _readyForDestruction;
    private float _elapsedTime;

    private void Update()
    {
        if (_readyForDestruction)
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime > _delayBeforeDestruction)
            {
                gameObject.SetActive(false);
                _elapsedTime = 0;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out CharacterMovementController character))
        {
            _readyForDestruction = true;
        }
    }
}
