using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class ZombieSpawner : ObjectPool
{
    [SerializeField] private List<Level> _levels;
    [SerializeField] private LevelCounter _levelCounter;
    [SerializeField] private ZombieTargetsCompositeRoot _targets;
    [SerializeField] private List<SpawnPoint> _spawnPoints;
    [SerializeField] private int _numberOfCircleLevel;
    [SerializeField] private int _maxActiveZombie;

    readonly private int _startWaveIndex = 0;

    private Level _currentLevel;
    private Wave _currentWave;
    private float _timeAfterLastSpawn;
    private List<Zombie> _zombies = new List<Zombie>();
    private int _currentAliveZombieInWave;
    private bool _isAllEnemiesDied => _currentAliveZombieInWave == 0;
    private int _currentWaveNumber = 0;

    public int WavesCount => _currentLevel.WavesCount;
    public int CurrentWave => _currentWaveNumber + 1;
    public int ZombiesNumberInWave => _currentWave.Count;

    public event UnityAction Ended;
    public event UnityAction HeadshotReceived;
    public event UnityAction BodyshotReceived;
    public event UnityAction<Zombie> ZombieDied;
    public event UnityAction<int> WaveSetted;
    public event UnityAction Loaded;

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

    public override void GeneratePrefabs()
    {
        for (int i = 0; i < _currentWave.Count; i++)
            SpawnPrefab(_currentWave.GetTemplate(i).gameObject);
    }

    public void StartSpawn()
    {
        if (_targets == null)
            throw new ArgumentNullException(nameof(_targets));

        LoadLevel();
        SetWave(_currentWaveNumber);

        _timeAfterLastSpawn = _currentWave.Delay - 1;
    }

    private void LoadLevel()
    {
        int levelIndex = _levelCounter.Level - 1;

        if (levelIndex > _numberOfCircleLevel)
            levelIndex = _numberOfCircleLevel;
        
        _currentLevel = _levels[levelIndex];
        _currentWaveNumber = _startWaveIndex;

        if (_currentLevel == null)
            throw new ArgumentNullException(nameof(_currentLevel));

        Loaded?.Invoke();
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
        _currentWave = _currentLevel.GetWave(index);
        _currentAliveZombieInWave = _currentWave.Count;
        GeneratePrefabs();
        WaveSetted?.Invoke(_currentWaveNumber);
    }

    private void NextWave()
    {
        _zombies = new List<Zombie>();

        ++_currentWaveNumber;

        SetWave(_currentWaveNumber);
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
        {
            if (_currentWaveNumber + 1 >= _currentLevel.WavesCount)
                Ended?.Invoke();
            else
                NextWave();
        }
    }
}