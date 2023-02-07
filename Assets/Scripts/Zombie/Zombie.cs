using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class Zombie : MonoCache, IDamageable
{
    [SerializeField] private Health _health;
    [SerializeField] private ZombieOptions _options;
    [SerializeField] private int _score;
    [SerializeField] private ZombieType _type;

    private ZombieTargets _zombieTargets;
    private ITarget _currentTarget;
    private ITarget _mainTarget;
    private Vector3 _contactPosition;
    private DamageHandler[] _damageHandlers = new DamageHandler[] { };

    public ZombieOptions Options => _options;
    public ZombieType Type => _type;
    public bool IsDied => _health.Value <= 0;
    public DamageHandlerType LastDamageHandlerType { get; private set; }
    public bool WasHeadHit { get; private set; }

    public Health Health => _health;
    public Vector3 CurrentTargetPosition => _currentTarget.Position;
    public Vector3 ContactPosition => _contactPosition;
    public ITarget CurrentTarget => _currentTarget;

    public event UnityAction<Zombie> Spawned;
    public event UnityAction<IDamageable> Died;
    public event UnityAction<DamageHandlerType> HitTaken;

    private void Start()
    {
        AddUpdate();
        InitDamageHandler();
        Spawned?.Invoke(this);
    }

    private void OnDisable()
    {
        RemoveUpdate();
        for (int i = 0; i < _damageHandlers.Length; i++)
            _damageHandlers[i].HitTaken -= OnHitTaken;
    }

    public void Init(ZombieTargets zombieTargets)
    {
        _zombieTargets = zombieTargets;

        _currentTarget = _zombieTargets.GetRandomTarget();
        _mainTarget = _currentTarget;
    }

    public void TakeDamage(int damage, Vector3 contactPosition)
    {
        _health.TakeDamage(damage);
        _contactPosition = contactPosition;

        if (IsDied)
        {
            DisableDamageHendlers();
            Die();
        }
    }

    /*
 * Изначально рандомный таргет
 * Потом если игрок рядом, тогда на него, но есть дистаниция, тогда опять башня
 * Если значально игрок, то не менять
 * 
 * 
 * Стартовый таргет, его будем пересылать как дистанция больше требуемой
 */
    public override void OnTick()
    {
        if (_mainTarget is Player || _isAttackedTower)
            return;

        float distanceToPlayerTarget = Vector3.Distance(transform.position, _zombieTargets.Player.Position);
        if(Options.ChangeTargetDistance >= distanceToPlayerTarget)
        {
            if (_currentTarget.Equals(_zombieTargets.Tower))
                _currentTarget = _zombieTargets.GetOtherTarget(_currentTarget, transform.position);
        }
        else//дОБАВИТЬ, еСЛИ НАЧАЛ БИТЬ БАШНЮ, ТО НЕ МЕНЯТЬ ТАРГЕТ
        {
            if (_currentTarget != _mainTarget)
                _currentTarget = _mainTarget;
        }
    }
    private bool _isAttackedTower;
    public void AttackedTower()
    {
        _isAttackedTower = true;
    }

    public void Die()
    {
        Died?.Invoke(this);
    }

    private void InitDamageHandler()
    {
        _damageHandlers = GetComponentsInChildren<DamageHandler>();

        for (int i = 0; i < _damageHandlers.Length; i++)
        {
            _damageHandlers[i].Init(this);
            _damageHandlers[i].HitTaken += OnHitTaken;
        }
    }

    private void DisableDamageHendlers()
    {
        foreach (DamageHandler handler in _damageHandlers)
            handler.Collider.isTrigger = true;
    }

    private void OnHitTaken(DamageHandlerType type)
    {
        if (type == DamageHandlerType.Head)
            WasHeadHit = true;

        LastDamageHandlerType = type;
        HitTaken?.Invoke(type);
    }
}