using System.Resources;
using UnityEngine;
using Zenject;
using static ResourceManager;

public class CastleHealthManager : MonoBehaviour
{

    private int _castleHealth;

    public delegate void OnHealthChange(int health);
    public event OnHealthChange onHealthChange;

    private void Start()
    {
        InitializeHealth();
    }

    // Инициализация здоровья
    private void InitializeHealth()
    {
        _castleHealth = 100;
    }

    // Получение текущего количества здоровья
    public int GetCastleHealth() => _castleHealth;

    // Добавление здоровья
    public void AddHealth(int health)
    {
        _castleHealth = Mathf.Clamp(_castleHealth + health, 0, 100);

        if (_castleHealth == 100)
        {
            OnCastleRepaired();
        }

        TriggerHealthChanged(_castleHealth);
    }

    public void RemoveHealth(int damage)
    {
        _castleHealth -= damage;

        if (_castleHealth <= 0)
        {
            _castleHealth = 0; // Не допускаем отрицательное здоровье
            OnCastleDestroyed();
        }

        TriggerHealthChanged(_castleHealth);
    }

    public void OnCastleDestroyed()
    {
        Debug.Log("Замок разрушен!");
    }

    public void OnCastleRepaired()
    {
        Debug.Log("Замок полностью восстановлен!");
    }

    private void TriggerHealthChanged(int health)
    {
        Debug.Log($"Новое количество здоровья: {health}");
        onHealthChange?.Invoke(health);
    }
}
