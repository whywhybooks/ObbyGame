using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SuperpowerController : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;

    [Header("Long-jump")]
    [SerializeField] private float _delayTime;
    [SerializeField] private float _longJumpHeight;

    [Header("Speed-boost")]
    [SerializeField] private float _accelerationTime;
    [SerializeField] private float _multiplier;

    private int _superItemCount;

    public int SuperItemCount { get => _superItemCount; private set => _superItemCount = value; }

    public event UnityAction ChangeSuperItemCount;
    public event UnityAction OverSuperItemCount;

    private void Start()
    {
        //Подгрузка из памяти количества супер айтемов
        ChangeSuperItemCount?.Invoke();
    }

    private void Update()
    {
        if (_superItemCount <= 0)
            return;

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            _playerInput.LongJump(_longJumpHeight, _delayTime);
            _superItemCount--;
            ChangeSuperItemCount?.Invoke();

            CheckCuperItemCount();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _playerInput.BoostSpeed(_multiplier);
            StartCoroutine(SpeedBoost());
            _superItemCount--;
            ChangeSuperItemCount?.Invoke();

            CheckCuperItemCount();
        }
    }


    private void CheckCuperItemCount()
    {
        if (_superItemCount == 0)
        {
            OverSuperItemCount?.Invoke();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.TryGetComponent(out SuperItem superItem))
        {
            _superItemCount++;
            ChangeSuperItemCount?.Invoke();
            superItem.gameObject.SetActive(false);
        }
    }

    private IEnumerator SpeedBoost()
    {
        yield return new WaitForSeconds(_accelerationTime);
        _playerInput.RemoveAcceleration(_multiplier);
    }
}