using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private float shakeDuration = 0.5f;  // Длительность тряски
    [SerializeField] private float shakeStrength = 10f;   // Максимальная сила смещения
    [SerializeField] private AnimationCurve shakeCurve;   // Кривая затухания

    private RectTransform[] childTransforms;             // Массив дочерних объектов канваса
    private Vector3[] initialPositions;                 // Исходные позиции дочерних объектов
    private bool isShaking = false;

    private void Start()
    {
        // Сохраняем ссылки на дочерние объекты
        RectTransform canvasTransform = GetComponent<RectTransform>();
        if (canvasTransform == null)
        {
            //Debug.LogError("CameraShake: Компонент RectTransform не найден на канвасе!");
            return;
        }

        childTransforms = canvasTransform.GetComponentsInChildren<RectTransform>();
        initialPositions = new Vector3[childTransforms.Length];

        // Сохраняем изначальные позиции всех объектов
        for (int i = 0; i < childTransforms.Length; i++)
        {
            initialPositions[i] = childTransforms[i].localPosition;
        }
    }

    /// <summary>
    /// Запускает эффект тряски.
    /// </summary>
    public void StartShake()
    {
        if (!isShaking)
        {
            StartCoroutine(Shake());
        }
    }

    private IEnumerator Shake()
    {
        isShaking = true;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float strengthMultiplier = shakeCurve.Evaluate(elapsedTime / shakeDuration);
            float offsetX = Random.Range(-1f, 1f) * shakeStrength * strengthMultiplier;
            float offsetY = Random.Range(-1f, 1f) * shakeStrength * strengthMultiplier;

            Vector3 shakeOffset = new Vector3(offsetX, offsetY, 0f);

            // Применяем тряску ко всем дочерним объектам
            for (int i = 0; i < childTransforms.Length; i++)
            {
                childTransforms[i].localPosition = initialPositions[i] + shakeOffset;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Возвращаем все объекты в исходное положение
        for (int i = 0; i < childTransforms.Length; i++)
        {
            childTransforms[i].localPosition = initialPositions[i];
        }

        isShaking = false;
    }
}
