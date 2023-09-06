using System.Collections;
using UnityEngine;

public class ZombieStatesAudio : Audio
{
    [SerializeField] private AudioClip _deadAudioClip;
    [SerializeField] private AudioClip _spawnAudioClip;

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
        PlayOneShot(_spawnAudioClip);
    }

    private void PlayWalkingAudio()
    {
        if (AudioSource.isPlaying == false)
            AudioSource.Play();
    }

    private IEnumerator StopAudioSource()
    {
        yield return new WaitWhile(() => AudioSource.isPlaying);
        AudioSource.Stop();
    }

    private void OnDied(IDamageable damageable)
    {
        PlayOneShot(_deadAudioClip);
    }
}
