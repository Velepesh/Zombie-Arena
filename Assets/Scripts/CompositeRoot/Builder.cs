using UnityEngine.Events;

public abstract class Builder : CompositeRoot
{
    public abstract void AddHealth(int value);
    public abstract Health GetHealth();

    public event UnityAction<Health> HealthLoaded;
    public event UnityAction Died;

    protected void OnHealthLoaded(Health health)
    {
        HealthLoaded?.Invoke(health);
    }

    protected void OnDied()
    {
        Died?.Invoke();
    }
}