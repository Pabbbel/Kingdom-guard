using UnityEngine;
using Zenject;

public enum UnitType
{
    SimpleUnit,
    StrongUnit,
    SimpleEnemy,
    BossEnemy,
    SpecialEnemy
}

public class UnitBase : MonoBehaviour
{
    public UnitType Type { get; protected set; }
    public int Health { get; protected set; }
    public int MaxHealth { get; protected set; }
    public int AttackDamage { get; protected set; }
    public float AttackRange { get; protected set; }
    public float AttackSpeed { get; protected set; }

    

    // Конструктор для инициализации
    protected virtual void Initialize(int maxHealth, int attackDamage, float attackRange, float attackSpeed)
    {
        MaxHealth = maxHealth;
        Health = maxHealth;
        AttackDamage = attackDamage;
        AttackRange = attackRange;
        AttackSpeed = attackSpeed;
    }

    // Метод для получения урона
    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
        Health = Mathf.Clamp(Health, 0, MaxHealth);

        if (Health <= 0)
        {
            Die();
        }
    }

    // Метод для смерти юнита
    protected virtual void Die()
    {
        Debug.Log($"{Type} погиб.");
        Destroy(gameObject);
    }
}
