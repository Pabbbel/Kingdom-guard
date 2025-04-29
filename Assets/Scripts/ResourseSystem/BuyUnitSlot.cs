using System.Collections.Generic;
using System.Resources;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BuyUnitSlot : MonoBehaviour
{
    [SerializeField] private ResourceIconDatabase _iconDatabase;
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _currentSlotsAmount;
    [SerializeField] private TextMeshProUGUI _currentWarriorCount;
    [SerializeField] private TextMeshProUGUI _currentSlotCost;

    public int _requirementResourceAmount { get; private set; } = 50;
    private ResourceType _resourceType = ResourceType.Gold;
    public int _maxUnitSlots { get; private set; } = 4;
    public int _currentWariorCount { get; private set; } = 0;

    public static event Action WariorAdded;

    private ResourceManager _resourceManager;

    [Inject]
    public void Construct(ResourceManager resourceManager)
    {
        _resourceManager = resourceManager;
    }

    private void Start()
    {
        SimpleUnit.UnitDie += RemoveWarrior;

        InitializeSlots();
        UpdateDisplayText();
    }

    private void OnDisable()
    {
        SimpleUnit.UnitDie -= RemoveWarrior;
    }

    private void InitializeSlots()
    {
        _icon.sprite = _iconDatabase.GetResourseIcon(_resourceType);
        _currentWarriorCount.text = _currentWariorCount.ToString();
        _currentSlotsAmount.text = _maxUnitSlots.ToString();
    }

    private void CurrentSlotCost()
    {
        if (_maxUnitSlots < 5)
            _requirementResourceAmount = 50;
        else if (_maxUnitSlots < 10)
            _requirementResourceAmount = 100;
        else
            _requirementResourceAmount = 150;
    }

    public void TryBuySlot() 
    {
        CurrentSlotCost();

        if (_resourceManager.GetResource(_resourceType) >= _requirementResourceAmount)
        {
            BuySlot();
        }
    }

    private void BuySlot()
    {
        _resourceManager.SpendResource(_resourceType, _requirementResourceAmount);
        AddSlot();
    }

    public void AddSlot()
    {
        _maxUnitSlots++;
        UpdateDisplayText();
    }

    public void AddWarior()
    {
        if(_currentWariorCount < _maxUnitSlots)
            _currentWariorCount++;

        UpdateDisplayText();
        WariorAdded?.Invoke();
    }

    private void RemoveWarrior()
    {
        _currentWariorCount--;
        UpdateDisplayText();
    }

    public void UpdateDisplayText()
    {
        CurrentSlotCost();
        _currentSlotCost.text = _requirementResourceAmount.ToString();
        _currentWarriorCount.text = _currentWariorCount.ToString();
        _currentSlotsAmount.text = _maxUnitSlots.ToString();
    }
}