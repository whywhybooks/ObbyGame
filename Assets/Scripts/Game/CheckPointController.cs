using Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CheckPointController : MonoBehaviour
{
    [SerializeField] private CharacterHealth _character;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private ThirdPersonCamera.CameraController _camera; //Она здесь, чтобы поворачивать в нужную сторону при респауне
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _previousButton;
    [SerializeField] private List<CheckPoint> _checkPoints = new List<CheckPoint>();

    private CheckPoint _currentCheckPoint;
    private int _currentCheckPointIndex;

    public List<CheckPoint> CheckPoints { get => _checkPoints; private set => _checkPoints = value; }
    public int CurrentCheckPointIndex { get => _currentCheckPointIndex; private set => _currentCheckPointIndex = value; }
    public int AllLevelsCount;

    public event UnityAction OnRestart;
    public event UnityAction OnActiveCheckpoint;
    public event UnityAction OnReachedUnlockLevel;


    private const int UnlockLevel = 29;
    private const int _maxActivateNextLevelsCount = 15;
    private const int _maxActivatePreviousLevelsCount = 3;

    /* private void Awake()
     {
         _currentCheckPoint = _checkPoints[0];
         _currentCheckPointIndex = 0;
         _nextButton.onClick.AddListener(NextCheckPoint);
         _previousButton.onClick.AddListener(PreviousCheckPoint);

         for (int i = 0; i < _checkPoints.Count; i++)
         {
             CheckPoint checkPoint = _checkPoints[i];
             checkPoint.Initialize(i);
             checkPoint.OnCollisionEnter += SetCheckPoint;
         }

        // _character.OnDied += Restart;

         for (int i = _maxActivateNextLevelsCount; i < _checkPoints.Count; i++)
         {
             _checkPoints[i].SetActiveSoholdingZone(false);
         }
     }

     private void Start()
     {
         Restart();
     }*/

    private void OnEnable()
    {
        AllLevelsCount = _checkPoints.Count;
    }

    public void StartGame()
    {
        StartCoroutine(StartGameC()); //корутиной делаем задержку в кадр, чтобы камера нормально повернулась. Иначе камера поворачивается в другую сторону
    }
    public IEnumerator StartGameC()
    {
        yield return null;  

        _currentCheckPoint = _checkPoints[PlayerPrefs.GetInt(PlayerPrefsParametrs.CurrentNumberCheckpoint)];
        _currentCheckPointIndex = PlayerPrefs.GetInt(PlayerPrefsParametrs.CurrentNumberCheckpoint);
        OnActiveCheckpoint?.Invoke();

        _nextButton.onClick.AddListener(NextCheckPoint);
        _previousButton.onClick.AddListener(PreviousCheckPoint);

        for (int i = 0; i < _checkPoints.Count; i++)
        {
            CheckPoint checkPoint = _checkPoints[i];
            checkPoint.Initialize(i);
            checkPoint.OnCollisionEnter += SetCheckPoint;
        }

        DisablePreviousLevels(_currentCheckPoint);
        EnableNextLevels(_currentCheckPoint);

        /*  for (int i = _maxActivateNextLevelsCount; i < _checkPoints.Count; i++)
          {
              _checkPoints[i].SetActiveSoholdingZone(false);
          }*/

        for (int i = Mathf.Min(_checkPoints.Count, _currentCheckPoint.Index + _maxActivateNextLevelsCount); i < _checkPoints.Count; i++)
        {
            _checkPoints[i].SetActiveSoholdingZone(false);
        }

        Restart();
    }

    private void OnDisable()
    {
        _nextButton.onClick.RemoveListener(NextCheckPoint);
        _previousButton.onClick.RemoveListener(PreviousCheckPoint);

        foreach (CheckPoint checkPoint in _checkPoints)
        {
            checkPoint.OnCollisionEnter -= SetCheckPoint;
        }

       // _character.OnDied -= Restart;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PreviousCheckPoint();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            NextCheckPoint();
        }
    }

    public void SetCheckpointOnIndex(int index)
    {
        if (index >= 0 &&  index < _checkPoints.Count)
        {
            SetCheckPoint(_checkPoints[index]);
        }

        CharacterSetPosition(_currentCheckPoint.RestartPosition, _currentCheckPoint.transform.eulerAngles);
    }

    private void SetCheckPoint(CheckPoint checkPoint)
    {
        if (checkPoint.Index == _currentCheckPointIndex)
            return;

        if (_currentCheckPoint != null)
        {
            _currentCheckPoint = null;
        }

        _currentCheckPoint = checkPoint;
        _currentCheckPointIndex = _currentCheckPoint.Index;
        GameAnalytics.gameAnalytics.LogEvent($"get_point_{_currentCheckPointIndex + 1}");

        if (_currentCheckPointIndex == UnlockLevel)
        {
            OnReachedUnlockLevel?.Invoke();
        }

        OnActiveCheckpoint?.Invoke();

        PlayerPrefs.SetInt(PlayerPrefsParametrs.CurrentNumberCheckpoint, _currentCheckPointIndex);

        DisablePreviousLevels(checkPoint);

        EnableNextLevels(checkPoint);
    }

    private void DisablePreviousLevels(CheckPoint checkPoint)
    {
        if (checkPoint.Index > _maxActivatePreviousLevelsCount && checkPoint.Index < _checkPoints.Count)
        {
            for (int i = checkPoint.Index - _maxActivatePreviousLevelsCount; i >= 0; i--)
            {
                _checkPoints[i].SetActiveSoholdingZone(false);
            }

            for (int i = checkPoint.Index; i >= checkPoint.Index - _maxActivatePreviousLevelsCount; i--)
            {
                _checkPoints[i].SetActiveSoholdingZone(true);
            }
        }
    }

    private void EnableNextLevels(CheckPoint checkPoint)
    {
        for (int i = checkPoint.Index; i < Mathf.Min(_checkPoints.Count, checkPoint.Index + _maxActivateNextLevelsCount); i++)
        {
            _checkPoints[i].SetActiveSoholdingZone(true);
        }
    }

    public void Restart()
    {
        CharacterSetPosition(_currentCheckPoint.RestartPosition, _currentCheckPoint.transform.eulerAngles);
        StartCoroutine(RestartDelay());
    }

    private IEnumerator RestartDelay()
    {
        yield return new WaitForSeconds(1);
        OnRestart?.Invoke();
    }

    public void NextCheckPoint()
    {
        if (_currentCheckPoint.Index + 1 >=  _checkPoints.Count)
        {
            SetCheckPoint(_checkPoints[0]);
        }
        else
        {
            SetCheckPoint(_checkPoints[_currentCheckPoint.Index + 1]);
        }

        CharacterSetPosition(_currentCheckPoint.RestartPosition, _currentCheckPoint.transform.eulerAngles);
    }

    private void PreviousCheckPoint()
    {
        if (_currentCheckPoint.Index - 1 < 0)
        {
            SetCheckPoint(_checkPoints[_checkPoints.Count - 1]);
        }
        else
        {
            SetCheckPoint(_checkPoints[_currentCheckPoint.Index - 1]);
        }

        CharacterSetPosition(_currentCheckPoint.RestartPosition, _currentCheckPoint.transform.eulerAngles);
    }

    private void CharacterSetPosition(Vector3 targetPosition, Vector3 targetRotation)
    {
        _characterController.enabled = false;
        _character.transform.position = targetPosition;
        targetRotation.y += 90;
        _character.transform.eulerAngles = new Vector3(0, targetRotation.y, 0);
        targetRotation.x = 30;
        _camera.SetRotation(targetRotation);
        _characterController.enabled = true;
    }
}
