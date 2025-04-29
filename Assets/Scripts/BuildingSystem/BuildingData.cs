using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Типы ресурсов
public enum ResourceType
{
    Gold, // Золото
    Wood, // Дерево
    Meat, // Мясо
    Food, // Еда
    Clay, // Глина
    Brick, // Кирпичи
    Ore, // Руда
    Ingot, // Слитки
    Warrior, //Солдат
    StrongWarrior, //усиленный солдат
    HitPoint, // Здоровье
    Empty
}

//Типы строений
public enum BuildingType
{
    Empty,
    Forest, // Лес. Добывает дерево
    Farm, // Ферма. Добывает мясо
    Market, // Рынок. Дает возможность торговать
    Camp, // Лагерь. Дает возможность нанимать воинов
    Mine, // Шахта. Добывает руду
    Quarry, // Каменоломня. Добывает глину
    Workshop, // Мастерская. Хилит стены
    Cafe, // Кафе. Производит еду
    Mall, // Торговый центр. Сам генерирует золото
    College, // Колледж. Обучает воинов и делает их усиленными
    Foundry, // Литейный цех. Добывает слитки
    Plant // Завод. Производит кирпичи
}

[System.Serializable]
public class ResourceEntry
{
    // Сериализуемый класс, который объединяет тип и количество ресурса
    public ResourceType Type;
    public int Amount;

    // Конструктор для удобства
    public ResourceEntry(ResourceType type, int amount)
    {
        Type = type;
        Amount = amount;
    }
}

[CreateAssetMenu(fileName = "BuildingData", menuName = "Game/Building")]
public class BuildingData : ScriptableObject
{
    [Header("Тип строения")]
    public string BuildingName;
    public BuildingType Type;
    // Интервал производства для этого типа здания
    public float ProductionInterval = 1f;
    public int MaxResourceAmount = 999;

    // Список для покупки
    [Header("Стоимость строения")]
    public List<ResourceEntry> BuildCost = new List<ResourceEntry>();

    // Список для производства
    [Header("Производство строения")]
    public List<ResourceEntry> Production = new List<ResourceEntry>();

    // Список требований для производства
    [Header("Требования строения (может быть пустым)")]
    public List<ResourceEntry> ProductionRequirements = new List<ResourceEntry>();

    // Ленивая инициализация словаря
    private Dictionary<ResourceType, int> _buildCostDictionary;
    private Dictionary<ResourceType, int> _productionDictionary;
    private Dictionary<ResourceType, int> _productionRequirementsDictionary;

    // Преобразование списка в словарь с обработкой дубликатов
    private Dictionary<ResourceType, int> ConvertToDictionary(List<ResourceEntry> entries, bool suppressWarnings = false)
    {
        // Если список пуст - возвращаем пустой словарь и полуяаем уведомление в консоли. Код не выбрасывает
        if (entries == null || entries.Count == 0)
        {
            if (!suppressWarnings)
            {
                Debug.LogWarning($"Список ресурсов для {BuildingName} пуст!");
            }
            return new Dictionary<ResourceType, int>();
        }

        // Групировка одинаковых значений
        return entries
            .GroupBy(entry => entry.Type)
            .ToDictionary(
                group => group.Key,
                group => group.Sum(entry => entry.Amount)
            );
    }

    // Свойства для получения словарей
    public Dictionary<ResourceType, int> BuildCostDictionary
    {
        get
        {
            if (_buildCostDictionary == null)
            {
                _buildCostDictionary = ConvertToDictionary(BuildCost);
            }
            return _buildCostDictionary;
        }
    }

    public Dictionary<ResourceType, int> ProductionDictionary
    {
        get
        {
            if (_productionDictionary == null)
            {
                _productionDictionary = ConvertToDictionary(Production);
            }
            return _productionDictionary;
        }
    }

    public Dictionary<ResourceType, int> ProductionRequirementsDictionary
    {
        get
        {
            if (_productionRequirementsDictionary == null)
            {
                _productionRequirementsDictionary = ConvertToDictionary(ProductionRequirements, suppressWarnings: true);
            }
            return _productionRequirementsDictionary;
        }
    }
}