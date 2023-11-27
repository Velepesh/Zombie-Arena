using UnityEngine;
using System;

[Serializable]
public class Health
{
    [SerializeField] private int _health;

    private int _startHealth;

    public int Value => _health;

    public event Action<int> HealthChanged;
    public event Action<int> HealthAdded;

    public void SetHealth(int value)
    {
        if (value <= 0)
            throw new ArgumentException(nameof(value));

        _health = value;
        SetStartHealth(value);
    }

    public void Reborn()
    {
        _health = _startHealth;
        HealthChanged?.Invoke(_health);
    }

    public void AddHealth(int value)
    {
        if (value < 0)
            throw new ArgumentException(nameof(value));

        _health += value;
        SetStartHealth(value);
        HealthAdded?.Invoke(_health);
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

    private void SetStartHealth(int value)
    {
        _startHealth = value;
    }
}