using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public int range = 10;//max height of Box's movement
    public float xCenter = 6f;

    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
      //  rb.MovePosition(new Vector3(xCenter + Mathf.PingPong(Time.time * 2, range) - range / 2f, transform.position.y, transform.position.z));
        Quaternion targetRotation = Quaternion.Euler(0, range * Time.fixedDeltaTime, 0);
        rb.MoveRotation(rb.rotation * targetRotation);
    }
}
