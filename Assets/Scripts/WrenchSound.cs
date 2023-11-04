using UnityEngine;

public class WrenchSound : MonoBehaviour
{
    [SerializeField] private Wrench _wrench;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _attackClip;
    [SerializeField] private AudioClip _hitClip;

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
        _audioSource.PlayOneShot(_attackClip);
    }

    private void OnHit()
    {
        _audioSource.PlayOneShot(_hitClip);
    }
}