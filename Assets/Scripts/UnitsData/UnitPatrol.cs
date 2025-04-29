using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Zenject;

public class UnitPatrol : MonoBehaviour
{
    private NavMeshAgent agent;
    private const string UNITAREA = "PlayerSpawn";
    private float detectionRadius = .1f;
    private float castleDefenseRadius = 2f; // Радиус защиты замка
    private bool isEngaged = false;
    private Vector3 patrolCenter;
    private EnemySpawner enemySpawner;
    private GameObject castle; // Ссылка на замок

    private SoundSystem _soundSystem;

    [Inject]
    public void Construct(SoundSystem soundSystem)
    {
        _soundSystem = soundSystem;
    }

    private void Start()
    {
        if (_soundSystem = null)
            Debug.LogWarning("SoundSystem не внедрен");

         _soundSystem = GameObject.FindFirstObjectByType<SoundSystem>();

        enemySpawner = GameObject.FindFirstObjectByType<EnemySpawner>();
        castle = GameObject.FindGameObjectWithTag("Target"); // Находим замок
    }

    public void StartPatrol(NavMeshAgent agent, string areaName)
    {
        this.agent = agent;
        patrolCenter = GetLeftCenterSpawnPoint(UNITAREA);
        StartCoroutine(PatrolArea());
        StartCoroutine(ScanForThreats());
    }

    private IEnumerator ScanForThreats()
    {
        while (true)
        {
            if (!isEngaged && enemySpawner != null)
            {
                GameObject targetEnemy = FindPriorityTarget();
                if (targetEnemy != null)
                {
                    StartCoroutine(EngageEnemy(targetEnemy));
                }
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    private GameObject FindPriorityTarget()
    {
        GameObject priorityTarget = null;
        float minDistanceToCastle = float.MaxValue;

        foreach (GameObject enemy in enemySpawner.activeEnemies)
        {
            if (enemy != null)
            {
                // Проверяем расстояние от врага до замка
                float enemyToCastleDistance = Vector3.Distance(enemy.transform.position, castle.transform.position);
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

                // Приоритет отдаётся врагам, которые:
                // 1. Находятся в радиусе защиты замка
                // 2. Находятся ближе к замку
                // 3. Находятся в пределах досягаемости юнита
                if (enemyToCastleDistance < castleDefenseRadius)
                {
                    // Если враг близко к замку, увеличиваем радиус обнаружения
                    float effectiveDetectionRadius = detectionRadius;

                    if (distanceToEnemy <= effectiveDetectionRadius &&
                        enemyToCastleDistance < minDistanceToCastle)
                    {
                        priorityTarget = enemy;
                        minDistanceToCastle = enemyToCastleDistance;
                    }
                }
                // Если нет врагов у замка, ищем любых врагов в радиусе обнаружения
                else if (priorityTarget == null && distanceToEnemy <= detectionRadius)
                {
                    priorityTarget = enemy;
                    minDistanceToCastle = enemyToCastleDistance;
                }
            }
        }
        return priorityTarget;
    }

    private IEnumerator EngageEnemy(GameObject enemy)
    {
        if (enemy == null) yield break;

        isEngaged = true;
        agent.isStopped = false; // Разрешаем движение для преследования
        var unitBase = GetComponent<SimpleUnit>();
        var enemyBase = enemy.GetComponent<EnemyBase>();

        while (enemy != null && enemyBase != null && enemyBase.Health > 0)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy <= detectionRadius) // Дистанция для атаки
            {
                agent.isStopped = true;

                // Поворот к врагу
                Vector3 direction = enemy.transform.position - transform.position;
                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
                }

                enemyBase.TakeDamage(unitBase.AttackDamage);

                Debug.LogWarning("Юнит атакует врага");
                PlayAttackSound();
                    
                yield return new WaitForSeconds(unitBase.AttackSpeed);
            }
            else
            {
                // Преследуем врага
                agent.isStopped = false;
                agent.SetDestination(enemy.transform.position);
                yield return new WaitForSeconds(0.1f);

                // Если враг слишком далеко убежал, прекращаем преследование
                if (distanceToEnemy > detectionRadius * 2)
                {
                    break;
                }
            }
        }

        // Возвращаемся к патрулированию
        isEngaged = false;
        agent.isStopped = false;
        StartCoroutine(PatrolArea());
    }

    private IEnumerator PatrolArea()
    {
        float patrolRadius = Random.Range(1f, 2f);
        float verticalAmplitude = Random.Range(0.5f, 1.5f);
        float uniqueTimeOffset = Random.Range(0f, Mathf.PI * 2);

        Vector3 uniqueOffset = new Vector3(
            Random.Range(-2f, 2f),
            Random.Range(-1f, 1f),
            0
        );

        // Если замок существует, смещаем патрулирование ближе к нему
        if (castle != null)
        {
            Vector3 directionToCastle = (castle.transform.position - patrolCenter).normalized;
            uniqueOffset += directionToCastle * Random.Range(0f, 2f);
        }

        Vector3 adjustedCenterPoint = patrolCenter + uniqueOffset;

        while (!isEngaged)
        {
            float horizontalOffset = Mathf.PingPong(Time.time + uniqueTimeOffset, patrolRadius * 2) - patrolRadius;
            float verticalOffset = Mathf.Sin(Time.time + uniqueTimeOffset) * verticalAmplitude;

            Vector3 targetPosition = adjustedCenterPoint + new Vector3(horizontalOffset, verticalOffset, 0);

            if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private Vector3 GetLeftCenterSpawnPoint(string areaName)
        {
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10f, 1 << NavMesh.GetAreaFromName(areaName)))
            {
                return hit.position;
            }
            Debug.LogWarning($"Не удалось найти точку в области {areaName}");
            return transform.position;
        }

    private void PlayAttackSound()
    {
        int index = Random.Range(1, 3);

        string[] attackSounds = new string[3];
        attackSounds[0] = "Sword";
        attackSounds[1] = "Sword1";
        attackSounds[2] = "Sword2";

        _soundSystem.PlaySound(attackSounds[index]);
    }
}

