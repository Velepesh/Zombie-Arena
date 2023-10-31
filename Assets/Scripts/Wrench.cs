using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Wrench : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _attackClip;
    [SerializeField] private AudioClip _hitClip;
    [SerializeField] private int _damageToEnemy;
    [SerializeField] private int _damageToTwins;

    private Collider _collider;
    private bool _isTwinsHitted;
    private List<IDamageable> _damageables = new List<IDamageable>();

    private void OnValidate()
    {
        _damageToEnemy = Mathf.Clamp(_damageToEnemy, 0, int.MaxValue);
        _damageToTwins = Mathf.Clamp(_damageToTwins, 0, int.MaxValue);
    }

    private void Start()
    {
        _collider = GetComponent<Collider>();
        DisableCollider();
    }

    public void StartAttack()
    {
        _damageables.Clear();
        _isTwinsHitted = false;
        EnableCollider();
        _audioSource.PlayOneShot(_attackClip);
    }

    public void EndAttack()
    {
        DisableCollider();
    }

    private void EnableCollider()
    {
        _collider.enabled = true;
    }

    private void DisableCollider()
    {
        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            if (damageable is TwinCollider twinCollider)
            {
                if (_isTwinsHitted == false)
                {
                    twinCollider.TakeDamage(_damageToTwins, Vector3.zero);
                    _isTwinsHitted = true;
                }
            }
            else if (damageable is DamageHandler damageHandler)
            {
                Zombie zombie = damageHandler.Zombie;

                if (IsDamageableInList(_damageables, zombie) == false)
                {
                    damageHandler.TakeDamage(_damageToEnemy, Vector3.zero);
                    _damageables.Add(zombie);
                }
            }

            _audioSource.PlayOneShot(_hitClip);
        }
    }

    private bool IsDamageableInList(List<IDamageable> damageables, IDamageable damageable)
    {
        for (int i = 0; i < damageables.Count; i++)
        {
            if (damageable == damageables[i])
                return true;
        }

        return false;
    }
}