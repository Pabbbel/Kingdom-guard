using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemySpawnPoint;
    [SerializeField] private WaveSystem _waveSystem;

    public List<GameObject> activeEnemies = new List<GameObject>();

    private GameStateSystem _gameStateSystem;

    private static event Action EnemySpawned;

    [Inject]
    public void Construct(GameStateSystem gameStateSystem)
    {
        _gameStateSystem = gameStateSystem;
    }

    private void Start()
    {
        GameStateSystem.OnStateChanged += HandleGameStateChange;
    }

    private void HandleGameStateChange(GameState newState)
    {
        if (newState == GameState.EnemyWave)
        {
            StartSpawningEnemies();
        }
    }

    private void StartSpawningEnemies()
    {
        StartCoroutine(SpawnEnemyWave());
    }

    private IEnumerator SpawnEnemyWave()
    {
        int enemyCount = _waveSystem.EnemyWaveCount;

        for (int i = 0; i < enemyCount; i++)
        {
            // Добавим небольшой спред при спавне
            Vector3 randomOffset = UnityEngine.Random.insideUnitSphere * .2f;
            randomOffset.z = 0;

            GameObject enemy = Instantiate(
                enemyPrefab,
                enemySpawnPoint.position + randomOffset,
                Quaternion.identity
            );

            enemy.AddComponent<EnemyBase>();

            // При спавне каждый юнит получает текущее состояние игры
            EnemyStateMachine enemyStateMachine = enemy.GetComponent<EnemyStateMachine>();
            if (enemyStateMachine != null)
            {
                enemyStateMachine.TransitionToState(enemyStateMachine.CurrentState);

                activeEnemies.Add(enemy);
                Debug.Log("Юнит создан и должен начать наступление.");

                yield return new WaitForSeconds(.1f); // Интервал между спавном
            }
        }
        _waveSystem.AddWave();
        EnemySpawned?.Invoke();
        Debug.Log("Волна врагов заспавнилась");

        _gameStateSystem.ChangeState(GameState.Combat);
    }
}
