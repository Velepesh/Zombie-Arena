using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Zombie))]
[RequireComponent(typeof(NavAgentEnabler))]
public class ZombieAttacker : State
{
    [SerializeField] private AttackCollider _attackCollider;

    private Zombie _zombie;
    private NavAgentEnabler _agent;

    public event UnityAction Attacked;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
        _agent = GetComponent<NavAgentEnabler>();
    }

    private void OnEnable()
    {
        AddUpdate();
        _agent.StopAgent();
        Attack();
    }

    private void OnDisable()
    {
        RemoveUpdate();
        _attackCollider.DisableCollider();
    }

    public override void OnTick()
    {
        Rotate();
    }

    private void OnStartAttackEvent()
    {
        _attackCollider.EnableCollider();
    }

    private void OnEndAttackEvent()
    {
        _attackCollider.DisableCollider();
    }

    private void Rotate()
    {
        Vector3 direction = _zombie.CurrentTargetPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _zombie.Options.RotationSpeed * Time.deltaTime);
    }

    private void Attack()
    {
        _attackCollider.Init(_zombie);
        Attacked?.Invoke();

        if (_zombie.CurrentTarget is Twin)
            _zombie.SetAttackedTwins();
    }
}