using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;

public class ZombieSpawner : ObjectPool
{
    [SerializeField] private List<Zombie> _templates;
    [SerializeField] private List<Wave> _waves;
    [SerializeField] private List<SpawnPoint> _spawnPoints;
    [SerializeField] private bool _isSpawnWhenStart;

    private Wave _currentWave;
    private int _currentWaveNumber = 0;
    private float _timeAfterLastSpawn;
    private int _spawned;
    private List<Zombie> _zombies = new List<Zombie>();
    private bool _isAllEnemiesDied => _zombies.Count == 0;
    private ITarget _target;

    public event UnityAction HeadshotReceived;
    public event UnityAction BodyshotReceived;

    private void OnEnable() => AddUpdate();

    private void OnDisable()
    {
        RemoveUpdate();

        for (int i = 0; i < _zombies.Count; i++)
        {
            Zombie zombie = _zombies[i];

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

    public void StartSpawn(ITarget target)
    {
        if (target == null)
            throw new ArgumentNullException(nameof(target));

        _target = target;
        OnLevelStarted();
    }

    private void OnLevelStarted()
    {
        SetWave(_currentWaveNumber);

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
            zombieObject.transform.position = spawnPoint.Position;
            zombie.Init(_target);
            spawnPoint.Init(zombie);
            zombie.Died += OnDied;
            zombie.HitTaken += OnHitTaken;
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

        StartGenerate();
    }

    private void NextWave()
    {
        _zombies = new List<Zombie>();
        SetWave(++_currentWaveNumber);
        _spawned = 0;
    }

    private void OnDied(IDamageable damageable)
    {
        if (damageable is Zombie zombie)
        {
            zombie.Died -= OnDied;
            zombie.HitTaken -= OnHitTaken;
            _zombies.Remove(zombie);

            TrySpawnNexWave();
        }
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

    private void TrySpawnNexWave()
    {
        if (_isAllEnemiesDied)
        {
            if (_waves.Count > _currentWaveNumber + 1)
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