using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCMod : MonoBehaviour
{
    private bool _lockCursor = true;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            if (_lockCursor)
            {
                _lockCursor = false;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                _lockCursor= true;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
