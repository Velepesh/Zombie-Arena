using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Zombie))]
[RequireComponent(typeof(ZombieMover))]
[RequireComponent(typeof(ZombieAttacker))]
public class ZombieWalkingAudio : Audio
{
    [SerializeField] private AudioClip _audioClipWalkiing;

    private Zombie _zombie;
    private ZombieMover _mover;
    private ZombieAttacker _attacker;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
        _mover = GetComponent<ZombieMover>();
        _attacker = GetComponent<ZombieAttacker>();

        AudioSource.clip = _audioClipWalkiing;
        AudioSource.loop = true;
    }

    private void OnEnable()
    {
        _zombie.Died += OnDied;
        _mover.Moved += OnMoved;
        _attacker.Attacked += OnAttacked;
    }

    private void OnDisable()
    {
        _zombie.Died -= OnDied;
        _mover.Moved -= OnMoved;
        _attacker.Attacked -= OnAttacked;
    }

    private void OnMoved()
    {
        PlayWalkingAudio();
    }

    private void OnAttacked()
    {
        StopWalkingAudio();
    }

    private void PlayWalkingAudio()
    {
        if (AudioSource.isPlaying == false)
            AudioSource.Play();
    }

    private void StopWalkingAudio()
    {
        if (AudioSource.isPlaying)
            StartCoroutine(StopAudioSource());
    }

    private IEnumerator StopAudioSource()
    {
        yield return new WaitWhile(() => AudioSource.isPlaying);
        AudioSource.Stop();
    }

    private void OnDied(IDamageable damageable)
    {
        StopWalkingAudio();
    }
}