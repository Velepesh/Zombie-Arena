using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Zombie))]
public class SpawningZombie : State
{
    private Zombie _zombie;

    public event UnityAction Spawned;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
    }

    private void OnSpawnedEvent()
    {
        _zombie.EndSpawn();
    }
}