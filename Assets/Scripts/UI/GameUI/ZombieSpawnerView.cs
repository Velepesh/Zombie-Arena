using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ZombieSpawnerView : MonoBehaviour
{
    [SerializeField] private ZombieSpawner _zombieSpawner;
    [SerializeField] private TMP_Text _zombiesNumberText;
    [SerializeField] private TMP_Text _currentWaveText;
    [SerializeField] private TMP_Text _totalWavesText;

    private int _currentZombiesNumber;
    private int _waveNumber;

    public event UnityAction<int> WaveSetted;

    private void OnEnable()
    {
        _zombieSpawner.Loaded += OnLoaded;
        _zombieSpawner.WaveSetted += OnWaveSetted;
        _zombieSpawner.ZombieDied += OnZombieDied;
    }

    private void OnDisable()
    {
        _zombieSpawner.Loaded -= OnLoaded;
        _zombieSpawner.WaveSetted -= OnWaveSetted;
        _zombieSpawner.ZombieDied -= OnZombieDied;
    }

    private void OnLoaded()
    {
        UpdateText(_totalWavesText, _zombieSpawner.WavesCount);
    }

    private void OnWaveSetted(int index)
    {
        _waveNumber = index + 1;

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
        text.text = number.ToString();
    }
}