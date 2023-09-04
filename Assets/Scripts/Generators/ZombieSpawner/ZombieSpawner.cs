using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class ZombieSpawner : ObjectPool
{
    [SerializeField] private ZombieTargetsCompositeRoot _targets;
    [SerializeField] private List<Wave> _waves;
    [SerializeField] private List<SpawnPoint> _spawnPoints;
    [SerializeField] private int _numberOfCircleWave;
    [SerializeField] private int _maxActiveZombie;

    private Wave _currentWave;
    private float _timeAfterLastSpawn;
    private List<Zombie> _zombies = new List<Zombie>();
    private int _currentAliveZombieInWave;
    private bool _isAllEnemiesDied => _currentAliveZombieInWave == 0;

    public int CurrentWaveNumber { get; private set; }
    public int ZombiesNumberInWave => _currentWave.Count;

    public event UnityAction HeadshotReceived;
    public event UnityAction BodyshotReceived;
    public event UnityAction<Zombie> ZombieDied;
    public event UnityAction<int> WaveSetted;

    private void OnEnable()
    {
        AddUpdate();
    }

    private void OnDisable()
    {
        RemoveUpdate();

        for (int i = 0; i < _zombies.Count; i++)
        {
            Zombie zombie = _zombies[i];

            zombie.Disabled -= OnDisabled;
            zombie.Died -= OnDied;
            zombie.HitTaken -= OnHitTaken;
        }
    }

    public override void OnTick()
    {
        if (_currentWave == null)
            return;

        _timeAfterLastSpawn += Time.deltaTime;

        if (_timeAfterLastSpawn >= _currentWave.Delay && _zombies.Count < _maxActiveZombie)
        {
            if (IsSpawnPointEmpty() == false)
                return;

            if (TryGetObject(out GameObject zombieObject))
            {
                Spawn(zombieObject);
                _timeAfterLastSpawn = 0;
            }
        }
    }

    public override void StartGenerate()
    {
        for (int i = 0; i < _currentWave.Count; i++)
            SpawnPrefab(_currentWave.Templates[i].gameObject);
    }

    public void StartSpawn()
    {
        if (_targets == null)
            throw new ArgumentNullException(nameof(_targets));

        SetWave(CurrentWaveNumber);

        _timeAfterLastSpawn = _currentWave.Delay - 1;
    }

    private void Spawn(GameObject zombieObject)
    {
        SpawnPoint spawnPoint = GetSpawnPoint();

        while (spawnPoint.CanSpawn == false)
            spawnPoint = GetSpawnPoint();

        if (zombieObject.TryGetComponent(out Zombie zombie))
        {
            zombieObject.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            zombieObject.SetActive(true);
            zombieObject.transform.position = spawnPoint.Position;
            spawnPoint.Init(zombie);
            zombie.Init(_targets);
            zombie.Died += OnDied;
            zombie.Disabled += OnDisabled;
            zombie.HitTaken += OnHitTaken;
            _zombies.Add(zombie);
        }
    }

    private SpawnPoint GetSpawnPoint()
    {
        return _spawnPoints[Random.Range(0, _spawnPoints.Count)];
    }

    private bool IsSpawnPointEmpty()
    {
        for (int i = 0; i < _spawnPoints.Count; i++)
        {
            if (_spawnPoints[i].CanSpawn)
                return true;
        }

        return false;
    }

    private void SetWave(int index)
    {
        _currentWave = _waves[index];
        _currentAliveZombieInWave = _currentWave.Count;
        StartGenerate();

        WaveSetted?.Invoke(CurrentWaveNumber + 1);
    }

    private void NextWave()
    {
        _zombies = new List<Zombie>();

        int nextWaveIndex = 0;
        ++CurrentWaveNumber;

        if (CurrentWaveNumber >= _numberOfCircleWave)
            nextWaveIndex = _numberOfCircleWave;
        else
            nextWaveIndex = CurrentWaveNumber;

        SetWave(nextWaveIndex);
    }

    private void OnDied(IDamageable damageable)
    {
        if (damageable is Zombie zombie)
        {
            ZombieDied?.Invoke(zombie);
            zombie.Died -= OnDied;
            zombie.HitTaken -= OnHitTaken;
            _currentAliveZombieInWave--;
            _zombies.Remove(zombie);

            TrySpawnNextWave();
        }
    }

    private void OnDisabled(Zombie zombie)
    {
        DisableObject(zombie.gameObject);
        zombie.Disabled -= OnDisabled;
    }

    private void OnHitTaken(DamageHandlerType type)
    {
        if (type == DamageHandlerType.Head)
            HeadshotReceived?.Invoke();
        else
            BodyshotReceived?.Invoke();
    }

    private void TrySpawnNextWave()
    {
        if (_isAllEnemiesDied)
            NextWave();
    }
}

[System.Serializable]
public class Wave
{
    public List<Zombie> Templates;
    public float Delay;
    public int Count => Templates.Count;

    public void RemoveTemplate(Zombie template)
    {
        Templates.Remove(template);
    }
}