using Plugins.Audio.Utils;
using UnityEngine;

[RequireComponent(typeof(AttackCollider))]
public class AttackColliderSoundEffects : Audio
{
    [SerializeField] private AudioDataProperty _takeDamageSound;
    [SerializeField] private AudioDataProperty _attackAudioClip;

    private AttackCollider _attackCollider;

    private void Awake()
    {
        _attackCollider = GetComponent<AttackCollider>();
    }

    private void OnEnable()
    {
        _attackCollider.Attacked += OnAttacked;
        _attackCollider.Hit += OnHit;
    }

    private void OnDisable()
    {
        _attackCollider.Attacked -= OnAttacked;
        _attackCollider.Hit -= OnHit;
    }

    private void OnAttacked()
    {
        SourceAudio.PlayOneShot(_attackAudioClip.Key);
    }
    
    private void OnHit()
    {
        SourceAudio.PlayOneShot(_takeDamageSound.Key);
    }
}