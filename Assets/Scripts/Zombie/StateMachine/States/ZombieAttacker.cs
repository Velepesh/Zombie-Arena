using UnityEngine.Events;

public class ZombieAttacker : State
{
    public event UnityAction Attacked;

    private void Start()
    {
        Attack();
    }

    private void Attack()
    {
        Attacked?.Invoke();
    }
}