using UnityEngine;

[RequireComponent(typeof(Zombie))]
public class StateMachine : MonoCache
{
    [SerializeField] private State _firstState;

    private State _currentState;

    public State Current => _currentState;

    private void OnEnable()
    {
        AddUpdate();
    }

    private void Start()
    {
        SetFirstState();
    }

    private void OnDisable() => RemoveUpdate();

    public void SetFirstState()
    {
        Transit(_firstState);
    }

    public void ResetTargetState()
    {
        _currentState = null;
    }

    public override void OnTick()
    {
        if (_currentState == null)
            return;

        var nextState = _currentState.GetNextState();

        if (nextState != null)
            Transit(nextState);
    }

    private void Transit(State nextState)
    {
        if (_currentState != null)
            _currentState.Exit();

        _currentState = nextState;

        if (_currentState != null)
            _currentState.Enter();
    }
}