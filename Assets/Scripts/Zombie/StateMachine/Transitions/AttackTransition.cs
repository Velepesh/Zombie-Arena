using UnityEngine;

[RequireComponent(typeof(Zombie))]
public class AttackTransition : Transition
{
    private Zombie _zombie;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
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

        if (distance <= _zombie.Options.AttackDistance && _zombie.IsDied == false)
            Transit();
    }

    private void Transit()
    {
        NeedTransit = true;
    }
}