using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZombieSpawner : ObjectPool
{
    [SerializeField] private List<Zombie> _templates;
    [SerializeField] private List<Wave> _waves;
    [SerializeField] private List<SpawnPoint> _spawnPoints;
    [SerializeField] private bool _isSpawnWhenStart;

    private Wave _currentWave;
    private float _timeAfterLastSpawn;
    private int _spawned;
    private List<Zombie> _zombies = new List<Zombie>();
    private bool _isAllEnemiesDied => _zombies.Count == 0;
    private ZombieTargets _targets;

    public int CurrentWaveNumber { get; private set; }
    public int ZombiesNumberInWave => _currentWave.Count;

    public event UnityAction HeadshotReceived;
    public event UnityAction BodyshotReceived;
    public event UnityAction<Zombie> ZombieDied;
    public event UnityAction<int> WaveSetted;

    private void OnEnable() => AddUpdate();

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

        if (_timeAfterLastSpawn >= _currentWave.Delay)
        {
            if (CanSpawn() == false)
                return;

            if (TryGetObject(out GameObject zombieObject, _currentWave.Templates[_spawned].Type))
            {
                Spawn(zombieObject);
                _timeAfterLastSpawn = 0;
            }
        }

        if (_currentWave.Count <= _spawned)
            _currentWave = null;
    }

    public override void StartGenerate()
    {
        for (int i = 0; i < _templates.Count; i++)
            Init(_templates[i].gameObject);
    }

    public void StartSpawn(ZombieTargets targets)
    {
        if (targets == null)
            throw new ArgumentNullException(nameof(targets));

        _targets = targets;

        OnLevelStarted();
    }

    private void OnLevelStarted()
    {
        StartGenerate();
        SetWave(CurrentWaveNumber);

        if(_isSpawnWhenStart)
            _timeAfterLastSpawn = _currentWave.Delay - 1;
    }

    private void StopSpawn()
    {
        _currentWave = null;
    }

    private void Spawn(GameObject zombieObject)
    {
        SpawnPoint spawnPoint = GetSpawnPoint();

        while (spawnPoint.CanSpawn == false)
            spawnPoint = GetSpawnPoint();

        if (zombieObject.TryGetComponent(out Zombie zombie))
        {
            zombieObject.SetActive(true);
            Debug.Log(zombieObject.name);//////////////////////////////////
            zombieObject.transform.position = spawnPoint.Position;
            spawnPoint.Init(zombie);
            zombie.Init(_targets);
            zombie.Died += OnDied;
            zombie.Disabled += OnDisabled;
            zombie.HitTaken += OnHitTaken;
            zombie.Spawn();
            _zombies.Add(zombie);

            _spawned++;
        }
    }

    private SpawnPoint GetSpawnPoint()
    {
        return _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count)];
    }

    private bool CanSpawn()
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

        WaveSetted?.Invoke(index + 1);
    }

    private void NextWave()
    {
        _zombies = new List<Zombie>();
        SetWave(++CurrentWaveNumber);
        _spawned = 0;
    }

    private void OnDied(IDamageable damageable)
    {
        if (damageable is Zombie zombie)
        {
            ZombieDied?.Invoke(zombie);
            zombie.Died -= OnDied;
            zombie.HitTaken -= OnHitTaken;
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

    private void OnPlayerDied(IDamageable damageable)
    {
        StopSpawn();
        damageable.Died -= OnPlayerDied;  
    }

    private void TrySpawnNextWave()
    {
        if (_isAllEnemiesDied)
        {
            if (_waves.Count > CurrentWaveNumber + 1)
                NextWave();
        }
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