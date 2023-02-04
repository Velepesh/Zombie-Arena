using UnityEngine;
using UnityEngine.Events;

public class Zombie : MonoBehaviour, IDamageable//Bool что было попадание в голову, в скор обращается спавнер при смерти зомби
{
    [SerializeField] private Health _health;
    [SerializeField] private ZombieOptions _options;
    [SerializeField] private int _score;
    [SerializeField] private ZombieType _type;

    private ITarget _target;
    private Vector3 _contactPosition;
    private DamageHandler[] _damageHandlers = new DamageHandler[] { };

    public ZombieOptions Options => _options;
    public ZombieType Type => _type;
    public bool IsDied => _health.Value <= 0;
    public DamageHandlerType LastDamageHandlerType { get; private set; }
    public bool WasHeadHit { get; private set; }

    public Health Health => _health;
    public Vector3 TargetPosition => _target.Position;
    public Vector3 ContactPosition => _contactPosition;

    public event UnityAction<IDamageable> Died;
    public event UnityAction<DamageHandlerType> HitTaken;

    private void Start()
    {
        InitDamageHandler();
    }

    private void OnDisable()
    {
        for (int i = 0; i < _damageHandlers.Length; i++)
            _damageHandlers[i].HitTaken -= OnHitTaken;
    }

    public void Init(ITarget target)
    {
        _target = target;
    }

    public void InitDamageHandler()
    {
        _damageHandlers = GetComponentsInChildren<DamageHandler>();

        for (int i = 0; i < _damageHandlers.Length; i++)
        {
            _damageHandlers[i].Init(this);
            _damageHandlers[i].HitTaken += OnHitTaken;
        }
    }

    public void Die()
    {
        Died?.Invoke(this);
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