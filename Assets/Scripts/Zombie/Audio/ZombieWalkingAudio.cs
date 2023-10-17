using UnityEngine;
using Plugins.Audio.Core;
using Plugins.Audio.Utils;

[RequireComponent(typeof(Zombie))]
[RequireComponent(typeof(ZombieMover))]
[RequireComponent(typeof(ZombieAttacker))]
public class ZombieWalkingAudio : MonoBehaviour
{
    [SerializeField] private SourceAudio _sourceAudio;
    [SerializeField] private AudioDataProperty _clip;

    private Zombie _zombie;
    private ZombieMover _mover;
    private ZombieAttacker _attacker;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
        _mover = GetComponent<ZombieMover>();
        _attacker = GetComponent<ZombieAttacker>();
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
        _sourceAudio.Play(_clip.Key);
    }

    private void StopWalkingAudio()
    {
        if (_sourceAudio.IsPlaying)
            _sourceAudio.Stop();
    }

    private void OnDied(IDamageable damageable)
    {
        StopWalkingAudio();
    }
}