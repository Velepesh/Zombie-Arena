using Plugins.Audio.Utils;
using UnityEngine;

[RequireComponent(typeof(IZombieSpawner))]
public class ZombieSpawnerAudio : Audio
{
    [SerializeField] private AudioDataProperty _headKilledAudioClip;

    private IZombieSpawner _zombieSpawner;

    private void Awake()
    {
        _zombieSpawner = GetComponent<IZombieSpawner>();
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
            SourceAudio.PlayOneShot(_headKilledAudioClip.Key);
    }
}
