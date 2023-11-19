using UnityEngine;
using UnityEngine.Events;
using YG;

public class ZombieSpawnerCompositeRoot : CompositeRoot
{
    [SerializeField] private ZombieSpawner _zombieSpawner;

    public ZombieSpawner ZombieSpawner => _zombieSpawner;

    public event UnityAction Ended;

    private void OnEnable()
    {
        _zombieSpawner.Ended += OnEnded;
    }

    private void OnDisable()
    {
        _zombieSpawner.Ended -= OnEnded;
    }

    public override void Compose()
    {
        _zombieSpawner.StartSpawn();

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