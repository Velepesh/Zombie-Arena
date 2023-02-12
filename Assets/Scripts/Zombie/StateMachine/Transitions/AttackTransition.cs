using UnityEngine;

[RequireComponent(typeof(Zombie))]
[RequireComponent(typeof(NavAgentEnabler))]
public class AttackTransition : Transition
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
        AddUpdate();
    }

    private void OnDisable() => RemoveUpdate();

    public override void OnTick()
    {  
        Vector3 targetPosition = _zombie.CurrentTargetPosition;

        if (targetPosition == null)
            return;

        float distance = Vector3.Distance(targetPosition, transform.position);

        if (distance <= _zombie.Options.AttackDistance)
            Transit();
    }

    private void Transit()
    {
        NeedTransit = true;
        _agent.StopAgent();
    }
}