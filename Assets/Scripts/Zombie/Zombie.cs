using UnityEngine;
using UnityEngine.Events;

public class Zombie : MonoBehaviour, IDamageable
{
    [SerializeField] private Health _health;
    [SerializeField] private ZombieOptions _options;
    [SerializeField] private int _score;

    private ITarget _target;

    public ZombieOptions Options => _options;
    public bool IsDied => _health.Value <= 0;

    public Health Health => _health;
    public Vector3 TargetPosition => _target.Position;

    public event UnityAction<IDamageable> Died;

    private void Start()
    {
        InitDamageHandler();
    }

    public void InitDamageHandler()
    {
        var damageHandlers = GetComponentsInChildren<DamageHandler>();

        foreach (var handler in damageHandlers) 
            handler.Init(this);
    }

    public void Die()
    {
        Died?.Invoke(this);
    }

    public void TakeDamage(int damage)
    {
        _health.TakeDamage(damage);

        if (IsDied)
            Die();
    }

    public void Init(ITarget target)
    {
        _target = target;
    }
}