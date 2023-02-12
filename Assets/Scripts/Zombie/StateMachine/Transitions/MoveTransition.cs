using UnityEngine;

[RequireComponent(typeof(Zombie))]
[RequireComponent(typeof(NavAgentEnabler))]
public class MoveTransition : Transition
{
    private Zombie _zombie;
    private NavAgentEnabler _agent;
    private bool _isSpawned;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
        _agent = GetComponent<NavAgentEnabler>();
    }

    private void OnEnable()
    {
        AddUpdate();
       
        NeedTransit = false;
        EnableNavMeshAgent();

        _zombie.Spawned += OnSpawned;
    }

    private void OnDisable()
    {
        RemoveUpdate();
        _zombie.Spawned -= OnSpawned;
    }

    public override void OnTick()
    {
        if (_isSpawned == false)
            return;

        Vector3 targetPosition = _zombie.CurrentTargetPosition;

        if (targetPosition == null)
            return;

        float distance = Vector3.Distance(targetPosition, transform.position);

        if (distance > _zombie.Options.AttackDistance)
            Transit();
    }

    private void EnableNavMeshAgent()
    {
        _agent.EnableAgent();
    }

    private void OnSpawned(Zombie zombie)
    {
        _isSpawned = true;
        Transit();
    }

    private void Transit()
    {
        NeedTransit = true;
        _agent.StartAgent();
    }
}
