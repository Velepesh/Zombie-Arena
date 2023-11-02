using UnityEngine;
using System;
using UnityEngine.Events;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class DamageHandler : MonoBehaviour, IDamageable
{
    [SerializeField] private DamageHandlerType _type;

    readonly private int _headDamageMultiplier = 2;

    private Zombie _zombie;
    private bool _isIgnoringPlayer;
    private Collider _collider;
    private List<ParticleSystem> _holeEffects = new List<ParticleSystem>();

    public Zombie Zombie => _zombie;
    public DamageHandlerType Type => _type;

    public event UnityAction<DamageHandlerType> HitTaken;

    private void Start()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnDisable()
    {
        if (_zombie != null)
        {
            _zombie.Died -= OnDied;
            _zombie.HeadKilled -= OnHeadKilled;
        }
    }

    public void Init(IDamageable damageable)
    {
        if (damageable is Zombie zombie)
        {
            _zombie = zombie;
            _zombie.Died += OnDied;
            _zombie.HeadKilled += OnHeadKilled;
        }
    }

    public void TakeDamage(int damage, Vector3 contactPoint)
    {
        if (_zombie == null)
            Debug.LogError("Don't Init" + nameof(IDamageable));

        if (_zombie.Health.Value > 0)
        {
            if (damage < 0)
                throw new ArgumentException(nameof(damage));

            if (_type == DamageHandlerType.Head)
                damage *= _headDamageMultiplier;

            HitTaken?.Invoke(_type);
            _zombie.TakeDamage(damage, contactPoint);
        }
    }

    public void IgnorePlayerCollider()
    {
        _isIgnoringPlayer = true;
    }

    public void AddHoleEffect(ParticleSystem holeEffect)
    {
        _holeEffects.Add(holeEffect);
    }

    private void DestroyHoleEffects()
    {
        for (int i = 0; i < _holeEffects.Count; i++)
            _holeEffects[i].gameObject.SetActive(false);
    }

    private void OnHeadKilled()
    {
        if (_type == DamageHandlerType.Head)
        {
            _collider.enabled = false;
            DestroyHoleEffects();
        }
    }

    private void OnDied(IDamageable damageable)
    {
        SwitchLayerToIgnorePLayer();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isIgnoringPlayer && collision.gameObject.TryGetComponent(out PlayerCollider player))
            Physics.IgnoreCollision(player.Collider, _collider);
    }

    private void SwitchLayerToIgnorePLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");
    }
}
public enum DamageHandlerType
{
    Body,
    Head
}