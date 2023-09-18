using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class Health
{
    [SerializeField] private int _health;

    public int Value => _health;
    public event UnityAction<int> HealthChanged;

    public void SetStartHealth(int health)
    {
        if (health <= 0)
            throw new ArgumentException(nameof(health));

        _health = health;
    }

    public void AddHealth(int value)
    {
        if (value < 0)
            throw new ArgumentException(nameof(value));

        _health += value;
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0)
            throw new ArgumentException(nameof(damage));

        _health -= damage;

        if (_health <= 0)
            _health = 0;

        HealthChanged?.Invoke(_health);
    }
}