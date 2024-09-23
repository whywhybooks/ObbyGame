using UnityEngine;

public class AccelerationZone : MonoBehaviour
{
    [SerializeField] private float _multiplier;
   // [SerializeField] private float _duration;

    [Header("Collision Parametrs")]
    [SerializeField] private Transform _boxCollider;
    [SerializeField] private LayerMask _characterLayer;

    private Collider[] _hits;
    private ChraracterSpeedBooster _characterSpeedBooster;
    private bool _isActive;

    private void Start()
    {
       // _boxCollider.GetComponent<MeshRenderer>().enabled = false;
    }

    private void FixedUpdate()
    {
        CheckCollision();
    }

    private void CheckCollision()
    {
        _hits = Physics.OverlapBox(_boxCollider.position, _boxCollider.lossyScale / 2, transform.rotation, _characterLayer);

        if (_hits.Length > 0 && _isActive == false)
        {
            if (_characterSpeedBooster == null) 
            {
                _characterSpeedBooster = _hits[0].GetComponent<ChraracterSpeedBooster>();
                _isActive = true;
            }

            // _hits[0].GetComponent<ChraracterSpeedBooster>().BoostSpeed(_multiplier, _duration);
            _characterSpeedBooster.PermanentBoostSpeed(_multiplier);
        }
        else if (_isActive && _hits.Length == 0)
        {
            _isActive = false;
            _characterSpeedBooster.DeletePermanentBoostSpeed(_multiplier);
            _characterSpeedBooster = null;
        }
    }
}
