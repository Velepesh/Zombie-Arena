using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, IDamageable, ITarget
{
    [SerializeField] private Health _health;

    public bool IsDied => _health.Value <= 0;

    public Health Health => _health;
    public Vector3 Position => transform.position;

    public event UnityAction<IDamageable> Died;

    public void TakeDamage(int damage)
    {
        _health.TakeDamage(damage);

        if (IsDied)
            Die();
    }

    public void Die()
    {
        Died?.Invoke(this);
    }
}
