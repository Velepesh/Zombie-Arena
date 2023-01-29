using UnityEngine;
using System;

public class DamageHandler : MonoBehaviour
{
    [SerializeField] private DamageHandlerType _type;

    readonly private int _headDamageMultiplier = 2;

    private IDamageable _damageable;

    public void Init(IDamageable damageable)
    {
        _damageable = damageable;
    }

    public void TakeDamage(int damage)
    {
        if (_damageable == null)
            Debug.LogError("Don't Init" + nameof(IDamageable));

        if (damage <= 0)
            throw new ArgumentException(nameof(damage));

        if (_type == DamageHandlerType.Head)
            damage *= _headDamageMultiplier;

        _damageable.TakeDamage(damage);
    }
}

enum DamageHandlerType
{
    Body,
    Head
}