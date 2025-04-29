using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

public class SoundButton : MonoBehaviour
{
    [SerializeField] private Image _iconPlaceHolder;
    [SerializeField] private Sprite _soundOn;
    [SerializeField] private Sprite _soundOff;

    private SoundSystem _soundSystem;

    [Inject]
    public void Construct(SoundSystem soundSystem)
    {
        _soundSystem = soundSystem;
    }

    private void Start()
    {
        _iconPlaceHolder.sprite = YG2.saves.SoundEnabled ? _soundOn : _soundOff;
    }
    public void ChangeSettings()
    {
        _soundSystem.ChangeSoundSetting();

        // Обновляем текст в зависимости от нового значения
        _iconPlaceHolder.sprite = YG2.saves.SoundEnabled ? _soundOn : _soundOff;
    }
}