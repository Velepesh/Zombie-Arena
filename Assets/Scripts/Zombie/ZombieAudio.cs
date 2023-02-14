using InfimaGames.LowPolyShooterPack;
using UnityEngine;

[RequireComponent(typeof(Zombie))]
[RequireComponent(typeof(AudioSource))]
public class ZombieAudio : MonoBehaviour
{
    [SerializeField] private AudioClip _headKilledAudioClip;
    [SerializeField] private AudioClip _headDamageddAudioClip;
    [Tooltip("Audio Settings.")]
    [SerializeField]
    private InfimaGames.LowPolyShooterPack.AudioSettings audioSettings = new InfimaGames.LowPolyShooterPack.AudioSettings(1.0f, 0.0f, true);

    private Zombie _zombie;
    private AudioSource _audioSource;
    private IAudioManagerService audioManagerService;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
        _audioSource = GetComponent<AudioSource>();
        audioManagerService ??= ServiceLocator.Current.Get<IAudioManagerService>();
    }

    private void OnEnable()
    {
        _zombie.HeadKilled += OnHeadKilled;
        _zombie.HeadDamaged += OnHeadDamaged;
    }

    private void OnDisable()
    {
        _zombie.HeadKilled -= OnHeadKilled;
        _zombie.HeadDamaged -= OnHeadDamaged;
    }

    private void OnHeadKilled()
    {
        audioManagerService.PlayOneShotDelayed(_headKilledAudioClip, audioSettings, 0f);
    }

    private void OnHeadDamaged()
    {
        audioManagerService.PlayOneShotDelayed(_headDamageddAudioClip, audioSettings, 0f);
    }
}