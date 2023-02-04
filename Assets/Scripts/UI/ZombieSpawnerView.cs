using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ZombieSpawnerView : MonoBehaviour
{
    [SerializeField] private ZombieSpawner _zombieSpawner;
    [SerializeField] private TMP_Text _waveNumberText;
    [SerializeField] private TMP_Text _zombiesNumberText;

    private int _currentZombiesNumber;

    public event UnityAction<int> WaveSetted;

    private void OnEnable()
    {
        _zombieSpawner.WaveSetted += OnWaveSetted;
        _zombieSpawner.ZombieDied += OnZombieDied;
    }

    private void OnDisable()
    {
        _zombieSpawner.WaveSetted -= OnWaveSetted;
        _zombieSpawner.ZombieDied -= OnZombieDied;
    }

    private void OnWaveSetted(int index)
    {
        _waveNumberText.text = index.ToString();
        _currentZombiesNumber = _zombieSpawner.ZombiesNumberInWave;

        WaveSetted?.Invoke(index);

        UpdateZombiesNumberView(_currentZombiesNumber);
    }

    private void OnZombieDied(Zombie zombie)
    {
        _currentZombiesNumber--;

        UpdateZombiesNumberView(_currentZombiesNumber);
    }

    private void UpdateZombiesNumberView(int zombiesNumber)
    {
        _zombiesNumberText.text = zombiesNumber.ToString();
    }
}