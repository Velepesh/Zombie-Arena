using System;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
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

    private void Update()
    {
        if (_currentWave == null)
            return;

        _timeAfterLastSpawn += Time.deltaTime;

        if (_timeAfterLastSpawn >= _currentWave.Delay)
        {
            Spawn();
            _timeAfterLastSpawn = 0;
        }

        if (_currentWave.Count <= _spawned)
            _currentWave = null;
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


    private void Spawn()
    {
        if (CanSpawn() == false)
            return;

        SpawnPoint spawnPoint = GetSpawnPoint();

        while (spawnPoint.CanSpawn == false)
            spawnPoint = GetSpawnPoint();

        GameObject template = _currentWave.Templates[_spawned];

        if (template.TryGetComponent(out Zombie zombie))
        {
            zombie = Instantiate(template, spawnPoint.Position, template.transform.rotation).GetComponent<Zombie>();
            zombie.Init(_target);
            spawnPoint.Init(zombie);
            zombie.Died += OnDied;
            _zombies.Add(zombie);
        }

        _spawned++;
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
            _zombies.Remove(zombie);

            TrySpawnNexWave();
        }
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
    public List<GameObject> Templates;
    public float Delay;
    public int Count => Templates.Count;

    public void RemoveTemplate(GameObject template)
    {
        Templates.Remove(template);
    }
}