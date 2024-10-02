using Analytics;
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
    public event UnityAction<bool, int> TriggerEnterForKey;

    private void OnEnable()
    {
        _characterKeys.OnOpen += SetNewDoor;

        foreach (var door in _doors)
        {
            door.OnTriggerEnterForKey += OnPlayerTriggerEnter;
        }
    }

    private void Start()
    {
        _currentDoorIndex = PlayerPrefs.GetInt(PlayerPrefsParametrs.CurrentDoorIndex);
        OnSetNewDoor?.Invoke();

        foreach (Door door in _doors)
        {
            door.Initialize(_characterKeys);
        }
    }

    private void OnDisable()
    {
        foreach (var door in _doors)
        {
            door.OnTriggerEnterForKey -= OnPlayerTriggerEnter;
        }
    }

    private void SetNewDoor()
    {
        GameAnalytics.gameAnalytics.LogEvent($"finish_quest{_currentDoorIndex+1}");

        _currentDoorIndex++;

        PlayerPrefs.SetInt(PlayerPrefsParametrs.CurrentDoorIndex, _currentDoorIndex);

        if (_currentDoorIndex < _doors.Count)
        {
           // TargetKeysCount = _doors[_currentDoorIndex].TargetKeys;
            OnSetNewDoor?.Invoke();
        }
        else
        {
            OnOverDoors?.Invoke();
        }
    }

    private void OnPlayerTriggerEnter(bool isTrigger, int keyCount)
    {
        TriggerEnterForKey?.Invoke(isTrigger, keyCount);
        TargetKeysCount = keyCount;
    }
}

