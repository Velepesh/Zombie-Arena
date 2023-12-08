using TMPro;
using UnityEngine;
using System;

public class ZombieSpawnerView : MonoBehaviour
{
    [SerializeField] private TMP_Text _zombiesNumberText;
    [SerializeField] private TMP_Text _currentWaveText;
    [SerializeField] private TMP_Text _totalWavesText;

    private readonly string _infinitySign = "∞";

    private IZombieSpawner _zombieSpawner;
    private int _currentZombiesNumber;
    private int _waveNumber;

    public event Action<int> WaveSetted;

    private void OnDisable()
    {
        if (_zombieSpawner != null)
        {
            _zombieSpawner.WaveSetted -= OnWaveSetted;
            _zombieSpawner.ZombieDied -= OnZombieDied;
        }
    }

    public void Init(IZombieSpawner zombieSpawner)
    {
        if(zombieSpawner == null)
            throw new ArgumentNullException(nameof(zombieSpawner));

        _zombieSpawner = zombieSpawner;

        _zombieSpawner.WaveSetted += OnWaveSetted;
        _zombieSpawner.ZombieDied += OnZombieDied;
    }

    public void Enable()
    {
        UpdateText(_totalWavesText, _zombieSpawner.WavesCount);
    }

    private void OnWaveSetted()
    {
        _waveNumber++;

        _currentZombiesNumber = _zombieSpawner.ZombiesNumberInWave;

        WaveSetted?.Invoke(_waveNumber);

        UpdateText(_zombiesNumberText, _currentZombiesNumber);
        UpdateText(_currentWaveText, _waveNumber);
    }

    private void OnZombieDied(Zombie zombie)
    {
        _currentZombiesNumber--;

        UpdateText(_zombiesNumberText, _currentZombiesNumber);
    }

    private void UpdateText(TMP_Text text, int number)
    {
        if(IsInfinity(number))
            text.text = _infinitySign;
        else
            text.text = number.ToString();
    }

    private bool IsInfinity(int value)
    {
        if (value == int.MaxValue)
            return true;

        return false;
    }
}