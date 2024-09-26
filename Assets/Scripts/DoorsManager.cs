using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorsManager : MonoBehaviour
{
    [SerializeField] private List<Door> _doors = new List<Door>();
    [SerializeField] private CharacterKeys _characterKeys;

    private int _currentDoorIndex;

    public int TargetKeysCount { get; private set; }

    public event UnityAction OnSetNewDoor;
    public event UnityAction OnOverDoors;

    private void OnEnable()
    {
        _characterKeys.OnOpen += SetNewDoor;
    }

    private void Start()
    {
        _currentDoorIndex = 0;
        TargetKeysCount = _doors[_currentDoorIndex].TargetKeys;
        OnSetNewDoor?.Invoke();

        foreach (Door door in _doors)
        {
            door.Initialize(_characterKeys);
        }
    }

    private void SetNewDoor()
    {
        _currentDoorIndex++;

        if (_currentDoorIndex < _doors.Count)
        {
            TargetKeysCount = _doors[_currentDoorIndex].TargetKeys;
            OnSetNewDoor?.Invoke();
        }
        else
        {
            OnOverDoors?.Invoke();
        }
    }
}

