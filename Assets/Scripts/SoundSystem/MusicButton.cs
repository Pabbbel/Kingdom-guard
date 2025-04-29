using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;


public class MusicButton : MonoBehaviour
{
    [SerializeField] private Image _iconPlaceHolder;
    [SerializeField] private Sprite _soundOn;
    [SerializeField] private Sprite _soundOff;

    private BackGroundMusic _backGroundMusic;

    [Inject]
    public void Construct(BackGroundMusic backGroundMusic)
    {
        _backGroundMusic = backGroundMusic;
    }
    private void Start()
    {
        _iconPlaceHolder.sprite = YG2.saves.MusicEnabled ? _soundOn : _soundOff;
    }
    public void ChangeSettings()
    {
        _backGroundMusic.ChangeSoundSetting();

        // Обновляем текст в зависимости от нового значения
        _iconPlaceHolder.sprite = YG2.saves.MusicEnabled ? _soundOn : _soundOff;
    }
}