using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class ZombiesSpawner : ObjectPool, IZombieSpawner
{
    readonly private int _startWaveIndex = 0;
    readonly private int _endWaveZombiesCount = 3;

    private IReadOnlyList<ISpawnPoint> _spawnPoints;
    private TargetsCompositeRoot _targets;
    private bool _isMobilePlatform;
    private Level _currentLevel;
    private Wave _currentWave;
    private float _timeAfterLastSpawn;
    private List<Zombie> _zombies = new List<Zombie>();
    private int _currentAliveZombieInWave;
    private bool _isAllEnemiesDied => _currentAliveZombieInWave == 0;
    private int _currentWaveNumber = 0;
    private IWaveSetter _waveSetter;
    private int _wavesCount;

    public bool IsWaveEnding => _currentAliveZombieInWave <= _endWaveZombiesCount;
    public int WavesCount => _wavesCount;
    public int ZombiesNumberInWave => _currentWave.Count;

    public event Action Ended;
    public event Action HeadshotReceived;
    public event Action BodyshotReceived;
    public event Action<Zombie> ZombieDied;
    public event Action WaveSetted;
    public event Action Loaded;

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

        if (_timeAfterLastSpawn >= _currentWave.Delay && _zombies.Count < _currentWave.GetMaxActiveZombie(_isMobilePlatform))
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

    public void Init(IReadOnlyList<ISpawnPoint> spawnPoints, Level level, TargetsCompositeRoot targets, bool isMobilePlatform, IWaveSetter waveSetter)
    {
        if (spawnPoints.Count == 0)
            throw new ArgumentNullException(nameof(spawnPoints));

        if (level == null || level is ILevel == false)
            throw new ArgumentNullException(nameof(level));

        if (targets == null)
            throw new ArgumentNullException(nameof(targets));

        if (waveSetter == null)
            throw new ArgumentNullException(nameof(waveSetter));

        _spawnPoints = spawnPoints;
        _currentLevel = level;
        _targets = targets;
        _isMobilePlatform = isMobilePlatform;
        _waveSetter = waveSetter;

        if (level is ILevel iLevel)
            _wavesCount = level.GetWavesCount(iLevel);
    }

    public void Run()
    {
        LoadLevel();
        SetWave(_currentWaveNumber);

        _timeAfterLastSpawn = _currentWave.Delay - 1;
    }

    private void LoadLevel()
    {
        _currentWaveNumber = _startWaveIndex;
        Loaded?.Invoke();
    }

    private void Spawn(GameObject zombieObject)
    {
        ISpawnPoint spawnPoint = GetSpawnPoint();

        while (spawnPoint.CanSpawn == false)
            spawnPoint = GetSpawnPoint();

        if (zombieObject.TryGetComponent(out Zombie zombie))
        {
            zombieObject.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            ObjectEnabler.Enable(zombieObject);
            zombieObject.transform.position = spawnPoint.Position;
            zombie.Init(_targets, _isMobilePlatform, spawnPoint);
            spawnPoint.TakePoint();
            zombie.Died += OnDied;
            zombie.Disabled += OnDisabled;
            zombie.HitTaken += OnHitTaken;
            _zombies.Add(zombie);
        }
    }

    private ISpawnPoint GetSpawnPoint()
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
        WaveSetted?.Invoke();
    }

    private void NextWave()
    {
        _zombies = new List<Zombie>();

        _currentWaveNumber = _waveSetter.GetNextWaveIndex(_currentWaveNumber);

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
            bool isNextWaveExist = _waveSetter.CheckForNextWave(_currentWaveNumber, WavesCount);

            if (isNextWaveExist)
                NextWave();
            else
                Ended?.Invoke();
        }
    }
}
