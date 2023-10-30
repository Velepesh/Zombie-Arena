using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, IDamageable, ITarget, IHealth, IDie
{
    [SerializeField] private Health _health;

    public bool IsDied => _health.Value <= 0;

    public Health Health => _health;
    public Vector3 Position => transform.position;

    public event UnityAction<IDamageable> Died;
    public event UnityAction<Zombie> Attacked;

    public void TakeDamage(int damage, Vector3 contactPosition)
    {
        _health.TakeDamage(damage);

        if (IsDied)
            Die();
    }

    public void SetAttackingZombie(Zombie zombie)
    {
        Attacked?.Invoke(zombie);
    }

    public void Die()
    {
        Died?.Invoke(this);
    }
}
