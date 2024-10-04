using Analytics;
using FMODUnity;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SuperpowerController : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private CharacterTypeChanger _characterTypeChanger;
    [SerializeField] private int _targetCountSuperItem;

    [Header("Long-jump")]
    [SerializeField] private float _delayTime;
    [SerializeField] private float _longJumpHeight;

    [Header("Speed-boost")]
    [SerializeField] private float _accelerationTime;
    [SerializeField] private float _multiplier;

    [Header("Collision Parametrs")]
    [SerializeField] private Transform _collisionPoint;
    [SerializeField] private Vector3 _cubeSize;
    [SerializeField] private LayerMask _superItemMask;

    private int _superItemCount;
    private bool _hasAbility;
    private Collider[] _hits;

    public int SuperItemCount { get => _superItemCount; private set => _superItemCount = value; }
    public int TargetCountSuperItem { get => _targetCountSuperItem; private set => _targetCountSuperItem = value; }

    public event UnityAction ChangeSuperItemCount;
    public event UnityAction PickupSuperItemCount;
    public event UnityAction OverSuperItemCount;
    public event UnityAction FillSuperItemCount;
    public event UnityAction AbilityActivate;

    private void Start()
    {
        //Подгрузка из памяти количества супер айтемов
        _superItemCount = PlayerPrefs.GetInt(PlayerPrefsParametrs.StarsCounter);
        ChangeSuperItemCount?.Invoke();
        CheckCuperItemCount();
    }

    private void FixedUpdate()
    {
        MyOnTriggerEnter();
    }

    private void Update()
    {
        if (_superItemCount < TargetCountSuperItem)
            return;

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            PlayPower();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            PlayPower();
        }
    }

    public bool PlayPower()
    {
        if (_superItemCount < TargetCountSuperItem)
            return false;

        /*   if (_characterTypeChanger.CharacterType == CharacterType.Girl) // Если это девочка
           {
               _playerInput.LongJump(_longJumpHeight, _delayTime);
               _superItemCount--;
               ChangeSuperItemCount?.Invoke();

               CheckCuperItemCount();
               return true;
            }*/

        /* if (_characterTypeChanger.CharacterType == CharacterType.Man) // Если это мальчик
         {
             _playerInput.BoostSpeed(_multiplier);
             StartCoroutine(SpeedBoost());
             _superItemCount--;
             ChangeSuperItemCount?.Invoke();

             CheckCuperItemCount();
             return true;
         }*/

        _playerInput.LongJump(_longJumpHeight, _delayTime);
        _superItemCount -= TargetCountSuperItem;
        PlayerPrefs.SetInt(PlayerPrefsParametrs.StarsCounter, _superItemCount);
        ChangeSuperItemCount?.Invoke();

        CheckCuperItemCount();
        return true;
    }


    private void CheckCuperItemCount()
    {
        if (_superItemCount < TargetCountSuperItem)
        {
            OverSuperItemCount?.Invoke();
            _hasAbility = false;
        }
        else
        {
            FillSuperItemCount?.Invoke();

            if (_hasAbility == false)
            {
                _hasAbility = true;
                AbilityActivate?.Invoke();
            }
        }
    }

    private void MyOnTriggerEnter()
    {
        _hits = Physics.OverlapBox(_collisionPoint.position, _cubeSize / 2, transform.rotation, _superItemMask);

        if (_hits.Length > 0)
        {
            _superItemCount++;
            PlayerPrefs.SetInt(PlayerPrefsParametrs.StarsCounter, _superItemCount);
            ChangeSuperItemCount?.Invoke();
            PickupSuperItemCount?.Invoke();
            _hits[0].gameObject.SetActive(false);
            CheckCuperItemCount();
            GameAnalytics.gameAnalytics.LogEvent("pick_up_star");
        }
    }

    private IEnumerator SpeedBoost()
    {
        yield return new WaitForSeconds(_accelerationTime);
        _playerInput.RemoveAcceleration(_multiplier);
    }
}