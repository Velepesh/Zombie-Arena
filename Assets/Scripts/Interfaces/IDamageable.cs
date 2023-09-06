using UnityEngine;
using UnityEngine.Events;

public interface IDamageable
{
    Health Health { get; }
    event UnityAction<IDamageable> Died;
    void TakeDamage(int damage, Vector3 contactPosition);
    void Die();
}