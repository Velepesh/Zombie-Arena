using UnityEngine;
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
    private bool _isAttackedTower;

    public ZombieOptions Options => _options;
    public ZombieType Type => _type;
    public bool IsDied => _health.Value <= 0;
    public DamageHandlerType LastDamageHandlerType { get; private set; }
    public bool WasHeadHit { get; private set; }

    public Health Health => _health;
    public Vector3 CurrentTargetPosition => _currentTarget.Position;
    public Vector3 ContactPosition => _contactPosition;
    public ITarget CurrentTarget => _currentTarget;

    public event UnityAction HeadKilled;
    public event UnityAction HeadDamaged;
    public event UnityAction<Zombie> Spawned;
    public event UnityAction<IDamageable> Died;
    public event UnityAction<Zombie> Disabled;
    public event UnityAction<DamageHandlerType> HitTaken;

    private void OnEnable() => AddUpdate();

    private void Start()
    {
        InitDamageHandler();
        _health.SetStartHealth();
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

    public void EndSpawn()
    {
        Spawned?.Invoke(this);
    }

    public void TakeDamage(int damage, Vector3 contactPosition)
    {
        _health.TakeDamage(damage);
        _contactPosition = contactPosition;

        if (IsDied)
            Die();
    }

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
        else
        {
            if (_currentTarget != _mainTarget)
                _currentTarget = _mainTarget;
        }
    }

    public void AttackedTower()
    {
        _isAttackedTower = true;
    }

    public void Die()
    {
        if (LastDamageHandlerType == DamageHandlerType.Head)
            HeadKilled?.Invoke();

        Died?.Invoke(this);
    }

    public void DisableZombie()
    {
        Disabled?.Invoke(this);
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

    private void OnHitTaken(DamageHandlerType type)
    {
        if (type == DamageHandlerType.Head)
        {
            HeadDamaged?.Invoke();
            WasHeadHit = true;
        }

        LastDamageHandlerType = type;
        HitTaken?.Invoke(type);
    }
}