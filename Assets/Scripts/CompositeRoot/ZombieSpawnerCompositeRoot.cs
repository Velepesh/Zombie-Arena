using UnityEngine;
using UnityEngine.Events;
using YG;

public class ZombieSpawnerCompositeRoot : CompositeRoot
{
    [SerializeField] private WavesSpawner _zombieSpawner;
    [SerializeField] private ZombieSpawnerView _spawnerView;

    private LevelCounter _levelCounter;

    public WavesSpawner ZombieSpawner => _zombieSpawner;

    public event UnityAction Ended;

    private void OnEnable()
    {
        _zombieSpawner.Ended += OnEnded;
    }

    private void OnDisable()
    {
        _zombieSpawner.Ended -= OnEnded;
    }

    public void Init(LevelCounter levelCounter)
    {
        _levelCounter = levelCounter;
    }

    public override void Compose()
    {
        _zombieSpawner.StartSpawn(_levelCounter);
        _spawnerView.Init(_zombieSpawner);

        if (YandexGame.SDKEnabled == true)
        {
            bool isMobile = YandexGame.EnvironmentData.isDesktop == false;
            _zombieSpawner.SetPlatform(isMobile);
        }
    }

    private void OnEnded()
    {
        Ended?.Invoke();
    }
}