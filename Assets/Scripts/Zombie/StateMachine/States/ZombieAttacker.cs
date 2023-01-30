using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Zombie))]
public class ZombieAttacker : State
{
    private Zombie _zombie;

    public event UnityAction Attacked;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
    }

    private void OnEnable()
    {
        Attack();
    }

    private void Update()
    {
        transform.LookAt(_zombie.TargetPosition);
    }

    private void Attack()
    {
        Attacked?.Invoke();
    }
}