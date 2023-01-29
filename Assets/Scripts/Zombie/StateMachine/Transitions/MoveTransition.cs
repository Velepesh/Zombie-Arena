using UnityEngine;

[RequireComponent(typeof(Zombie))]
[RequireComponent(typeof(SpawningZombie))]
public class MoveTransition : Transition
{
    private Zombie _zombie;
    private SpawningZombie _spawning;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
        _spawning = GetComponent<SpawningZombie>();
    }

    private void OnEnable()
    {
        NeedTransit = false;
        _spawning.Spawned += OnSpawned;
    }

    private void OnDisable()
    {
        _spawning.Spawned += OnSpawned;
    }

    private void Update()//Если до этого был удар
    {
        //Vector3 targetPosition = _zombie.TargetPosition;

        //if (targetPosition == null)
        //    return;

        //float distance = Vector3.Distance(targetPosition, transform.position);

        //if (distance > _zombie.Options.AttackDistance)
        //    Transit();
    }

    private void OnSpawned()
    {
        Transit();
    }

    private void Transit()
    {
        NeedTransit = true;
    }
}
