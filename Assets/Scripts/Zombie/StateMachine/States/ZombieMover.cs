using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(Zombie))]
[RequireComponent(typeof(NavMeshAgent))]
public class ZombieMover : State
{
    private Zombie _zombie;
    private NavMeshAgent _agent;

    public event UnityAction Moved;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        _agent.isStopped = false;
        Moved?.Invoke();
    }

    private void OnDisable()
    {
        _agent.isStopped = true;
    }

    private void Update()
    {
        Vector3 targetPosition = _zombie.TargetPosition;

        if (targetPosition == null)
            return;

        _agent.SetDestination(targetPosition);
    }
}