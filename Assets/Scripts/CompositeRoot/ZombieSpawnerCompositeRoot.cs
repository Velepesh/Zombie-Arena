using UnityEngine;
using System;

public class ZombieSpawnerCompositeRoot : CompositeRoot
{
    [SerializeField] private ZombieSpawnersCreator _creator;
    [SerializeField] private ZombieSpawnerView _spawnerView;
    [SerializeField] private HitMarkers _hitMarkers;

    private ZombiesSpawner _zombiesSpawner;

    public event Action ZombiesEnded;
    public event Action<ZombiesSpawner> SpawnerInited;

    private void OnDisable()
    {
        if(_zombiesSpawner != null)
            _zombiesSpawner.Ended -= OnZombiesEnded;
    }

    public override void Compose()
    {
        _spawnerView.Init(_zombiesSpawner);
        _hitMarkers.Init(_zombiesSpawner);
        _zombiesSpawner.StartSpawn();
        _spawnerView.Enable();
    }

    public void Init(GameMode gameMode, int levelIndex, ZombieTargetsCompositeRoot targets, bool isMobilePlatform)
    {
        if (levelIndex <= 0)
            throw new ArgumentException(nameof(levelIndex));

        if (targets == null)
            throw new ArgumentException(nameof(targets));

        _zombiesSpawner = _creator.CreateSpawner(gameMode, levelIndex, targets, isMobilePlatform);
        _zombiesSpawner.Ended += OnZombiesEnded;

        SpawnerInited?.Invoke(_zombiesSpawner);
    }

    private void OnZombiesEnded()
    {
        ZombiesEnded?.Invoke();
    }
}