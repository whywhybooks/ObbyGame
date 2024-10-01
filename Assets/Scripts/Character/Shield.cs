using UnityEngine;

public class Shield : MonoBehaviour
{
    [field: SerializeField] public float Duration { get; private set; }
    [SerializeField] private GameObject _renderer;
    [SerializeField] private Collider _collider;

    private CharacterHealth _characterHealth;

    private void OnEnable()
    {
        if (_characterHealth == null )
            _characterHealth = FindObjectOfType<CharacterHealth>();

        _characterHealth.OnDied += Enable;
    }

    private void OnDisable()
    {
        _characterHealth.OnDied -= Enable;
    }

    public void Disable()
    {
        _renderer.SetActive( false );   
        _collider.enabled = false;
    }

    public void Enable()
    {
        _renderer.SetActive(true);
        _collider.enabled = true;
    }
}
