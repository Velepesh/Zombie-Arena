using UnityEngine;
using UnityEngine.Events;

public class ZombieHealthView : HealthView
{
    [SerializeField] private Zombie _zombie;
    [SerializeField] private SliderAnimation _fader;

    public event UnityAction HealthChanged;

    private void OnEnable()
    {
        _zombie.Health.HealthChanged += OnHealthChanged;
        _zombie.Died += OnDied;
    }

    private void OnDisable()
    {
        _zombie.Health.HealthChanged -= OnHealthChanged;
        _zombie.Died -= OnDied;
    }

    private void Start()
    {
        Init(_zombie);
    }

    private void OnHealthChanged(int health)
    {
        SetHealth(health);
        HealthChanged?.Invoke();
    }

    private void OnDied(IDamageable damageable)
    {
        DisableSliders();
    }
}