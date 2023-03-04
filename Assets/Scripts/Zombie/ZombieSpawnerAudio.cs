using InfimaGames.LowPolyShooterPack;
using UnityEngine;

[RequireComponent(typeof(ZombieSpawner))]
[RequireComponent(typeof(AudioSource))]
public class ZombieSpawnerAudio : MonoBehaviour
{
    [SerializeField] private AudioClip _headKilledAudioClip;
    [Tooltip("Audio Settings.")]
    [SerializeField]
    private InfimaGames.LowPolyShooterPack.AudioSettings _audioSettings = new InfimaGames.LowPolyShooterPack.AudioSettings(1.0f, 0.0f, true);

    private ZombieSpawner _zombieSpawner;
    private IAudioManagerService _audioManagerService;

    private void Awake()
    {
        _zombieSpawner = GetComponent<ZombieSpawner>();

        _audioManagerService ??= ServiceLocator.Current.Get<IAudioManagerService>();
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
        if(zombie.IsHeadKill)
            _audioManagerService.PlayOneShotDelayed(_headKilledAudioClip, _audioSettings, 0f);
    }
}
