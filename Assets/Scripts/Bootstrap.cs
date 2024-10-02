using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private CheckPointController CheckPointController;

    private void Awake()
    {
        CheckPointController.StartGame();
    }
}
