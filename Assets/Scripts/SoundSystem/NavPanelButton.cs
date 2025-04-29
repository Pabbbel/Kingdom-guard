using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class NavPanelButton : MonoBehaviour
{
    private SoundSystem _soundSystem;

    [SerializeField] private Animator _animator;
    [SerializeField] private Button _openButton;
    [SerializeField] private Button _closeButton;

    private const string IN = "in";
    private const string OUT = "out";

    private bool _isOpen = false;

    [Inject]
    public void Construct(SoundSystem soundSystem)
    {
        _soundSystem = soundSystem;
    }

    private void Start()
    {
        _openButton.onClick.AddListener(OnOpenButtonClick);
        _closeButton.onClick.AddListener(OnCloseButtonClick);
    }

    private void OnOpenButtonClick()
    {
        // Проверяем, если панель уже открыта, ничего не делаем
        if (_isOpen) return;
        Debug.Log($"{_isOpen}");

        _soundSystem.PlaySound("Paper_1");

        _animator.SetTrigger(IN);
        _isOpen = true; 
    }

    private void OnCloseButtonClick()
    {
        // Проверяем, если панель уже закрыта, ничего не делаем
        if (!_isOpen) return;
        Debug.Log($"{_isOpen}");

        _soundSystem.PlaySound("woosh");

        _animator.SetTrigger(OUT);
        _isOpen = false;

    }

    private void SwitchTrigger()
    {
        _isOpen = !_isOpen;
        Debug.Log($"{_isOpen}");
    }

    public void SwithTriggerAnimator()
    {
        SwitchTrigger();
    }
}