using UnityEngine;
using UnityEngine.Events;

public class ZombieHealthView : HealthView
{
    [SerializeField] private Zombie _zombie;
    [SerializeField] private SliderAnimation _fader;
    [SerializeField] private float _showDuration;

    public event UnityAction HealthChanged;

    private void OnValidate()
    {
        _showDuration = Mathf.Clamp(_showDuration, 0f, float.MaxValue);
    }

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
        DisableSlider();
    }
}