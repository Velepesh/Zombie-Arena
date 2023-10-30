using UnityEngine.Events;

public class DamageableHealthView : HealthView
{
    public event UnityAction<int, int> HealthChanged;

    public void SetIHealth(IHealth health)
    {
        Init(health);
        UpdateView(health.Health.Value);
    }

    public void UpdateView(int health)
    {
        SetHealth(health);
        HealthChanged?.Invoke(StartHealth, health);
    }
}