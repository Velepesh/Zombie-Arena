using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ZombieSpawnerView : MonoBehaviour
{
    [SerializeField] private TMP_Text _zombiesNumberText;
    [SerializeField] private TMP_Text _currentWaveText;
    [SerializeField] private TMP_Text _totalWavesText;

    private WavesSpawner _zombieSpawner;
    private int _currentZombiesNumber;
    private int _waveNumber;

    public event UnityAction<int> WaveSetted;

    private void OnDisable()
    {
        if (_zombieSpawner != null)
        {
            _zombieSpawner.WaveSetted -= OnWaveSetted;
            _zombieSpawner.ZombieDied -= OnZombieDied;
        }
    }

    public void Init(WavesSpawner wavesSpawner)
    {
        _zombieSpawner = wavesSpawner;

        _zombieSpawner.WaveSetted += OnWaveSetted;
        _zombieSpawner.ZombieDied += OnZombieDied;

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