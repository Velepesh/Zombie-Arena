using UnityEngine;
using UnityEngine.Events;

public class SpawningZombie : State
{
    [SerializeField] private Vector3 _spawnEffectOffset;
    [SerializeField] private ParticleSystem _spawnEffect;

    public event UnityAction Spawned;

    private void Start()
    {
        ShowEffect();
    }

    public void EndSpawnEvent()
    {
        Spawned?.Invoke();
    }

    private void ShowEffect()
    {
        Instantiate(_spawnEffect.gameObject, transform.position + _spawnEffectOffset, Quaternion.identity);
    }
}