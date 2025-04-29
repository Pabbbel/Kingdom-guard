using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class ActiveBuildings : MonoBehaviour
{
    public List<BuildingType> BuildedBuildings= new List<BuildingType>();

    public BuildingData _buildingData;

    public static event Action OnMarketBuildet;
    public static event Action OnMarketNotBuildet;

    public void AddBuilding(BuildingType type)
    {
        // Добавляем здание в список
        if (type != BuildingType.Market)
        {
            BuildedBuildings.Add(type);
            Debug.Log($"Здание {type} добавлено. Всего зданий этого типа: {BuildedBuildings.Count(b => b == type)}");
        }
        else
        {
            BuildedBuildings.Add(type);
            Debug.Log($"Здание {type} добавлено. Всего зданий этого типа: {BuildedBuildings.Count(b => b == type)}");
            MarketBuildet(type);
        }
    }

    public void RemoveBuilding(BuildingType type)
    {
        // Находим первое вхождение здания и удаляем его
        int index = BuildedBuildings.IndexOf(type);

        if (type != BuildingType.Market)
        {
            if (index != -1)
            {
                BuildedBuildings.RemoveAt(index);
                Debug.Log($"Здание {type} удалено. Осталось зданий этого типа: {BuildedBuildings.Count(b => b == type)}");
            }
        }
        else
        {

            if (index != -1)
            {
                BuildedBuildings.RemoveAt(index);
                Debug.Log($"Здание {type} удалено. Осталось зданий этого типа: {BuildedBuildings.Count(b => b == type)}");
            }
            MarketBuildet(type);
        }
    }

    private void MarketBuildet(BuildingType type)
    {
        if (BuildedBuildings.Count(b => b == type) > 0)
            OnMarketBuildet?.Invoke();
        else
            OnMarketNotBuildet?.Invoke();
    }
}
