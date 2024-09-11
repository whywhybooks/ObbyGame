using UnityEngine;

public class Levitation : MonoBehaviour
{
    // Амплитуда колебаний (максимальное отклонение вверх и вниз)
    public float amplitude = 0.5f;

    // Скорость колебаний
    public float frequency = 1.0f;

    // Угол максимального наклона
    public float tiltAngle = 15.0f;

    // Скорость наклона
    public float tiltSpeed = 1.0f;

    // Начальная позиция объекта
    private Vector3 startPosition;

    // Начальная ориентация объекта
    private Quaternion startRotation;

    void Start()
    {
        // Запоминаем начальную позицию и ориентацию объекта
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        // Вычисляем новое положение объекта (колебания вверх-вниз)
        float yOffset = amplitude * Mathf.Sin(Time.time * frequency);
        transform.position = new Vector3(startPosition.x, startPosition.y + yOffset, startPosition.z);

        // Вычисляем произвольные наклоны вокруг центра
        float xTilt = tiltAngle * Mathf.Sin(Time.time * tiltSpeed);
        float zTilt = tiltAngle * Mathf.Cos(Time.time * tiltSpeed);

        // Применяем наклон к начальной ориентации объекта
        Quaternion targetRotation = Quaternion.Euler(xTilt, 0, zTilt) * startRotation;
        transform.rotation = targetRotation;
    }
}
