using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using UnityEngine.Networking.Types;

[RequireComponent(typeof(Zombie))]
[RequireComponent(typeof(AudioSource))]
public class ZombieAudio : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClipWalkiing;
    [SerializeField] private AudioClip _deadAudioClip;

    [Tooltip("Audio Settings.")]
    [SerializeField]
    private InfimaGames.LowPolyShooterPack.AudioSettings _audioSettings = new InfimaGames.LowPolyShooterPack.AudioSettings(1.0f, 0.0f, true);

    private Zombie _zombie;
    private AudioSource _audioSource;
    private IAudioManagerService _audioManagerService;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
        _audioSource = GetComponent<AudioSource>();
        _audioManagerService ??= ServiceLocator.Current.Get<IAudioManagerService>();
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
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _audioClipWalkiing;
        _audioSource.loop = true;

        if (_audioSource.isPlaying == false)
            _audioSource.Play();
    }

    private void OnDied(IDamageable damageable)
    {
        if (_audioSource.isPlaying)
            _audioSource.Pause();

        _audioManagerService.PlayOneShotDelayed(_deadAudioClip, _audioSettings, 0f);
    }
}