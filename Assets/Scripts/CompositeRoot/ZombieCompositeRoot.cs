using UnityEngine;

public class ZombieCompositeRoot : CompositeRoot
{
    [SerializeField] private ZombieSpawner _zombieSpawner;

    public override void Compose()
    {
        _zombieSpawner.StartSpawn();
    }
}