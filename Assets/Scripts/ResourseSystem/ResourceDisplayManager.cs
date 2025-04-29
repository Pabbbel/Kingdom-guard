using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Zenject;

public class ResourceDisplayManager : MonoBehaviour
{
    // Создаем словарь для связи типа ресурса и текстового поля
    [System.Serializable]
    public class ResourceTextMapping
    {
        public ResourceType ResourceType;
        public TextMeshProUGUI DisplayText;
    }

    // Массив для инспектора Unity, чтобы можно было настроить связи
    [SerializeField] private ResourceTextMapping[] ResourceDisplays;

    // Словарь для быстрого доступа к текстовым полям
    private Dictionary<ResourceType, TextMeshProUGUI> _resourceTextMap;

    private ResourceManager _resourceManager;

    [Inject]
    public void Construct(ResourceManager resourceManager)
    {
        _resourceManager = resourceManager;
    }

    private void Start()
    {
        // Создаем словарь для быстрого доступа
        InitializeResourceTextMapping();

        // Подписываемся на событие изменения ресурсов
        _resourceManager.onResourceChange += UpdateResourceDisplay;

        // Первоначальное обновление всех значений
        UpdateAllResourceDisplays();
    }

    // Создаем словарь для быстрого доступа к текстовым полям
    private void InitializeResourceTextMapping()
    {
        _resourceTextMap = new Dictionary<ResourceType, TextMeshProUGUI>();
        foreach (var mapping in ResourceDisplays)
        {
            _resourceTextMap[mapping.ResourceType] = mapping.DisplayText;
        }
    }

    // Обновление конкретного ресурса
    private void UpdateResourceDisplay(ResourceType type, int newAmount)
    {
        if (type != ResourceType.HitPoint)
        {
            // Проверяем, есть ли текстовое поле для этого ресурса
            if (_resourceTextMap.TryGetValue(type, out TextMeshProUGUI displayText))
            {
                displayText.text = newAmount.ToString();
            }
            else
            {
                Debug.Log($"TextMeshPro компонент не назначен для {type}");
            }
        }
    }

    // Обновление всех ресурсов при старте
    private void UpdateAllResourceDisplays()
    {
        foreach (var resourceType in _resourceTextMap.Keys)
        {
            int currentAmount = _resourceManager.GetResource(resourceType);
            _resourceTextMap[resourceType].text = currentAmount.ToString();
        }
    }

    // Важно: отписываемся от события
    private void OnDestroy()
    {
        _resourceManager.onResourceChange -= UpdateResourceDisplay;
    }
}
