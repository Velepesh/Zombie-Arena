using UnityEngine;

[RequireComponent(typeof(Zombie))]
public class SpawningZombie : State
{
    private Zombie _zombie;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
    }

    private void OnSpawnedEvent()
    {
        _zombie.EndSpawn();
    }
}