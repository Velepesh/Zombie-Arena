using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Zombie))]
public class ZombieAttacker : State
{
    [SerializeField] private AttackCollider _attackCollider;

    private Zombie _zombie;

    public event UnityAction Attacked;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
    }

    private void OnEnable()
    {
        AddUpdate();
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

        if (_zombie.CurrentTarget is Tower)
            _zombie.AttackedTower();
    }
}