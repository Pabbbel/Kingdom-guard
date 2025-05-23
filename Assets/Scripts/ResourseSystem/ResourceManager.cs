using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{

    // Словарь для хранения текущих ресурсов игрока
    private Dictionary<ResourceType, int> _resources = new Dictionary<ResourceType, int>();

    // Событие для обновления UI или других систем
    public delegate void OnResourceChange(ResourceType type, int newAmount);
    public event OnResourceChange OnResourceChange;

    private void Awake()
    {
        InitializeResources();
    }

    // Инициализация всех ресурсов (золото = 150, остальные = 0)
    private void InitializeResources()
    {
        foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
        {
            _resources[type] = type == ResourceType.Gold ? 900 : 150;
        }
    }

    // Получение текущего количества ресурса
    public int GetResource(ResourceType type)
    {
        return _resources.TryGetValue(type, out int amount) ? amount : 0;
    }

    // Добавление ресурса
    public void AddResource(ResourceType type, int amount)
    {
        if (!_resources.ContainsKey(type)) _resources[type] = 0;

        _resources[type] += amount;
        TriggerResourceChanged(type);
    }

    // Уменьшение ресурса
    public bool SpendResource(ResourceType type, int amount)
    {
        if (GetResource(type) >= amount)
        {
            _resources[type] -= amount;
            TriggerResourceChanged(type);
            return true;
        }
        return false; // Не хватает ресурса
    }

    public bool HasEnoughResource(ResourceType type, int amount)
    {
        return GetResource(type) >= amount;
    }

    // Вызов события при изменении ресурса
    private void TriggerResourceChanged(ResourceType type)
    {
        //Debug.Log($"Ресурс {type} изменен. Новое количество: {_resources[type]}");
        OnResourceChange?.Invoke(type, _resources[type]);
    }
}