using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLooking : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;

    private void LateUpdate()
    {
        transform.eulerAngles = new Vector3(0, _mainCamera.transform.eulerAngles.y, 0);
    }
}
