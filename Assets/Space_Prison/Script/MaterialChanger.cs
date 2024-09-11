using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    [System.Serializable]
    public class TagSettings
    {
        public string tagName;          // Тег объекта
        public Material newMaterial;    // Новый материал
        public float amplitude = 1f;    // Амплитуда движения
        public float speed = 1f;        // Скорость движения
        public float destroyDelay = 2f; // Задержка удаления объекта
        public GameObject spawnPrefab;  // Префаб для спавна после удаления объекта
    }

    // Список настроек для разных тегов
    public List<TagSettings> tagSettingsList;

    // Метод, который вызывается, когда другой коллайдер входит в зону триггера
    private void OnTriggerEnter(Collider other)
    {
        // Получаем настройки для тега объекта
        TagSettings settings = GetSettingsForTag(other.tag);

        // Если найдены настройки для тега
        if (settings != null)
        {
            // Получаем компонент Renderer объекта
            Renderer objRenderer = other.GetComponent<Renderer>();

            // Если у объекта есть Renderer, меняем материал
            if (objRenderer != null && settings.newMaterial != null)
            {
                objRenderer.material = settings.newMaterial;
            }

            // Запускаем движение объекта
            StartCoroutine(MoveObjectUpDown(other.gameObject, settings));

            // Запускаем корутину для удаления объекта и создания префаба
            StartCoroutine(DestroyAndSpawn(other.gameObject, settings));
        }
    }

    // Метод для получения настроек для заданного тега
    private TagSettings GetSettingsForTag(string tag)
    {
        foreach (TagSettings settings in tagSettingsList)
        {
            if (settings.tagName == tag)
            {
                return settings;
            }
        }
        return null;
    }

    // Корутина для удаления объекта и создания префаба
    private IEnumerator DestroyAndSpawn(GameObject obj, TagSettings settings)
    {
        // Ждём указанное количество времени
        yield return new WaitForSeconds(settings.destroyDelay);

        // Если префаб указан, создаём его на месте удалённого объекта
        if (settings.spawnPrefab != null)
        {
            Instantiate(settings.spawnPrefab, obj.transform.position, obj.transform.rotation);
        }

        // Удаляем объект
        Destroy(obj);
    }

    // Корутина для движения объекта вверх-вниз
    private IEnumerator MoveObjectUpDown(GameObject obj, TagSettings settings)
    {
        Vector3 startPosition = obj.transform.position;

        // Уникальное смещение по времени для каждого объекта
        float timeOffset = Random.Range(0f, 2f); // Случайное смещение от 0 до 2 секунд

        // Бесконечный цикл для движения объекта вверх-вниз
        while (true)
        {
            // Рассчитываем новую позицию объекта на основе времени, амплитуды и скорости
            float newY = startPosition.y + Mathf.Sin((Time.time + timeOffset) * settings.speed) * settings.amplitude;

            // Применяем новую позицию
            obj.transform.position = new Vector3(obj.transform.position.x, newY, obj.transform.position.z);

            // Ждём до следующего кадра
            yield return null;
        }
    }
}
