using UnityEngine;

public class SpawnPoint : MonoBehaviour, ISpawnPoint
{
    private bool _canSpawn = true;

    public bool CanSpawn => _canSpawn;
    public Vector3 Position => transform.position;

    public void TakePoint()
    {
        _canSpawn = false;
    }

    public void ResetPoint()
    {
        _canSpawn = true;
    }
}