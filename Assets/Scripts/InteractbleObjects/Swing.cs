using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    public Transform pivotPoint;  // Точка опоры
    public float torqueMultiplier = 10f;  // Множитель момента силы

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Если столкновение произошло с платформой
        if (hit.collider.CompareTag("Platform"))
        {
            // Вычисляем вектор от точки опоры до точки столкновения
            Vector3 leverArm = hit.point - pivotPoint.position;

            // Определяем направление силы, которая должна применяться к платформе
            Vector3 forceDirection = Vector3.Cross(leverArm, Vector3.up).normalized;

            // Применяем вращение к платформе
            hit.collider.transform.RotateAround(pivotPoint.position, forceDirection, torqueMultiplier * Time.deltaTime);
        }
    }
}
