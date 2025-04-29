using System;
using UnityEngine;
using Zenject;

public class SimpleUnit : UnitBase
{
    public static event Action UnitDie;

    private PlayerUnitSpawner _playerUnitSpawner;

    [Inject]
    public void Construct(PlayerUnitSpawner playerUnitSpawner)
    {
        _playerUnitSpawner = playerUnitSpawner;
    }

    private void Start()
    {
        if (_playerUnitSpawner == null)
            _playerUnitSpawner = FindFirstObjectByType<PlayerUnitSpawner>();

        // Вызов базового метода для инициализации параметров
        Initialize(
            maxHealth: 10, 
            attackDamage: 1, 
            attackRange: 1f, 
            attackSpeed: 1f);

        //Debug.Log($"{GetType().Name} создан с {Health} здоровья.");
    }

    // Переопределение поведения при получении урона
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage); // Вызов базовой логики
        Debug.Log($"{GetType().Name} получил {damage} урона. Осталось здоровья: {Health}.");
    }

    // Добавляем свою логику смерти
    protected override void Die()
    {
        _playerUnitSpawner.SpawnedUnits.Remove(gameObject);
        UnitDie?.Invoke();
        Debug.Log($"{GetType().Name} умер героически!");
        base.Die();
    }
}
