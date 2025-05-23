using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _enemySpawnPoint;
    [SerializeField] private WaveSystem _waveSystem;

    public List<GameObject> ActiveEnemies = new List<GameObject>();

    private GameStateSystem _gameStateSystem;

    private static event Action s_EnemySpawned;

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
                _enemyPrefab,
                _enemySpawnPoint.position + randomOffset,
                Quaternion.identity
            );

            enemy.AddComponent<EnemyBase>();

            // При спавне каждый юнит получает текущее состояние игры
            EnemyStateMachine enemyStateMachine = enemy.GetComponent<EnemyStateMachine>();
            if (enemyStateMachine != null)
            {
                enemyStateMachine.TransitionToState(enemyStateMachine.CurrentState);

                ActiveEnemies.Add(enemy);
                Debug.Log("Юнит создан и должен начать наступление.");

                yield return new WaitForSeconds(.1f); // Интервал между спавном
            }
        }
        _waveSystem.AddWave();
        s_EnemySpawned?.Invoke();
        Debug.Log("Волна врагов заспавнилась");

        _gameStateSystem.ChangeState(GameState.Combat);
    }
}
