using Plugins.Audio.Utils;
using UnityEngine;

public class WrenchSound : Audio
{
    [SerializeField] private Wrench _wrench;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioDataProperty _attackClip;
    [SerializeField] private AudioDataProperty _hitClip;

    private void OnEnable()
    {
        _wrench.AttackStarted += OnAttackStarted;
        _wrench.Hit += OnHit;
    }

    private void OnDisable()
    {
        _wrench.AttackStarted -= OnAttackStarted;
        _wrench.Hit -= OnHit;
    }

    private void OnAttackStarted()
    {
        SourceAudio.PlayOneShot(_attackClip.Key);
    }

    private void OnHit()
    {
        SourceAudio.PlayOneShot(_hitClip.Key);
    }
}