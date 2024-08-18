using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    public Transform pivotPoint;  // ����� �����
    public float torqueMultiplier = 10f;  // ��������� ������� ����

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // ���� ������������ ��������� � ����������
        if (hit.collider.CompareTag("Platform"))
        {
            // ��������� ������ �� ����� ����� �� ����� ������������
            Vector3 leverArm = hit.point - pivotPoint.position;

            // ���������� ����������� ����, ������� ������ ����������� � ���������
            Vector3 forceDirection = Vector3.Cross(leverArm, Vector3.up).normalized;

            // ��������� �������� � ���������
            hit.collider.transform.RotateAround(pivotPoint.position, forceDirection, torqueMultiplier * Time.deltaTime);
        }
    }
}
