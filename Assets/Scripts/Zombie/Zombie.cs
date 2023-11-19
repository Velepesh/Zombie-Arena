using UnityEngine;
using UnityEngine.Events;

public class Zombie : MonoCache, IDamageable, IHealth, IDie
{
    [SerializeField] private Health _health;
    [SerializeField] private ZombieOptions _options;
    [SerializeField] private ZombieOptions _mobileOptions;

    private ZombieTargetsCompositeRoot _zombieTargets;
    private ITarget _currentTarget;
    private ITarget _mainTarget;
    private Vector3 _contactPosition;
    private DamageHandler[] _damageHandlers = new DamageHandler[] { };
    private bool _isAttackedTwins;
    private bool _isAttacking;
    private DamageHandlerType _lastDamageHandlerType;
    private ZombieOptions _currentOptions;

    public ZombieOptions Options => _currentOptions;
    public bool IsDied => _health.Value <= 0;
    public bool IsAttacking => _isAttacking;
    public bool WasHeadHit { get; private set; }
    public bool IsHeadKill { get; private set; }

    public Health Health => _health;
    public Vector3 CurrentTargetPosition => _currentTarget.Position;
    public Vector3 ContactPosition => _contactPosition;
    public ITarget CurrentTarget => _currentTarget;

    public event UnityAction HeadKilled;
    public event UnityAction<Zombie> Spawned;
    public event UnityAction<IDamageable> Died;
    public event UnityAction<Zombie> Disabled;
    public event UnityAction<DamageHandlerType> HitTaken;

    private void Start()
    {
        InitDamageHandler();
    }

    private void OnEnable() => AddUpdate();

    private void OnDisable()
    {
        RemoveUpdate();

        for (int i = 0; i < _damageHandlers.Length; i++)
            _damageHandlers[i].HitTaken -= OnHitTaken;
    }

    public void Init(ZombieTargetsCompositeRoot zombieTargets, bool isMobile)
    {
        _zombieTargets = zombieTargets;
        SetOptions(isMobile);

        _currentTarget = _zombieTargets.GetRandomTarget();
        _mainTarget = _currentTarget;
    }

    public void SetAttackingState(bool isAttacking)
    {
        _isAttacking = isAttacking;
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
        if (_mainTarget.Equals(_zombieTargets.Player) || _isAttackedTwins)
            return;

        ChangeCurrentTarget();
    }

    public void SetAttackedTwins()
    {
        _isAttackedTwins = true;
    }

    public void Die()
    {
        if (_lastDamageHandlerType == DamageHandlerType.Head)
        {
            IsHeadKill = true;
            HeadKilled?.Invoke();
        }

        Died?.Invoke(this);
    }

    public void DisableZombie()
    {
        Disabled?.Invoke(this);
    }

    private void SetOptions(bool isMobile)
    {
        if (isMobile)
            _currentOptions = _mobileOptions;
        else
            _currentOptions = _options;
    }

    private void ChangeCurrentTarget()
    {
        float distanceToPlayerTarget = Vector3.Distance(transform.position, _zombieTargets.Player.Position);

        if (Options.ChangeTargetDistance >= distanceToPlayerTarget)
        {
            if (_currentTarget is Twins)
                _currentTarget = _zombieTargets.Player;
        }
        else if (_currentTarget != _mainTarget)
        {
            _currentTarget = _mainTarget;
        }
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
            WasHeadHit = true;

        _lastDamageHandlerType = type;
        HitTaken?.Invoke(type);
    }
}