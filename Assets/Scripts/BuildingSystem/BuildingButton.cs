using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections;
using Zenject;
using System.Collections.Generic;
using Unity.VisualScripting;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BuildingButton : MonoBehaviour
{
    public BuildingData _buildingData;
    [SerializeField] private Button _buyButton;

    private Button _cancelBuildButton;

    private ResourceManager _resourceManager;
    private BuildingManager _buildingManager;
    private BuildingDescriptionDisplay _buildingDescriptionDisplay;

    [Inject]
    public void Construct(ResourceManager resourceManager, BuildingManager buildingManager, BuildingDescriptionDisplay buildingDescriptionDisplay)
    {
        _resourceManager = resourceManager;
        _buildingManager = buildingManager;
        _buildingDescriptionDisplay = buildingDescriptionDisplay;
    }

    private void Start()
    {
        _cancelBuildButton = _buildingManager.CancelBuildButton;
        _cancelBuildButton.onClick.AddListener(CancelBuild);
    }
    private void CancelBuild()
    {
        // Сбрасываем выбранное здание
        _buildingManager.CancelBuild();
    }

    public void OnClickBuildingButton()
    {
        _buyButton.onClick.RemoveAllListeners();
        // Показываем панель с описанием
        _buildingDescriptionDisplay.ShowDescriptionPanel();
        Debug.Log($"Здание {_buildingData.Type} готовится к постройке");

        // Обновляем информацию о стоимости
        _buildingDescriptionDisplay.UpdateCostDisplay(_buildingData);

        // Обновляем информацию о производстве
        _buildingDescriptionDisplay.UpdateProductionDisplay(_buildingData);

        // Обновляем информацию о требованиях к производству
        _buildingDescriptionDisplay.UpdateRequiredResourceDisplay(_buildingData);

        bool canAffordBuilding = _buildingData.BuildCostDictionary.All(cost =>
            _resourceManager.GetResource(cost.Key) >= cost.Value);

        if (canAffordBuilding)
        {
            _buyButton.interactable = true;
        }
        else
        {
            _buyButton.interactable = false;
        }

        _buyButton.onClick.AddListener(OnBuyButtonClick);
    }

    private void OnBuyButtonClick()
    {
        _cancelBuildButton.gameObject.SetActive(true);

        // Проверяем наличие ресурсов для всех типов
        bool canAffordBuilding = _buildingData.BuildCostDictionary.All(cost =>
            _resourceManager.GetResource(cost.Key) >= cost.Value);

        if (canAffordBuilding)
        {
            _buyButton.interactable = true;
            // Выбираем здание для постройки
            _buildingManager.SelectBuilding(_buildingData);

            Debug.Log($"Здание {_buildingData.Type} готово к постройке");

            _buildingDescriptionDisplay.HideDescriptionPanel();

            NavPanelButton navPanelButton = FindFirstObjectByType<NavPanelButton>();
            navPanelButton.SwithTriggerAnimator();
        }
        else
        {
            _buyButton.interactable = false;
            _cancelBuildButton.gameObject.SetActive(false);
            _buildingManager.CancelBuild();
            Debug.Log("Недостаточно ресурсов для постройки!");
        }
    }
}