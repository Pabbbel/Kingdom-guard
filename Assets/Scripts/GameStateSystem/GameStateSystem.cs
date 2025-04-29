using UnityEngine;
using System;
using TMPro;
using UnityEditor.SceneManagement;

public enum GameState
{
    Prepare,    // Подготовка    
    Idle,       // Покой
    EnemyWave,  // Волна врагов
    Combat,     // Атака
    Defeat      // Поражение
}

public class GameStateSystem : MonoBehaviour
{
    public GameState CurrentState { get; private set; } = GameState.Prepare;

    public string CurrentGameState;

    public static event Action<GameState> OnStateChanged;

    private void Awake()
    {
        ChangeState(GameState.Prepare);
    }

    private void Update()
    {
        CurrentGameState = CurrentState.ToString();
    }

    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
        OnStateChanged?.Invoke(newState);

        switch (newState)
        {
            case GameState.Prepare:
                StartPrepare();
                break;
            case GameState.Idle:
                HandleIdleState();
                break;
            case GameState.EnemyWave:
                StartEnemyWave();
                break;
            case GameState.Combat:
                StartCombat();
                break;
            case GameState.Defeat:
                HandleDefeat();
                break;
        }
    }

    private void StartPrepare()
    {

    }

    private void HandleIdleState()
    {
        // Юниты патрулируют
        // Останавливаем спавн врагов
    }

    private void StartEnemyWave()
    {
        // Начинаем спавн врагов
        // Юниты продолжают патрулировать
    }

    private void StartCombat()
    {
        // Юниты переключаются в боевой режим
        // Идут к врагам
    }

    private void HandleDefeat()
    {
        // Останавливаем все spawners
        // Показываем экран поражения
    }
}

