using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Zombie))]
[RequireComponent(typeof(SpawningZombie))]
[RequireComponent(typeof(NavAgentEnabler))]
public class MoveTransition : Transition
{
    private Zombie _zombie;
    private SpawningZombie _spawning;
    private NavAgentEnabler _agent;
    private bool _isSpawned;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
        _spawning = GetComponent<SpawningZombie>();
        _agent = GetComponent<NavAgentEnabler>();
    }

    private void OnEnable()
    {
        AddUpdate();
       
        NeedTransit = false;
        EnableNavMeshAgent();

        _spawning.Spawned += OnSpawned;
    }

    private void OnDisable()
    {
        RemoveUpdate();
        _spawning.Spawned -= OnSpawned;
    }

    public override void OnTick()
    {
        if (_isSpawned == false)
            return;

        Vector3 targetPosition = _zombie.TargetPosition;

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

    private void OnSpawned()
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