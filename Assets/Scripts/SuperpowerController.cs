using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SuperpowerController : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private CharacterTypeChanger _characterTypeChanger;

    [Header("Long-jump")]
    [SerializeField] private float _delayTime;
    [SerializeField] private float _longJumpHeight;

    [Header("Speed-boost")]
    [SerializeField] private float _accelerationTime;
    [SerializeField] private float _multiplier;

    private int _superItemCount;

    public int SuperItemCount { get => _superItemCount; private set => _superItemCount = value; }

    public event UnityAction ChangeSuperItemCount;
    public event UnityAction PickupSuperItemCount;
    public event UnityAction OverSuperItemCount;
    public event UnityAction FillSuperItemCount;

    private void Start()
    {
        //��������� �� ������ ���������� ����� �������
        ChangeSuperItemCount?.Invoke();
        CheckCuperItemCount();
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

    public bool PlayPower()
    {
        if (_superItemCount <= 0)
            return false;

        if (_characterTypeChanger.CharacterType == CharacterType.Girl) // ���� ��� �������
        {
            _playerInput.LongJump(_longJumpHeight, _delayTime);
            _superItemCount--;
            ChangeSuperItemCount?.Invoke();

            CheckCuperItemCount();
            return true;
         }

        if (_characterTypeChanger.CharacterType == CharacterType.Man) // ���� ��� �������
        {
            _playerInput.BoostSpeed(_multiplier);
            StartCoroutine(SpeedBoost());
            _superItemCount--;
            ChangeSuperItemCount?.Invoke();

            CheckCuperItemCount();
            return true;
        }

        return true;
    }


    private void CheckCuperItemCount()
    {
        if (_superItemCount == 0)
        {
            OverSuperItemCount?.Invoke();
        }
        else
        {
            FillSuperItemCount?.Invoke();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.TryGetComponent(out SuperItem superItem))
        {
            _superItemCount++;
            ChangeSuperItemCount?.Invoke();
            PickupSuperItemCount?.Invoke();
            superItem.gameObject.SetActive(false);
            CheckCuperItemCount();
        }
    }

    private IEnumerator SpeedBoost()
    {
        yield return new WaitForSeconds(_accelerationTime);
        _playerInput.RemoveAcceleration(_multiplier);
    }
}