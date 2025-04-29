using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class WaveSystem : MonoBehaviour
{
    private int _currentWave = 1;
    private const int MAX_WAVES = 99;

    //[SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _timerText;

    private float _timeToWave = 150f;

    private int CalculateEnemyCount()
    {
        int countToAdd = UnityEngine.Random.Range(1, 2);
        // Логика расчета количества врагов
        return Mathf.Clamp(_currentWave + countToAdd, 1, 40);
    }

    public int EnemyWaveCount => CalculateEnemyCount();

    private GameStateSystem _gameStateSystem;
    private SoundSystem _soundSystem;

    [Inject]
    public void Construct(GameStateSystem gameStateSystem, SoundSystem soundSystem)
    {
        _gameStateSystem = gameStateSystem;
        _soundSystem = soundSystem;
    }

    private void Start()
    {
        StartCoroutine(WaveTimer(_timeToWave));
    }

    private void AddWaveCount()
    {
        _currentWave++;
    }

    public void AddWave()
    {
        AddWaveCount();
    }

    private IEnumerator WaveTimer(float time)
    {
        //_slider.maxValue = time;
        //_slider.value = time;
        float remainingTime = time;

        while (remainingTime > 0)
        {
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
            //_slider.value -= 1f;

            // Конвертируем оставшееся время в минуты и секунды
            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);

            _timerText.text = $"Волна {_currentWave} через {minutes:00}:{seconds:00}";
            //Debug.Log($"Осталось времени: {minutes:00}:{seconds:00}");
        }

        Debug.Log("Таймер завершился, волна пошла");
        StartWawe();
    }

    private void StartWawe()
    {
        _soundSystem.PlaySound("Horn");
        _gameStateSystem.ChangeState(GameState.EnemyWave);
        StopAllCoroutines();
        StartCoroutine(WaveTimer(_timeToWave));
    }
}
