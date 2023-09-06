using UnityEngine;

[RequireComponent(typeof(ZombieSpawner))]
public class ZombieSpawnerAudio : Audio
{
    [SerializeField] private AudioClip _headKilledAudioClip;

    private ZombieSpawner _zombieSpawner;

    private void Awake()
    {
        _zombieSpawner = GetComponent<ZombieSpawner>();
    }

    private void OnEnable()
    {
        _zombieSpawner.ZombieDied += OnZombieDied;
    }

    private void OnDisable()
    {
        _zombieSpawner.ZombieDied -= OnZombieDied;
    }

    private void OnZombieDied(Zombie zombie)
    {
        if (zombie.IsHeadKill)
            PlayOneShot(_headKilledAudioClip);
    }
}
