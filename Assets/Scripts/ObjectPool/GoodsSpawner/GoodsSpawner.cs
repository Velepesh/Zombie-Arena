using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GoodsSpawner : ObjectPool
{
    [SerializeField] private List<SpawnPoint> _spawnPoints;
    [SerializeField] private GrenadeGoods _grenadeGoodsPrefab;
    [SerializeField] private HealthGoods _healthGoodsPrefab;
    [SerializeField] private int _maxEnabledGoods;
    [SerializeField] private float _timeBetweenSpawn;

    private List<Goods> _goods = new List<Goods>();
    private int _currentEnableGoods => _goods.Count;
    private float _timeAfterLastSpawn;
    private bool _isRunning;
    private Health _twinsHealth;

    private void OnValidate()
    {
        _maxEnabledGoods = Mathf.Clamp(_maxEnabledGoods, 0, int.MaxValue);
        _timeBetweenSpawn = Mathf.Clamp(_timeBetweenSpawn, 0, float.MaxValue);
    }

    private void OnEnable()
    {
        AddUpdate();
    }

    private void OnDisable()
    {
        RemoveUpdate();
    }

    public override void OnTick()
    {
        if (_isRunning == false)
            return;

        _timeAfterLastSpawn += Time.deltaTime;

        if (_timeAfterLastSpawn >= _timeBetweenSpawn && _currentEnableGoods < _maxEnabledGoods)
        {
            if (IsSpawnPointEmpty() == false)
                return;

            if (TryGetRandomObject(out GameObject goodsObject))
            {
                Spawn(goodsObject);
                ResetTimeCounter();
            }
        }
    }

    public override void GeneratePrefabs()
    {
        for (int i = 0; i < _maxEnabledGoods; i++)
        {
            SpawnPrefab(_grenadeGoodsPrefab.gameObject);
            SpawnPrefab(_healthGoodsPrefab.gameObject);
        }
    }

    public void Init(Health twinsHealth)
    {
        if (twinsHealth == null)
            throw new ArgumentNullException(nameof(twinsHealth));

        _twinsHealth = twinsHealth;
    }

    public void Run()
    {
        GeneratePrefabs();
        _isRunning = true;
    }

    private void Spawn(GameObject goodsObject)
    {
        ISpawnPoint spawnPoint = GetSpawnPoint();

        while (spawnPoint.CanSpawn == false)
            spawnPoint = GetSpawnPoint();

        if (goodsObject.TryGetComponent(out Goods goods))
        {
            goodsObject.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            goodsObject.SetActive(true);
            goodsObject.transform.position = spawnPoint.Position;
            goods.Init(spawnPoint);

            if (goods is IHealthGoods healthGoods)
                healthGoods.SetTwinsHealth(_twinsHealth);

            _goods.Add(goods);
            goods.PickedUp += OnPickedUp;
        }
    }

    private void OnPickedUp(Goods goods)
    {
        if(_currentEnableGoods == _maxEnabledGoods)
            ResetTimeCounter();

        _goods.Remove(goods);
        goods.PickedUp -= OnPickedUp;
        ObjectEnabler.Disable(goods.gameObject);
    }

    private void ResetTimeCounter()
    {
        _timeAfterLastSpawn = 0;
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
}