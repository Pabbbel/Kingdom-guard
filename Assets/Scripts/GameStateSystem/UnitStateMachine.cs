using UnityEngine;
using UnityEngine.AI;
using Zenject;

public enum UnitState
{
    Idle,       // Патрулирование
    Moving,     // Перемещение к цели
    Attacking,  // Атака
    Dead        // Уничтожен
}

public class UnitStateMachine : MonoBehaviour
{
    public UnitState CurrentState { get; private set; }

    [SerializeField] private UnitPatrol _unitPatrol;

    private void Start()
    {
        GameStateSystem.OnStateChanged += HandleGameStateChange;
    }

    private void HandleGameStateChange(GameState newState)
    {
        switch (newState)
        {
            case GameState.Prepare:
                TransitionToState(UnitState.Idle);
                break;
            case GameState.Idle:
                TransitionToState(UnitState.Idle);
                break;
            case GameState.EnemyWave:
                // Продолжаем патрулировать
                TransitionToState(UnitState.Idle);
                break;
            case GameState.Combat:
                // Начинаем движение к врагам
                TransitionToState(UnitState.Idle);
                break;
            case GameState.Defeat:
                TransitionToState(UnitState.Dead);
                break;
        }
    }

    private void OnDestroy()
    {
        GameStateSystem.OnStateChanged -= HandleGameStateChange;
    }

    public void TransitionToState(UnitState newState)
    {
        CurrentState = newState;
        switch (newState)
        {
            case UnitState.Idle:
                StartPatrolArea();
                break;
            case UnitState.Moving:
                MoveToTarget();
                break;
            case UnitState.Dead:
                HandleDeath();
                break;
        }
    }

    private void StartPatrolArea()
    {
        // Логика патрулирования в состоянии Idle
        NavMeshAgent agent = gameObject.GetComponent<NavMeshAgent>();
        _unitPatrol.StartPatrol(agent, "PlayerSpawn");
    }

    private void MoveToTarget()
    {
        // Логика движения к врагам
        // Найти ближайшего врага и двигаться к нему
        Debug.Log("Юнит идет к врагу");
    }

    private void HandleDeath()
    {
        // Логика смерти юнита
        Destroy(gameObject);
    }
}