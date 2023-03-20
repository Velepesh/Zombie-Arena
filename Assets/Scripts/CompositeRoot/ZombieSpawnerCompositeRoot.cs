using UnityEngine;

public class ZombieSpawnerCompositeRoot : CompositeRoot
{
    [SerializeField] private ZombieSpawner _zombieSpawner;

    public override void Compose()
    {
        _zombieSpawner.StartSpawn();
    }
}