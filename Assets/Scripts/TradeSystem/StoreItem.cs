using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public class StoreItem : MonoBehaviour
{
    private ResourceType _resourceType;
    public ResourceType ResourceType => _resourceType;

    [SerializeField] private StoreItemData _storeItemData;
    public StoreItemData StoreItemData => _storeItemData;

    [SerializeField] private ResourceIconDatabase _iconDatabase;

    public TradeButton _tradeButton;

    [SerializeField] private Image _tradeResourseImage;
    [SerializeField] private TextMeshProUGUI _tradeResourseText;
    private int _amountTradeResource;

    [SerializeField] private Image _costResourseImage;
    [SerializeField] private TextMeshProUGUI _costResourseText;
    private int _amountCostResource;

    private TradingSystem _tradingSystem;

    // Возможность в инспекторе видеть текущие данные
    [field: SerializeField]
    public string DebugItemName { get; private set; }

    [Inject]
    public void Construct(TradingSystem tradingSystem)
    {
        _tradingSystem = tradingSystem;
    }

    //private void OnEnable()
    //{
    //    TradeButton.ActivateTrading += ActivateTrading;
    //    TradeButton.DeactivateTrading += DeactivateTrading;
    //}

    //private void OnDisable()
    //{
    //    TradeButton.ActivateTrading -= ActivateTrading;
    //    TradeButton.DeactivateTrading -= DeactivateTrading;
    //}

    private void Start()
    {
        DebugItemName = $"{gameObject.name}: {_storeItemData?.TradeResourse}";
        InitializeStoreItems();
    }

    private void InitializeStoreItems()
    {
        _resourceType = _storeItemData.TradeResourse;

        _tradeResourseImage.sprite = _iconDatabase.GetResourseIcon(_storeItemData.TradeResourse);
        _amountTradeResource = _storeItemData.AmountTradeResource;
        _tradeResourseText.text = _amountTradeResource.ToString();

        _costResourseImage.sprite = _iconDatabase.GetResourseIcon(_storeItemData.CostResourse);
        _amountCostResource = _storeItemData.AmountCostResource;
        _costResourseText.text = _amountCostResource.ToString();
    }

    //private void DeactivateTrading()
    //{
    //    _tradingSystem.DeactivateTrading(this);
    //}

    //private void ActivateTrading()
    //{
    //    _tradingSystem.ActivateTrading(this);   
    //}
}
