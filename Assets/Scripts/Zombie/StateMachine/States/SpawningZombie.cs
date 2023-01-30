using UnityEngine.Events;

public class SpawningZombie : State
{
    public event UnityAction Spawned;

    public void EndSpawnEvent()
    {
        Spawned?.Invoke();
    }
}