using Plugins.Audio.Core;
using Plugins.Audio.Utils;
using UnityEngine;

public class ZombieStatesAudio : Audio
{
    [SerializeField] private AudioDataProperty _deadAudioClip;
    [SerializeField] private AudioDataProperty _spawnAudioClip;

    private Zombie _zombie;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
    }

    private void OnEnable()
    {
        _zombie.Died += OnDied;
    }

    private void OnDisable()
    {
        _zombie.Died -= OnDied;
    }

    private void Start()
    {
        SourceAudio.PlayOneShot(_spawnAudioClip.Key);
    }

    private void OnDied(IDamageable damageable)
    {
        SourceAudio.PlayOneShot(_deadAudioClip.Key);
    }
}
