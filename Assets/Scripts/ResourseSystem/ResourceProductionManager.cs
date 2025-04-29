using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using Zenject;

public class ResourceProductionManager : MonoBehaviour
{
    [SerializeField] private ActiveBuildings _activeBuildings;
    private Dictionary<BuildingType, float> _lastProductionTimes
        = new Dictionary<BuildingType, float>();

    private ResourceManager _resourceManager;
    private BuyUnitSlot _buyUnitSlot;
    private CastleHealthManager _castleHealthManager;


    [Inject]
    public void Construct(ResourceManager resourceManager, BuyUnitSlot buyUnitSlot, CastleHealthManager castleHealthManager)
    {
        _resourceManager = resourceManager;
        _buyUnitSlot = buyUnitSlot;
        _castleHealthManager = castleHealthManager;
    }

    private void Update()
    {
        Dictionary<BuildingType, int> buildingCounts = CountActiveBuildings();

        foreach (var buildingTypeCount in buildingCounts)
        {
            BuildingType buildingType = buildingTypeCount.Key;
            int buildingCount = buildingTypeCount.Value;

            BuildingData buildingData = GetBuildingDataByType(buildingType);

            if (buildingData != null)
            {
                // Инициализируем время последнего производства, если еще не установлено
                if (!_lastProductionTimes.ContainsKey(buildingType))
                {
                    _lastProductionTimes[buildingType] = 0f;
                }

                // Проверяем прошел ли нужный интервал для данного типа здания
                // Используем ProductionInterval из BuildingData
                if (Time.time - _lastProductionTimes[buildingType] >= buildingData.ProductionInterval)
                {
                    if (CanProduce(buildingData, buildingCount))
                    {
                        SpendProductionRequirements(buildingData, buildingCount);
                        ProduceResources(buildingData, buildingCount);
                        //LogProduction(buildingType, buildingCount);

                        // Обновляем время последнего производства
                        _lastProductionTimes[buildingType] = Time.time;
                    }
                }
            }
        }
    }

    private bool CanProduce(BuildingData buildingData, int buildingCount)
    {
        // Если нет требований - можно производить
        if (buildingData.ProductionRequirementsDictionary.Count == 0)
            return true;

        if (buildingData.Type == BuildingType.Camp || buildingData.Type == BuildingType.College)
        {
            // Проверка доступности слотов для воинов
            int currentWarriorSlots = _buyUnitSlot._currentWariorCount;
            int maxWarriorSlots = _buyUnitSlot._maxUnitSlots;

            if (currentWarriorSlots >= maxWarriorSlots)
            {
                //Debug.Log("Нет свободных слотов для производства воинов");
                return false;
            }

            return StartWarriorProduction(buildingData, buildingCount);
        }

        if (buildingData.Type == BuildingType.Workshop)
            HealCastle();

        if (buildingData.Type == BuildingType.Market)
            MarketBuildet(buildingData.Type);

        // Проверяем каждое требование с учетом количества зданий
        foreach (var requirement in buildingData.ProductionRequirementsDictionary)
        {
            int requiredAmount = requirement.Value * buildingCount;

            if (!_resourceManager.HasEnoughResource(requirement.Key, requiredAmount))
            {
                Debug.Log($"Недостаточно ресурса {requirement.Key} для производства. Требуется: {requiredAmount}");
                return false;
            }
        }

        return true;
    }

    private void SpendProductionRequirements(BuildingData buildingData, int buildingCount)
    {
        foreach (var requirement in buildingData.ProductionRequirementsDictionary)
        {
            int requiredAmount = requirement.Value * buildingCount;
            _resourceManager.SpendResource(requirement.Key, requiredAmount);
        }
    }

    private void ProduceResources(BuildingData buildingData, int buildingCount)
    {
        foreach (var resourceEntry in buildingData.Production)
        {
            _resourceManager.AddResource(
                resourceEntry.Type,
                resourceEntry.Amount * buildingCount
            );
        }
    }

    private void LogProduction(BuildingType buildingType, int buildingCount)
    {
        Debug.Log($"Произведено ресурсов зданием {buildingType}: {buildingCount} шт.");
    }

    private Dictionary<BuildingType, int> CountActiveBuildings()
    {
        Dictionary<BuildingType, int> buildingCounts = new Dictionary<BuildingType, int>();

        foreach (BuildingType buildingType in _activeBuildings.BuildedBuildings)
        {
            if (!buildingCounts.ContainsKey(buildingType))
            {
                buildingCounts[buildingType] = 0;
            }
            buildingCounts[buildingType]++;
        }

        return buildingCounts;
    }

    // Метод для получения BuildingData по типу здания
    private BuildingData GetBuildingDataByType(BuildingType type)
    {
        // Здесь нужно будет реализовать логику поиска BuildingData
        // Например, через Resources.LoadAll или через создание списка в инспекторе

        // Временное решение - использую LoadAll
        BuildingData[] allBuildings = Resources.LoadAll<BuildingData>("BuildingTypes");
        return allBuildings.FirstOrDefault(b => b.Type == type);
    }

    private bool StartWarriorProduction(BuildingData buildingData, int buildingCount)
    {
        if (buildingData.Type == BuildingType.Camp)
            return WarriorProduction(buildingData, buildingCount);
        else
            return StrongWarriorProduction(buildingData, buildingCount);
    }

    private bool WarriorProduction(BuildingData buildingData, int buildingCount)
    {
        // Производство обычных воинов
        int warriorsToAdd = buildingCount; // Например, одно здание = один воин

        // Проверка, что не превышаем максимальное количество слотов
        if (_buyUnitSlot._currentWariorCount + warriorsToAdd > _buyUnitSlot._maxUnitSlots)
        {
            //Debug.Log("Невозможно произвести воинов - недостаточно слотов");
            return false;
        }

        // Здесь можно добавить логику производства ресурсов для воинов
        for (int i = 0; i < warriorsToAdd; i++)
        {
            _buyUnitSlot.AddWarior();
        }

        return true;
    }

    private bool StrongWarriorProduction(BuildingData buildingData, int buildingCount)
    {
        // Аналогично, но для более сильных воинов
        // Можно добавить дополнительную логику стоимости, требований и т.д.
        //int strongWarriorsToAdd = buildingCount;

        //if (_buyUnitSlot._currentWariorCount + strongWarriorsToAdd > _buyUnitSlot._maxUnitSlots)
        //{
        //    Debug.Log("Невозможно произвести элитных воинов - недостаточно слотов");
        //    return false;
        //}

        //// Логика производства элитных воинов
        //for (int i = 0; i < strongWarriorsToAdd; i++)
        //{
        //    _buyUnitSlot.AddWarior();
        //}

        return true;
    }

    private void MarketBuildet(BuildingType type)
    {
        
    }

    private void HealCastle()
    {
        int health = 1;
        if (_castleHealthManager.GetCastleHealth() <= 100)
            _castleHealthManager.AddHealth(health);
    }
}