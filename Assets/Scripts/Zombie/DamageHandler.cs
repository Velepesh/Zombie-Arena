using UnityEngine;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class DamageHandler : MonoBehaviour
{
    [SerializeField] private DamageHandlerType _type;

    readonly private int _headDamageMultiplier = 2;

    private IDamageable _damageable;

    private Collider _collider;

    public event UnityAction<DamageHandlerType> HitTaken;

    private void Start()
    {
        _collider = GetComponent<Collider>();
    }

    public void Init(IDamageable damageable)
    {
        _damageable = damageable;
    }

    public void TakeDamage(int damage, Vector3 contactPoint)
    {
        if (_damageable == null)
            Debug.LogError("Don't Init" + nameof(IDamageable));

        if (_damageable.Health.Value > 0)
        {
            if (damage <= 0)
                throw new ArgumentException(nameof(damage));

            if (_type == DamageHandlerType.Head)
                damage *= _headDamageMultiplier;

            HitTaken?.Invoke(_type);
            _damageable.TakeDamage(damage, contactPoint);
        }
    }

    public void DisableCollider()
    {
        _collider.enabled = false;
    }
}

public enum DamageHandlerType
{
    Body,
    Head
}