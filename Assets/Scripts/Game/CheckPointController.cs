using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CheckPointController : MonoBehaviour
{
    [SerializeField] private CharacterHealth _character;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _previousButton;
    [SerializeField] private List<CheckPoint> _checkPoints = new List<CheckPoint>();

    private CheckPoint _currentCheckPoint;

    public List<CheckPoint> CheckPoints { get => _checkPoints; private set => _checkPoints = value; }

    private void OnEnable()
    {
       // _checkPoints = GetComponentsInChildren<CheckPoint>().ToList();

        _currentCheckPoint = _checkPoints[0];
        _nextButton.onClick.AddListener(NextCheckPoint);
        _previousButton.onClick.AddListener(PreviousCheckPoint);

        for (int i = 0; i < _checkPoints.Count; i++)
        {
            CheckPoint checkPoint = _checkPoints[i];
            checkPoint.Initialize(i);
            checkPoint.OnCollisionEnter += SetCheckPoint;
        }

        _character.OnDied += Restart;
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
    }

    private void Restart()
    {
        CharacterSetPosition(_currentCheckPoint.RestartPosition);
    }

    private void NextCheckPoint()
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
