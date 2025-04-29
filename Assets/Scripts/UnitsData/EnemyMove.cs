using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Zenject;

public class EnemyMove : MonoBehaviour
{
    private NavMeshAgent agent;
    private float detectionRadius = .2f;
    private bool isEngaged = false;
    private CastleHealthManager castleHealthManager;
    private PlayerUnitSpawner playerUnitSpawner;
    private Vector3 targetPosition;
    private GameObject currentTarget;

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

        castleHealthManager = GameObject.FindFirstObjectByType<CastleHealthManager>();
        playerUnitSpawner = GameObject.FindFirstObjectByType<PlayerUnitSpawner>();
    }

    public void StartMove(NavMeshAgent agent, string areaName)
    {
        this.agent = agent;
        StartCoroutine(MoveToTarget());
        StartCoroutine(ScanForUnits());
    }

    private IEnumerator ScanForUnits()
    {
        while (true)
        {
            if (!isEngaged && playerUnitSpawner != null)
            {
                GameObject nearestUnit = FindNearestUnit();
                if (nearestUnit != null)
                {
                    StartCoroutine(EngageUnit(nearestUnit));
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private GameObject FindNearestUnit()
    {
        GameObject nearest = null;
        float minDistance = float.MaxValue;

        foreach (GameObject unit in playerUnitSpawner.SpawnedUnits)
        {
            if (unit != null)
            {
                float distance = Vector3.Distance(transform.position, unit.transform.position);
                if (distance <= detectionRadius && distance < minDistance)
                {
                    nearest = unit;
                    minDistance = distance;
                }
            }
        }
        return nearest;
    }

    private IEnumerator EngageUnit(GameObject unit)
    {
        if (unit == null) yield break;

        isEngaged = true;
        agent.isStopped = true;
        currentTarget = unit;

        var enemyBase = GetComponent<EnemyBase>();
        var unitBase = unit.GetComponent<UnitBase>();

        while (unit != null && unitBase != null && unitBase.Health > 0)
        {
            float distance = Vector3.Distance(transform.position, unit.transform.position);
            if (distance <= detectionRadius)
            {
                // Поворот к юниту
                Vector3 direction = unit.transform.position - transform.position;
                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
                }

                unitBase.TakeDamage(enemyBase.AttackDamage);
                PlayAttackSound();

                yield return new WaitForSeconds(enemyBase.AttackSpeed);
            }
            else
            {
                // Если юнит убежал - возвращаемся к основной цели
                break;
            }
        }

        // Возвращаемся к движению к цели
        ResetEngagement();
    }

    private void ResetEngagement()
    {
        isEngaged = false;
        agent.isStopped = false;
        currentTarget = null;
        StartCoroutine(MoveToTarget());
    }

    private IEnumerator MoveToTarget()
    {
        while (!isEngaged)
        {
            GameObject target = GameObject.FindGameObjectWithTag("Target");
            if (target != null)
            {
                Vector3 basePosition = target.transform.position;
                float randomOffset = Random.Range(-0.5f, 0.5f);
                targetPosition = basePosition + new Vector3(randomOffset, randomOffset, 0);

                if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, 1f, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                }

                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    StartCoroutine(AttackTarget());
                    yield break;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator AttackTarget()
    {
        var enemyBase = GetComponent<EnemyBase>();

        while (!isEngaged && castleHealthManager != null)
        {
            castleHealthManager.RemoveHealth(enemyBase.AttackDamage);
            yield return new WaitForSeconds(enemyBase.AttackSpeed);
        }
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