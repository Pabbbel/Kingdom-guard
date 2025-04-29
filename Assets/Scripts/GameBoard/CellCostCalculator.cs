using UnityEngine;
using Zenject;

public class CellCostCalculator : MonoBehaviour
{
    public int _requirementResource { get; private set; } = 50; // По дефолту клетка стоит 50

    private GameBoard _gameBoard;

    [Inject]
    public void Construct(GameBoard gameBoard)
    {
        _gameBoard = gameBoard;
    }

    private void CurrentCellCost()
    {
        if (_gameBoard.GetActiveCells() < 5)
            _requirementResource = 50;
        else if (_gameBoard.GetActiveCells() < 10)
            _requirementResource = 100;
        else
            _requirementResource = 150;
    }

    public void GetCurrentCellCost()
    {
        CurrentCellCost();
    }
}
