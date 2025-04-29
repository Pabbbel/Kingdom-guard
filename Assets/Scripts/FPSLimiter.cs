using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    [SerializeField] private int _targetFrameRate = 60; // Можно настроить в инспекторе

    void Awake()
    {
        // Устанавливаем целевое количество кадров
        QualitySettings.vSyncCount = 0; // Отключаем вертикальную синхронизацию
        Application.targetFrameRate = _targetFrameRate;
    }
}
