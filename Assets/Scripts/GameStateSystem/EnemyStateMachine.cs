using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    public UnitState CurrentState { get; private set; }
    private NavMeshAgent agent;
    [SerializeField] private EnemyMove _enemyMove;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GameStateSystem.OnStateChanged += HandleGameStateChange;
    }

    private void OnDestroy()
    {
        GameStateSystem.OnStateChanged -= HandleGameStateChange;
    }

    public void HandleGameStateChange(GameState newState)
    {
        switch (newState)
        {
            default:
                TransitionToState(UnitState.Moving);
                break;
            case GameState.Defeat:
                TransitionToState(UnitState.Dead);
                break;
        }
    }

    public void TransitionToState(UnitState newState)
    {
        CurrentState = newState;
        switch (newState)
        {
            case UnitState.Moving:
                MoveTowardsPlayerSpawn();
                break;
            case UnitState.Dead:
                HandleDeath();
                break;
        }
    }

    private void MoveTowardsPlayerSpawn()
    {
        // Логика движения к зоне PlayerSpawn с патрулированием
        _enemyMove.StartMove(agent, "PlayerSpawn");
        Debug.Log("Юнит идет");
    }

    private void HandleDeath()
    {
        Destroy(gameObject);
    }
}