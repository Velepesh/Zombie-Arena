using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class DamageHandler : MonoBehaviour
{
    [SerializeField] private DamageHandlerType _type;

    readonly private int _headDamageMultiplier = 2;

    private IDamageable _damageable;

    public Collider Collider { get; private set; }

    private void Start()
    {
        Collider = GetComponent<Collider>();
    }

    public void Init(IDamageable damageable)
    {
        _damageable = damageable;
    }

    public void TakeDamage(int damage, Vector3 contactPoint)
    {
        if (_damageable == null)
            Debug.LogError("Don't Init" + nameof(IDamageable));

        if (damage <= 0)
            throw new ArgumentException(nameof(damage));

        if (_type == DamageHandlerType.Head)
            damage *= _headDamageMultiplier;

        _damageable.TakeDamage(damage, contactPoint);
    }
}

enum DamageHandlerType
{
    Body,
    Head
}