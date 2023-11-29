using UnityEngine;
using UnityEngine.Events;

public class ZombieSpawnerCompositeRoot : CompositeRoot
{
    [SerializeField] private ZombieSpawnersCreator _creator;
    [SerializeField] private ZombieSpawnerView _spawnerView;
    [SerializeField] private HitMarkers _hitMarkers;

    private ZombiesSpawner _zombieSpawner;
    private LevelCounter _levelCounter;
    private ZombieTargetsCompositeRoot _targets;
    private bool _isMobilePlatform;

    public event UnityAction Ended;

    private void OnDisable()
    {
        if(_zombieSpawner != null)
            _zombieSpawner.Ended -= OnEnded;
    }

    public override void Compose()
    {
        _spawnerView.Init(_zombieSpawner);
        _hitMarkers.Init(_zombieSpawner);
        _zombieSpawner.StartSpawn();
        _spawnerView.Enable();
    }

    public void Init(LevelCounter levelCounter, ZombieTargetsCompositeRoot targets, bool isMobilePlatform)
    {
        _levelCounter = levelCounter;
        _targets = targets;
        _isMobilePlatform = isMobilePlatform;
    }

    public ZombiesSpawner InitSpawner(GameMode gameMode)
    {
        _zombieSpawner = _creator.CreateSpawner(gameMode, _levelCounter, _targets, _isMobilePlatform);
        _zombieSpawner.Ended += OnEnded;

        return _zombieSpawner;
    }

    private void OnEnded()
    {
        Ended?.Invoke();
    }
}