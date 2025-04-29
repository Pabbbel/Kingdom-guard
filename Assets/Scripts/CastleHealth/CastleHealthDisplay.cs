using UnityEngine;
using TMPro;
using Zenject;

public class CastleHealthDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _displayText;

    private CastleHealthManager _castleHealthManager;

    [Inject]
    public void Construct(CastleHealthManager castleHealthManager)
    {
        _castleHealthManager = castleHealthManager;
    }

    private void Start()
    {
        // Подписываемся на событие при старте
        _castleHealthManager.onHealthChange += UpdateHealthDisplay;

        // Инициализируем текущее значение здоровья при старте
        UpdateHealthDisplay(_castleHealthManager.GetCastleHealth());
    }

    private void UpdateHealthDisplay(int newAmount)
    {
        _displayText.text = newAmount.ToString();
    }

    private void OnDestroy()
    {
        // Важно отписаться от события, чтобы избежать утечек памяти
        _castleHealthManager.onHealthChange -= UpdateHealthDisplay;
    }
}
