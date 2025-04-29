using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class BuyCellButton : MonoBehaviour
{
    [SerializeField] private ResourceIconDatabase _iconDatabase;
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _countText;

    private Sprite _iconSprite;
    public ResourceType _requirementResource = ResourceType.Wood;
    private Button _button;

    private CellCostCalculator _cellCostCalculator;
    private GameBoard _gameBoard;
    private ResourceManager _resourceManager;

    [Inject]
    public void Construct(CellCostCalculator cellCostCalculator, GameBoard gameBoard, ResourceManager resourceManager)
    {
        _cellCostCalculator = cellCostCalculator;
        _gameBoard = gameBoard;
        _resourceManager = resourceManager;
    }

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(CalculateCost);
        _iconSprite = _iconDatabase.GetResourseIcon(_requirementResource);
        CalculateCost();
    }

    public void CalculateCost()
    {
        _cellCostCalculator.GetCurrentCellCost();
        UpdateRequiredResourceDisplay(_cellCostCalculator._requirementResource);
    }

    // Метод для обновления текста с требуемым количеством
    private void UpdateRequiredResourceDisplay(int currentCost)
    {
        // Обновляем иконку ресурса
        _icon.sprite = _iconSprite;

        // Выводим только требуемое количество ресурсов
        _countText.text = currentCost.ToString();
    }

    public void OnBuyCell()
    {
        // Найдем подходящую стоимость открытия
        int currentCost = _cellCostCalculator._requirementResource;

        // Проверяем, есть ли еще место для открытия
        if (IsAllCellsOpened())
        {
            DisableButton();
            return;
        }

        BuyCell(currentCost);
    }

    private bool IsAllCellsOpened() =>
    _gameBoard.GetActiveCells() == _gameBoard.BoardButtons.Count;

    private void DisableButton()
    {
        _button.interactable = false;
        _button.GetComponentInChildren<TextMeshProUGUI>().text = "Все открыто";
    }

    private void BuyCell(int currentCost)
    {
        // Проверяем наличие ресурсов
        if (_resourceManager.GetResource(_requirementResource) >= currentCost)
        {
            // Списываем ресурсы
            if (_resourceManager.SpendResource(_requirementResource, currentCost))
            {
                // Открываем новые клетки
                _gameBoard.AddInteractiveButtons();
            }
        }
        else
        {
            Debug.Log($"Недостаточно {_requirementResource} для открытия клеток!");
        }
    }
}