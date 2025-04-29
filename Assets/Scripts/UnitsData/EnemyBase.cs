using UnityEngine;

public class EnemyBase : UnitBase
{
    private void Start()
    {
        Initialize(
            maxHealth: 5,           // Может быть меньше, чем у юнита игрока
            attackDamage: 2,         // Урон врага
            attackRange: 1.5f,       // Немного больший радиус атаки
            attackSpeed: 0.8f        // Чуть медленнее атака
        );
        Type = UnitType.SimpleEnemy; // Установка типа

        //Debug.Log($"{GetType().Name} создан с {Health} здоровья.");
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage); // Вызов базовой логики
        Debug.Log($"{GetType().Name} получил {damage} урона. Осталось здоровья: {Health}.");
    }

    // Переопределение логики смерти врага
    protected override void Die()
    {
        Debug.Log($"Враг {GetType().Name} повержен!");
        base.Die();
    }
}