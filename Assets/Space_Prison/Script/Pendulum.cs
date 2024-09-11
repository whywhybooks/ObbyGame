using UnityEngine;

public class Pendulum : MonoBehaviour
{
    // Скорость маятника (градусы в секунду)
    public float speed = 100f;

    // Лимиты отклонения (в градусах) в каждую сторону
    public float maxAngleX = 45f;
    public float maxAngleY = 0f;
    public float maxAngleZ = 0f;

    // Временной счетчик для управления движением маятника
    private float timeCounter = 0f;

    // Начальная ориентация объекта
    private Quaternion startRotation;

    void Start()
    {
        // Запоминаем начальную ориентацию объекта
        startRotation = transform.localRotation;
    }

    void Update()
    {
        // Увеличиваем счетчик времени с учетом скорости
        timeCounter += Time.deltaTime * speed;

        // Вычисляем углы поворота по каждой из осей с использованием синусоиды
        float angleX = maxAngleX * Mathf.Sin(timeCounter);
        float angleY = maxAngleY * Mathf.Sin(timeCounter);
        float angleZ = maxAngleZ * Mathf.Sin(timeCounter);

        // Применяем вращение относительно начальной ориентации объекта
        Quaternion rotationX = Quaternion.AngleAxis(angleX, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(angleY, Vector3.up);
        Quaternion rotationZ = Quaternion.AngleAxis(angleZ, Vector3.forward);

        // Компонуем вращение и применяем его к объекту
        transform.localRotation = startRotation * rotationX * rotationY * rotationZ;
    }
}
