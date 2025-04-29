using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BuildingManager : MonoBehaviour
{
    // Ссылки на необходимые компоненты
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private Button _cancelBuildButton;

    public Button CancelBuildButton => _cancelBuildButton;

    private ActiveBuildings _activeBuildings;
    private SoundSystem _soundSystem;

    [Inject]
    public void Construct(ActiveBuildings activeBuildings, SoundSystem soundSystem)
    {
        _activeBuildings = activeBuildings;
        _soundSystem = soundSystem;
    }

    // Текущее выбранное здание для постройки
    private BuildingData _selectedBuilding;
    [SerializeField] private CameraShake _cameraShake;

    private void Start()
    {
        _cancelBuildButton.gameObject.SetActive(false);
        _cancelBuildButton.onClick.AddListener(CancelBuild);
    }

    // Метод вызывается при выборе здания в UI
    public void SelectBuilding(BuildingData buildingData)
    {
        _selectedBuilding = buildingData;
    }

    public void CancelBuild()
    {
        // Сбрасываем выбранное здание
        _selectedBuilding = null;
        _cancelBuildButton.gameObject.SetActive(false);
    }

    // Метод вызывается при клике на клетку поля
    public void TryBuildBuilding(Button targetButton)
    {
        // Если здание не выбрано - выходим
        if (_selectedBuilding == null) return;

        // Проверяем наличие ресурсов
        if (!CanAffordBuilding(_selectedBuilding))
        {
            Debug.Log($"Недостаточно ресурсов для постройки {_selectedBuilding}");
            // Сбрасываем выбранное здание
            _selectedBuilding = null;
            _cancelBuildButton.gameObject.SetActive(false);
            return;
        }

        // Если на этом месте уже было здание, удаляем его
        BuildingType existingBuildingType = GetExistingBuildingType(targetButton);
        if (existingBuildingType != BuildingType.Empty) // Empty - это дефолтное состояние
        {
            _activeBuildings.RemoveBuilding(existingBuildingType);
        }

        // Списываем ресурсы
        SpendBuildingResources(_selectedBuilding);

        // Заменяем визуализацию кнопки
        ReplaceButtonVisual(targetButton, _selectedBuilding);

        // Добавляем новое здание
        AddActiveBuilding(_selectedBuilding);

        _cameraShake.StartShake();
        _soundSystem.PlaySound("Build");

        // Сбрасываем выбранное здание
        _selectedBuilding = null;
    }

    // Метод для определения существующего типа здания
    private BuildingType GetExistingBuildingType(Button button)
    {
        // Логика определения типа существующего здания
        // Например, через аниматор или какой-то другой компонент
        Animator imageAnimator = button.GetComponent<Animator>();

        foreach (BuildingType buildingType in System.Enum.GetValues(typeof(BuildingType)))
        {
            if (imageAnimator.GetCurrentAnimatorStateInfo(0).IsName(buildingType.ToString()))
            {
                return buildingType;
            }
        }

        return BuildingType.Empty; // Дефолтное состояние
    }

    private bool CanAffordBuilding(BuildingData building)
    {
        foreach (var resourceEntry in building.BuildCost)
        {
            if (!_resourceManager.HasEnoughResource(resourceEntry.Type, resourceEntry.Amount))
                return false;
        }
        return true;
    }

    private void SpendBuildingResources(BuildingData building)
    {
        foreach (var resourceEntry in building.BuildCost)
        {
            _resourceManager.SpendResource(resourceEntry.Type, resourceEntry.Amount);
        }
    }

    private void ReplaceButtonVisual(Button button, BuildingData building)
    {
        Debug.Log($"Здание {building.BuildingName} построено");
        // Логика замены спрайта/анимации кнопки
        Animator imageAnimator = button.GetComponent<Animator>();
        imageAnimator.SetTrigger($"{building.Type}");
        _cancelBuildButton.gameObject.SetActive(false);
    }

    private void AddActiveBuilding(BuildingData building)
    {
        if (_activeBuildings != null)
        {
            // Добавляем тип здания в список построенных зданий
            _activeBuildings.AddBuilding(building.Type);

            Debug.Log($"Добавлено активное здание: {building.BuildingName}");
        }
        else
        {
            Debug.LogError("Не найден компонент ActiveBuildings на сцене!");
        }
    }
}