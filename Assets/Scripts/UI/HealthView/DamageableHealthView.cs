using UnityEngine.Events;

public class DamageableHealthView : HealthView
{
    public event UnityAction<int, int> HealthChanged;

    public void SetIDamageable(IDamageable damageable)
    {
        Init(damageable);
        UpdateView(damageable.Health.Value);
    }

    public void UpdateView(int health)
    {
        SetHealth(health);
        HealthChanged?.Invoke(StartHealth, health);
    }
}