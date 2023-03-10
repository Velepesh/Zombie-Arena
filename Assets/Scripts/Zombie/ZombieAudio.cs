using InfimaGames.LowPolyShooterPack;
using UnityEngine;

[RequireComponent(typeof(Zombie))]
public class ZombieAudio : Audio
{
    [SerializeField] private AudioClip _audioClipWalkiing;
    [SerializeField] private AudioClip _deadAudioClip;

    private Zombie _zombie;

    protected override void Awake()
    {
        base.Awake();
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
        PlayWalkingAudio();
    }

    private void PlayWalkingAudio()
    {
        AudioSource = GetComponent<AudioSource>();
        AudioSource.clip = _audioClipWalkiing;
        AudioSource.loop = true;

        if (AudioSource.isPlaying == false)
            AudioSource.Play();
    }

    private void OnDied(IDamageable damageable)
    {
        if (AudioSource.isPlaying)
            AudioSource.Stop();

        PlayOneShot(_deadAudioClip);
    }
}