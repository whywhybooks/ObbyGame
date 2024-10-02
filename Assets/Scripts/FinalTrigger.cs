using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class FinalTrigger : MonoBehaviour
{
    [Header("Collision parametrs:")]
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Transform _triggerCollider;

    private bool _isActive;

    public event UnityAction IsActive;

    private void Start()
    {
        _triggerCollider.GetComponent<MeshRenderer>().enabled = false;  
    }

    private void Update()
    {
        CheckCollision();
    }

    private void CheckCollision()
    {
        if (_isActive)
            return;

        if (Physics.CheckBox(_triggerCollider.position, _triggerCollider.localScale / 2, transform.rotation, _playerLayer))
        {
            _isActive = true;
            IsActive?.Invoke();
            StartCoroutine(RestartScene());
        }
    }

    private IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(7.10f); //Длительность комикса

        PlayerPrefs.SetInt(PlayerPrefsParametrs.CurrentNumberCheckpoint, 0);
        PlayerPrefs.SetInt(PlayerPrefsParametrs.StarsCounter, 0);
        PlayerPrefs.SetInt(PlayerPrefsParametrs.DieCounter, 0);

        SceneManager.LoadScene(0);
    }
}