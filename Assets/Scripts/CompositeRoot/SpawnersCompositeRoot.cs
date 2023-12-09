using UnityEngine;
using System;

public class SpawnersCompositeRoot : CompositeRoot
{
    [SerializeField] private SpawnersCreator _creator;
    [SerializeField] private ZombieSpawnerView _spawnerView;
    [SerializeField] private HitMarkers _hitMarkers;

    private ZombiesSpawner _zombiesSpawner;

    public event Action ZombiesEnded;
    public event Action<ZombiesSpawner> SpawnerInited;

    private void OnDisable()
    {
        if (_zombiesSpawner != null)
            _zombiesSpawner.Ended -= OnZombiesEnded;
    }

    public override void Compose()
    {
        _spawnerView.Init(_zombiesSpawner);
        _hitMarkers.Init(_zombiesSpawner);
        _creator.RunSpawners();
        _spawnerView.Enable();
    }

    public void Init(GameMode gameMode, int levelIndex, TargetsCompositeRoot targets, bool isMobilePlatform)
    {
        if (levelIndex <= 0)
            throw new ArgumentException(nameof(levelIndex));

        if (targets == null)
            throw new ArgumentException(nameof(targets));

        CreatwSpawners(gameMode, levelIndex, targets, isMobilePlatform);
    }

    private void CreatwSpawners(GameMode gameMode, int levelIndex, TargetsCompositeRoot targets, bool isMobilePlatform)
    {
        _zombiesSpawner = _creator.CreateZombieSpawner(gameMode, levelIndex, targets, isMobilePlatform);
        
        if(gameMode == GameMode.Infinite)
            _creator.CreateGoodsSpawner(targets.Twins.Health, _zombiesSpawner);
        
        _zombiesSpawner.Ended += OnZombiesEnded;
        SpawnerInited?.Invoke(_zombiesSpawner);
    }

    private void OnZombiesEnded()
    {
        ZombiesEnded?.Invoke();
    }
}