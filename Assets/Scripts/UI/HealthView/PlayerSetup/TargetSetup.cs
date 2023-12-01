using UnityEngine;
using System;

public abstract class TargetSetup : MonoBehaviour
{
    public abstract void AddHealth(int value);

    public event Action<Health> HealthLoaded;

    protected void OnHealthLoaded(Health health)
    {
        HealthLoaded?.Invoke(health);
    }
}