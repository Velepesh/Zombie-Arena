using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Wrench : MonoBehaviour
{
    [SerializeField] private int _damageToEnemy;
    [SerializeField] private int _damageToTwins;

    private Collider _collider;
    private bool _isTwinsHitted;
    private List<IDamageable> _damageables = new List<IDamageable>();

    public event UnityAction Hit;
    public event UnityAction AttackStarted;

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
        AttackStarted?.Invoke();
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
                    Hit?.Invoke();
                }
            }
            else if (damageable is DamageHandler damageHandler)
            {
                Zombie zombie = damageHandler.Zombie;

                if (IsDamageableInList(_damageables, zombie) == false)
                {
                    damageHandler.TakeDamage(_damageToEnemy, Vector3.zero);
                    _damageables.Add(zombie);
                    Hit?.Invoke();
                }
            }
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