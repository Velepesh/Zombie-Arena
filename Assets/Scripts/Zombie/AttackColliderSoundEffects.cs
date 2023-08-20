using UnityEngine;

[RequireComponent(typeof(AttackCollider))]
public class AttackColliderSoundEffects : Audio
{
    [SerializeField] private AudioClip _takeDamageSound;
    [SerializeField] private AudioClip _attackAudioClip;

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
        PlayOneShot(_attackAudioClip);
    }
    
    private void OnHit()
    {
        PlayOneShot(_takeDamageSound);
    }
}