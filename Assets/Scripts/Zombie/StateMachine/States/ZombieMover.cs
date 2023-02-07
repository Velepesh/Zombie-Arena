using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(Zombie))]
[RequireComponent(typeof(NavAgentEnabler))]
public class ZombieMover : State
{
    private Zombie _zombie;
    private NavAgentEnabler _agent;

    public event UnityAction Moved;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
        _agent = GetComponent<NavAgentEnabler>();
    }

    private void OnEnable()
    {
        AddUpdate();
        Moved?.Invoke();
    }

    private void OnDisable() => RemoveUpdate();

    public override void OnTick()
    {
        Vector3 targetPosition = _zombie.CurrentTargetPosition;

        if (targetPosition == null)
            return;

        _agent.Agent.SetDestination(targetPosition);
    }
}