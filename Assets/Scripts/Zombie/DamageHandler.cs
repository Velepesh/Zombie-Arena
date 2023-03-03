using UnityEngine;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class DamageHandler : MonoBehaviour
{
    [SerializeField] private DamageHandlerType _type;

    readonly private int _headDamageMultiplier = 2;

    private IDamageable _damageable;
    private bool _isIgnoringPlayer;
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

    public void IgnorePLayerCollider()
    {
        _isIgnoringPlayer = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_isIgnoringPlayer && collision.gameObject.TryGetComponent(out PlayerCollider player))
            Physics.IgnoreCollision(player.Collider, _collider);

    }
}

public enum DamageHandlerType
{
    Body,
    Head
}