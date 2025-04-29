using UnityEngine;
using System.Collections.Generic;
using YG;

public class SoundSystem : MonoBehaviour
{
    // Словарь для хранения всех звуков
    public Dictionary<string, AudioClip> SoundLibrary = new Dictionary<string, AudioClip>();

    // Компонент для проигрывания звуков
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();

        // Предварительная загрузка всех звуков
        LoadSounds();
    }

    public void LoadSounds()
    {
        // Загрузка звуков из папки Resources/Sounds
        AudioClip[] sounds = Resources.LoadAll<AudioClip>("Sounds");

        foreach (AudioClip sound in sounds)
        {
            // Добавление в словарь по имени клипа
            SoundLibrary[sound.name] = sound;
        }
    }

    public void PlaySound(string soundName)
    {
        // Проверка включения звука в настройках
        if (YG2.saves.SoundEnabled)
        {
            // Проверка наличия звука в словаре
            if (SoundLibrary.ContainsKey(soundName))
            {
                _audioSource.PlayOneShot(SoundLibrary[soundName]);
            }
            else
            {
                Debug.LogWarning($"Звук {soundName} не найден в библиотеке!");
            }
        }
        else
        {
            Debug.Log("Звуки в игре выключены");
        }
    }

    public void ChangeSoundSetting()
    {
        // ����������� �������� SoundEnabled
        YG2.saves.SoundEnabled = !YG2.saves.SoundEnabled;
        Debug.Log("Звуки в игре " + YG2.saves.SoundEnabled);

        // ��������� ��������
        YG2.SaveProgress();
    }
}