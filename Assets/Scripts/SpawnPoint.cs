using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private Zombie _zombie;

    public bool CanSpawn => _zombie == null;
    public Vector3 Position => transform.position;
    private void OnDisable()
    {
        if (_zombie != null)
            _zombie.Spawned -= OnSpawned;
    }

    public void Init(Zombie zombie)
    {
        _zombie = zombie;
        _zombie.Spawned += OnSpawned;
    }


    private void OnSpawned(Zombie zombie)
    {
        _zombie.Spawned -= OnSpawned;
        _zombie = null;
    }
}
