using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class PlayerUnitSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerUnitPrefab; // Префаб юнита игрока
    [SerializeField] private Transform _spawnPosition;
    private int initialUnits = 0; // Начальное количество юнитов

    private NavMeshTriangulation triangulation;
    public List<GameObject> SpawnedUnits = new List<GameObject>();

    private void OnEnable()
    {
        BuyUnitSlot.WariorAdded += SpawnPlayerUnit;
    }

    private void OnDisable()
    {
        BuyUnitSlot.WariorAdded -= SpawnPlayerUnit;
    }

    private void Start()
    {
        triangulation = NavMesh.CalculateTriangulation();
        //SpawnPlayerUnits();
    }

    private void SpawnPlayerUnits()
    {
        for (int i = 0; i < initialUnits; i++)
        {
            SpawnPlayerUnit();
        }
    }


    public void SpawnPlayerUnit()
    {
        GameObject newUnit = Instantiate(playerUnitPrefab, _spawnPosition.transform.position, Quaternion.identity);
        newUnit.AddComponent<SimpleUnit>();

        // При спавне каждый юнит получает текущее состояние игры
        UnitStateMachine unitstateMachine = newUnit.GetComponent<UnitStateMachine>();
        if (unitstateMachine != null)
        {
            unitstateMachine.TransitionToState(unitstateMachine.CurrentState);
        }

        SpawnedUnits.Add(newUnit);

        Debug.Log("Юнит добавлен");
    }

}