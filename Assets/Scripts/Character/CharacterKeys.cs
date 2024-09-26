using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CharacterKeys : MonoBehaviour
{
    private int _currentCount;

    public int CurrentCount { get => _currentCount; private set => _currentCount = value; }

    public event UnityAction OnOpen;
    public event UnityAction OnGetKey;

    private void Start()
    {
       // OnGetKey?.Invoke();
    }

    public bool TryOpenDoor(int targetCount)
    {
        if (_currentCount >= targetCount)
        {
            _currentCount -= targetCount;
            OnOpen?.Invoke(); 

            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddKey()
    {
        _currentCount++;
        OnGetKey?.Invoke();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.TryGetComponent(out Key key))
        {
            AddKey();
            key.gameObject.SetActive(false);
        }
    }
}