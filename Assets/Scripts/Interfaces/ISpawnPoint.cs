using UnityEngine;

public interface ISpawnPoint
{
    bool CanSpawn { get; }
    Vector3 Position { get; }
    void TakePoint();
    void ResetPoint();
}