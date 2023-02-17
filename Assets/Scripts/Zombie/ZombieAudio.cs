using InfimaGames.LowPolyShooterPack;
using UnityEngine;

[RequireComponent(typeof(Zombie))]
[RequireComponent(typeof(AudioSource))]
public class ZombieAudio : MonoBehaviour
{
    [Tooltip("Audio Settings.")]
    [SerializeField]
    private InfimaGames.LowPolyShooterPack.AudioSettings _audioSettings = new InfimaGames.LowPolyShooterPack.AudioSettings(1.0f, 0.0f, true);

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
       
    }

    private void OnDisable()
    {
      
    }
}