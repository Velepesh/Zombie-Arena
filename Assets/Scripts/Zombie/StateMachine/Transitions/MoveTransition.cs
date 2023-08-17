using UnityEngine;

[RequireComponent(typeof(Zombie))]
public class MoveTransition : Transition
{
    private Zombie _zombie;
    private bool _isSpawned;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
    }

    private void OnEnable()
    {
        AddUpdate();
       
        NeedTransit = false;

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

        if (distance > _zombie.Options.AttackDistance && _zombie.IsAttacking == false)
            Transit();
    }

    private void OnSpawned(Zombie zombie)
    {
        _isSpawned = true;
        Transit();
    }

    private void Transit()
    {
        NeedTransit = true;
    }
}
