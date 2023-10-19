using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Zombie))]
public class NavAgentEnabler : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Zombie _zombie;

    public NavMeshAgent Agent => _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _zombie = GetComponent<Zombie>();

        SetAgentOptions();
    }

    public void EnableAgent()
    {
        if(_agent.enabled == false)
            _agent.enabled = true;
    }

    public void DisableAgent()
    {
        if (_agent.enabled == true)
            _agent.enabled = false;
    }

    public void StartAgent()
    {
        _agent.isStopped = false;
    }

    public void StopAgent()
    {
        _agent.isStopped = true;
    }

    private void SetAgentOptions()
    {
        _agent.speed = _zombie.Options.MoveSpeed;
    }
}