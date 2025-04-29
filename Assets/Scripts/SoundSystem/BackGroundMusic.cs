using UnityEngine;
using System.Collections.Generic;
using YG;
using UnityEngine.Audio;
using Zenject;


public class BackGroundMusic : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        AudioClip[] music = Resources.LoadAll<AudioClip>("Music");
        _audioSource.clip = music[0];
        _audioSource.loop = true;

        DontDestroyOnLoad(this.gameObject);

        CheckSettings();

        // Предварительная загрузка всей музыки
        PlayMusic();
    }

    // Проверка настроек звука
    private void CheckSettings()
    {
        if (YG2.saves.MusicEnabled)
        {
            _audioSource.mute = false;
        }
        else
        {
            _audioSource.mute = true;
        }
    }

    public void PlayMusic()
    {
        if (YG2.saves.MusicEnabled)
        {
            _audioSource.Play();
        }
        else
        {
            Debug.Log("Музыка в игре выключена");
        }
    }

    public void ChangeSoundSetting()
    {
        // Переключаем значение MusicEnabled
        YG2.saves.MusicEnabled = !YG2.saves.MusicEnabled;
        Debug.Log("Звуки в игре " + YG2.saves.MusicEnabled);

        // Сохраняем прогресс
        YG2.SaveProgress();

        CheckSettings();
        PlayMusic();
    }
}