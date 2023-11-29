using UnityEngine;

public interface ISpawnPoint
{
    bool CanSpawn { get; }
    Vector3 Position { get; }
    void Init(Zombie zombie);
}