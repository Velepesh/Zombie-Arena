using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Zombie))]
[RequireComponent(typeof(NavAgentEnabler))]
public class DieTransition : Transition
{
    private Zombie _zombie;
    private NavAgentEnabler _agent;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
        _agent = GetComponent<NavAgentEnabler>();
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
        _agent.DisableAgent();
    }
}