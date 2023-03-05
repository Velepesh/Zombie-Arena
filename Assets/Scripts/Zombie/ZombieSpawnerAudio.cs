using UnityEngine;

[RequireComponent(typeof(ZombieSpawner))]
[RequireComponent(typeof(AudioSource))]
public class ZombieSpawnerAudio : MonoBehaviour
{
    [SerializeField] private AudioClip _headKilledAudioClip;

    private AudioSource _audioSource;
    private ZombieSpawner _zombieSpawner;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
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
            _audioSource.PlayOneShot(_headKilledAudioClip);
    }
}
