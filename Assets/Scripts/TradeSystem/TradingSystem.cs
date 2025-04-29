using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class TradingSystem : MonoBehaviour
{
    [SerializeField] private GameObject _overlayPanel;
    private ResourceManager _resourceManager;

    [SerializeField] private StoreItem[] _storeItem;

    private bool _trading = false;

    private List<StoreItem> ResourseToTrade = new List<StoreItem>();
    private Dictionary<StoreItem, Coroutine> _activeTradings = new Dictionary<StoreItem, Coroutine>();


    private float _productionInterval = 5;

    [Inject]
    public void Construct(ResourceManager resourceManager)
    {
        _resourceManager = resourceManager;
    }

    private void OnEnable()
    {
        ActiveBuildings.OnMarketBuildet += OnMarketBuildet;
        ActiveBuildings.OnMarketNotBuildet += OnMarketNotBuildet;

        TradeButton.ActivateTrading += OnActivateTrading;
        TradeButton.DeactivateTrading += OnDeactivateTrading;
    }

    private void OnDisable()
    {
        ActiveBuildings.OnMarketBuildet -= OnMarketBuildet;
        ActiveBuildings.OnMarketNotBuildet -= OnMarketNotBuildet;

        TradeButton.ActivateTrading -= OnActivateTrading;
        TradeButton.DeactivateTrading -= OnDeactivateTrading;
    }

    private void Start()
    {
        InitializeStoreItems();

        // Добавим подробную диагностику
        Debug.Log($"Всего StoreItem в массиве: {_storeItem.Length}");
        foreach (var item in _storeItem)
        {
            if (item == null)
            {
                Debug.LogError("ВНИМАНИЕ: Один из элементов массива StoreItem == null!");
                continue;
            }

            if (item.StoreItemData == null)
            {
                Debug.LogError($"У StoreItem {item.name} не назначен StoreItemData!");
                continue;
            }

            Debug.Log($"StoreItem: {item.name}, Ресурс для торговли: {item.StoreItemData.TradeResourse}, Количество: {item.StoreItemData.AmountTradeResource}");
        }
    }

    private void InitializeStoreItems()
    {
        _overlayPanel.SetActive(true);

        foreach (StoreItem storeItem in _storeItem)
        {
            ResourseToTrade.Add(storeItem);
            Debug.Log($"Добавлен элемент торговли: {storeItem.StoreItemData.TradeResourse}");
        }

        Debug.Log($"В списке для продажи {ResourseToTrade.Count} элементов");
    }

    private void OnMarketBuildet()
    {
        _trading = true;
        Debug.Log("Рынок построен");
        Debug.Log(_trading);

        _overlayPanel.SetActive(!_trading);
    }

    private void OnMarketNotBuildet()
    {
        _trading = false;
        Debug.Log("Рынок не построен");
        Debug.Log(_trading);

        _overlayPanel.SetActive(!_trading);
        StopAllCoroutines();
    }

    private void OnActivateTrading(StoreItem item)
    {
        ActivateTrading(item);
    }

    private void OnDeactivateTrading(StoreItem item)
    {
        DeactivateTrading(item);
    }

    public void ActivateTrading(StoreItem item)
    {
        if (!_trading)
        {
            Debug.LogWarning("Торговля невозможна - рынок не построен!");
            return;
        }

        ResourceType tradeResourceType = item.StoreItemData.TradeResourse;
        int tradeResourceAmount = item.StoreItemData.AmountTradeResource;

        ResourceType costResourceType = item.StoreItemData.CostResourse;
        int costResourceAmount = item.StoreItemData.AmountCostResource;

        // Проверка ресурсов
        if (_resourceManager.GetResource(tradeResourceType) >= tradeResourceAmount)
        {
            Coroutine tradingCoroutine = StartCoroutine(Trading(item));
            _activeTradings[item] = tradingCoroutine;
        }
        else
        {
            Debug.Log($"Недостаточно ресурсов {costResourceType} для торговли. Требуется: {costResourceAmount}, доступно: {_resourceManager.GetResource(costResourceType)}");
        }
    }

    public void DeactivateTrading(StoreItem item)
    {
        if (_activeTradings.TryGetValue(item, out Coroutine activeCoroutine))
        {
            StopCoroutine(activeCoroutine);
            _activeTradings.Remove(item);
            Debug.Log($"Прекращена торговля для {item.StoreItemData.TradeResourse}");
        }
    }

    private IEnumerator Trading(StoreItem item)
    {
        while (_trading)
        {
            ResourceType tradeResourceType = item.StoreItemData.TradeResourse;
            int tradeResourceAmount = item.StoreItemData.AmountTradeResource;

            ResourceType costResourceType = item.StoreItemData.CostResourse;
            int costResourceAmount = item.StoreItemData.AmountCostResource;

            if (_resourceManager.GetResource(tradeResourceType) >= tradeResourceAmount)
            {
                _resourceManager.SpendResource(tradeResourceType, tradeResourceAmount);
                _resourceManager.AddResource(costResourceType, costResourceAmount);
            }
                

            yield return new WaitForSeconds(_productionInterval);
        }
    }
}
