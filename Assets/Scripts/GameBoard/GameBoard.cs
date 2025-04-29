using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _displayText;

    // Используем List для более гибкой работы с кнопками
    private List<Button> _boardButtons;

    // Количество активных кнопок по умолчанию
    private int _defaultActiveButtons = 3;

    // Количество добавляемых кнопок
    private int _addCount = 1;
    private int _currentCount = 0;
    public List<Button> BoardButtons => _boardButtons;

    private void Start()
    {
        InitializeGameBoard();
    }

    private void InitializeGameBoard()
    {
        _boardButtons = new List<Button>(GetComponentsInChildren<Button>());

        // Деактивируем все кнопки перед рандомным выбором
        DeactivateAllButtons();

        // Активируем случайные кнопки
        ActivateRandomButton(_defaultActiveButtons);
    }

    private void DeactivateAllButtons()
    {
        foreach (Button button in _boardButtons)
        {
            button.interactable = false;
        }
    }

    // Метод для активации случайных кнопок
    private void ActivateRandomButton(int count)
    {
        // Создаем список неактивных кнопок
        List<Button> inactiveButtons = _boardButtons.FindAll(button => !button.interactable);

        for (int i = 0; i < count; i++)
        {
            // Если есть неактивные кнопки
            if (inactiveButtons.Count > 0)
            {
                // Выбираем случайную неактивную кнопку
                int randomIndex = Random.Range(0, inactiveButtons.Count);

                // Активируем выбранную кнопку
                inactiveButtons[randomIndex].interactable = true;

                // Удаляем активированную кнопку из списка неактивных
                inactiveButtons.RemoveAt(randomIndex);
            }
            else
            {
                // Если все кнопки уже активны, прерываем цикл
                break;
            }
        }

        // Обновляем счетчик и отображение
        _currentCount += count;
        _displayText.text = _currentCount.ToString() + "/" + _boardButtons.Count.ToString();
    }

    public int GetActiveCells()
    {
        return _currentCount;
    }

    // Метод для добавления новых интерактивных клеток
    public void AddInteractiveButtons()
    {
        ActivateRandomButton(_addCount);
    }
}