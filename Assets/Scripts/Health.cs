using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class Health
{
    [SerializeField] private int _health;

    private int _startValue;

    public int Value => _health;
    public int StartValue => _startValue;

    public event UnityAction Healed;
    public event UnityAction<int> HealthChanged;

    public void AddHealth(int health)
    {
        IncreaseHealth(health);
    }

    public void Heal(int health)
    {
        IncreaseHealth(health);

        Healed?.Invoke();
    }

    public void RecordHealth()
    {
        _startValue = _health;
    }

    public void RestoreHealth()
    {
        _health = _startValue;
    }

    public void RemoveHealth(int health)
    {
        if (health > _health)
            _health = 0;
        else
            _health -= health;

        HealthChanged?.Invoke(_health);
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
            _health = 0;

        HealthChanged?.Invoke(_health);
    }

    private void IncreaseHealth(int health)
    {
        _health += health;

        HealthChanged?.Invoke(_health);
    }
}