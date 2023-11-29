using UnityEngine;

public class SpawnPoint : MonoBehaviour, ISpawnPoint
{
    private Zombie _zombie;

    public bool CanSpawn => _zombie == null;
    public Vector3 Position => transform.position;

    private void OnDisable()
    {
        if (_zombie != null)
        {
            _zombie.Spawned -= OnSpawned;
            _zombie.Died -= OnDied;
        }
    }

    public void Init(Zombie zombie)
    {
        _zombie = zombie;
        _zombie.Spawned += OnSpawned;
        _zombie.Died += OnDied;
    }


    private void OnSpawned(Zombie zombie)
    {
        if (_zombie != null)
        {
            _zombie.Spawned -= OnSpawned;
            ResetZombie();
        }
    }

    private void OnDied(IDamageable damageable)
    {
        if (_zombie != null)
        {
            _zombie.Died -= OnDied;
            ResetZombie();
        }
    }

    private void ResetZombie()
    {
        _zombie = null;
    }
}
