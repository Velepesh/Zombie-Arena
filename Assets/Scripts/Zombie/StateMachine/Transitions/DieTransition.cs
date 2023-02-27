using UnityEngine;

[RequireComponent(typeof(Zombie))]
public class DieTransition : Transition
{
    private Zombie _zombie;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
    }

    private void OnEnable()
    {
        NeedTransit = false;
        _zombie.Died += OnDied;
    }

    private void OnDisable()
    {
        _zombie.Died -= OnDied;
    }

    private void OnDied(IDamageable damageable)
    {
        NeedTransit = true;
    }
}