using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Zombie))]
[RequireComponent(typeof(NavMeshAgent))]
public class DieTransition : Transition
{
    private Zombie _zombie;
    private NavMeshAgent _agent;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        _zombie.Died += OnDied;
    }

    private void OnDisable()
    {
        _zombie.Died -= OnDied;
    }

    private void OnDied(IDamageable damageable)
    {
        _agent.enabled = false;
        NeedTransit = true;
    }
}