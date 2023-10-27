using UnityEngine;
using YG;

public class ZombieSpawnerCompositeRoot : CompositeRoot
{
    [SerializeField] private ZombieSpawner _zombieSpawner;

    public override void Compose()
    {
        _zombieSpawner.StartSpawn();

        if (YandexGame.SDKEnabled == true)
            _zombieSpawner.SetPlatform(YandexGame.EnvironmentData.isMobile);
    }
}