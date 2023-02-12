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

    public event UnityAction<int> HealthChanged;


    public void SetStartHealth()
    {
        _startValue = _health;
    }

    public void RestoreHealth()
    {
        _health = _startValue;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
            _health = 0;

        HealthChanged?.Invoke(_health);
    }
}