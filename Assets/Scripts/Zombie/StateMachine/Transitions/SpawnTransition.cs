using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public class SpawnTransition : Transition
{
    private StateMachine _stateMachine;
    private Zombie _zombie;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
        _stateMachine = GetComponent<StateMachine>();
    }

    private void OnEnable()
    {
        NeedTransit = false;
        _zombie.Enabled += OnEnabled;
    }

    private void OnDisable()
    {
        _zombie.Enabled -= OnEnabled;
    }

    private void OnEnabled()
    {
        Debug.Log("OnEnable");
       //_stateMachine.SetFirstState();
       // enabled = false;
        //NeedTransit = true;
    }
}