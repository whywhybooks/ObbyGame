using UnityEngine;

public class ContinuousRotation : MonoBehaviour
{
    // Общая скорость вращения (градусы в секунду)
    public float rotationSpeed = 100f;

    // Булевые переменные для выбора осей вращения
    public bool rotateAroundX = false;
    public bool rotateAroundY = true;
    public bool rotateAroundZ = false;

    void Update()
    {
        // Инициализируем вектор оси вращения
        Vector3 rotationAxis = Vector3.zero;

        // Проверяем, какие оси выбраны, и составляем вектор оси вращения
        if (rotateAroundX)
            rotationAxis += Vector3.right;
        if (rotateAroundY)
            rotationAxis += Vector3.up;
        if (rotateAroundZ)
            rotationAxis += Vector3.forward;

        // Вращаем объект, если хотя бы одна ось выбрана
        if (rotationAxis != Vector3.zero)
            transform.Rotate(rotationAxis.normalized * rotationSpeed * Time.deltaTime);
    }
}
