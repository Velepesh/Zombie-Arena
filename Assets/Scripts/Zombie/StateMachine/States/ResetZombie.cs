using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Zombie))]
[RequireComponent(typeof(NavAgentEnabler))]
[RequireComponent(typeof(MeshChanger))]
public class ResetZombie : State
{
    private Zombie _zombie;
    private NavAgentEnabler _navAgent;
    private MeshChanger _meshChanger;

    private void Awake()
    {
        _navAgent = GetComponent<NavAgentEnabler>();
        _meshChanger = GetComponent<MeshChanger>();
        _zombie = GetComponent<Zombie>();
    }

    private void OnEnable()
    {
        Reset();
    }

    private void Reset()
    {
        _zombie.Health.RestoreHealth();
        _navAgent.EnableAgent();
        _meshChanger.WearStandartMesh();
        _zombie.DisableZombie();
    }
}