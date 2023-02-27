using UnityEngine;
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

    private void Start()
    {
        SetMoveSpeed();
    }

    private void OnEnable()
    {
        AddUpdate();
        _agent.EnableAgent();
        _agent.StartAgent();
        Moved?.Invoke();
    }

    private void OnDisable() => RemoveUpdate();

    public override void OnTick()
    {
        Vector3 targetPosition = _zombie.CurrentTargetPosition;

        if (targetPosition == null || _agent.Agent.enabled == false)
            return;

        _agent.Agent.SetDestination(targetPosition);
    }

    private void SetMoveSpeed()
    {
        _agent.Agent.speed = _zombie.Options.MoveSpeed;
    }
}