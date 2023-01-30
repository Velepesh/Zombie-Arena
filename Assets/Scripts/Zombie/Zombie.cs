using UnityEngine;
using UnityEngine.Events;

public class Zombie : MonoBehaviour, IDamageable
{
    [SerializeField] private Health _health;
    [SerializeField] private ZombieOptions _options;
    [SerializeField] private int _score;

    private ITarget _target;
    private Vector3 _contactPosition;
    private DamageHandler[] _damageHandlers = new DamageHandler[] { };

    public ZombieOptions Options => _options;
    public bool IsDied => _health.Value <= 0;

    public Health Health => _health;
    public Vector3 TargetPosition => _target.Position;
    public Vector3 ContactPosition => _contactPosition;


    public event UnityAction<IDamageable> Died;

    private void Start()
    {
        InitDamageHandler();
    }

    public void Init(ITarget target)
    {
        _target = target;
    }

    public void InitDamageHandler()
    {
        _damageHandlers = GetComponentsInChildren<DamageHandler>();

        foreach (DamageHandler handler in _damageHandlers) 
            handler.Init(this);
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
}