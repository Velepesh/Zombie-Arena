using UnityEngine.Events;

public interface IDamageable
{
    Health Health { get; }
    event UnityAction<IDamageable> Died;
    void TakeDamage(int damage);
    void Die();
}