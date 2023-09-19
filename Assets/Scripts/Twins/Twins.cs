using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Twins : MonoBehaviour, IDamageable, ITarget
{
    [SerializeField] private Health _health;
    [SerializeField] private List<TwinCollider> _twinColliders;

    public bool IsDied => _health.Value <= 0;

    public Health Health => _health;
    public Vector3 Position => transform.position;

    public event UnityAction<IDamageable> Died;

    private void OnEnable()
    {
        for (int i = 0; i < _twinColliders.Count; i++)
            _twinColliders[i].Damaged += OnDamaged;
    }

    private void OnDisable()
    {
        for (int i = 0; i < _twinColliders.Count; i++)
            _twinColliders[i].Damaged -= OnDamaged;
    }

    public void TakeDamage(int damage, Vector3 contatPosition)
    {
        _health.TakeDamage(damage);

        if (IsDied)
            Die();
    }

    public void Die()
    {
        Died?.Invoke(this);
    }

    private void OnDamaged(int damage, Vector3 normal)
    {
        TakeDamage(damage, normal);
    }
}