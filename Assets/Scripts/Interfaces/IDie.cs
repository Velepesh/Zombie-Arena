using UnityEngine.Events;

public interface IDie
{
    event UnityAction<IDamageable> Died;
    void Die();
}