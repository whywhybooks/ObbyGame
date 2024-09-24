using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CheckPointController : MonoBehaviour
{
    [SerializeField] private CharacterHealth _character;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _previousButton;
    [SerializeField] private List<CheckPoint> _checkPoints = new List<CheckPoint>();

    private CheckPoint _currentCheckPoint;
    private int _currentCheckPointIndex;

    public List<CheckPoint> CheckPoints { get => _checkPoints; private set => _checkPoints = value; }

    public event UnityAction OnRestart;

    private void Awake()
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

        _character.OnDied += Restart;

        for (int i = 15; i < _checkPoints.Count; i++)
        {
            _checkPoints[i].SetActiveSoholdingZone(false);
        }
    }

    private void Start()
    {
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

        _character.OnDied -= Restart;
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

        CharacterSetPosition(_currentCheckPoint.RestartPosition);
    }

    private void SetCheckPoint(CheckPoint checkPoint)
    {
        if (_currentCheckPoint != null)
        {
            _currentCheckPoint = null;
        }

        _currentCheckPoint = checkPoint;

        if (checkPoint.Index > 15 && checkPoint.Index < _checkPoints.Count)
        {
            for (int i = checkPoint.Index - 15; i >= 0; i--)
            {
                _checkPoints[i].SetActiveSoholdingZone(false);
            }
        }

        for (int i = checkPoint.Index; i < checkPoint.Index + 15; i++)
        {
            _checkPoints[i].SetActiveSoholdingZone(true);
        }
    }

    private void Restart()
    {
        CharacterSetPosition(_currentCheckPoint.RestartPosition);
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

        CharacterSetPosition(_currentCheckPoint.RestartPosition);
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

        CharacterSetPosition(_currentCheckPoint.RestartPosition);
    }

    private void CharacterSetPosition(Vector3 targetPosition)
    {
        _characterController.enabled = false;
        _character.transform.position = targetPosition;
        _characterController.enabled = true;
    }
}
