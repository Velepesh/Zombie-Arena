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
    }

    private void Update()//Здесь проверяется только расстояние
    {  
        Vector3 targetPosition = _zombie.TargetPosition;

        if (targetPosition == null)
            return;

        float distance = Vector3.Distance(targetPosition, transform.position);

        if (distance <= _zombie.Options.AttackDistance)
            Transit();
    }

    private void Transit()
    {
        NeedTransit = true;
    }
}