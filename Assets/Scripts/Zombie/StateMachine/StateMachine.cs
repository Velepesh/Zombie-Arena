using UnityEngine;

public class StateMachine : MonoCache
{
    [SerializeField] private State _firstState;

    private ITarget _target;
    private State _currentState;

    public State Current => _currentState;

    private void OnEnable() => AddUpdate();

    private void OnDisable() => RemoveUpdate();

    private void Start()
    {
        Reset(_firstState);
    }

    public override void OnTick()
    {
        if (_currentState == null)
            return;

        var nextState = _currentState.GetNextState();
        if (nextState != null)
            Transit(nextState);
    }

    private void Reset(State startState)
    {
        _currentState = startState;

        if (_currentState != null)
            _currentState.Enter(_target);
    }

    private void Transit(State nextState)
    {
        if (_currentState != null)
            _currentState.Exit();

        _currentState = nextState;

        if (_currentState != null)
            _currentState.Enter(_target);
    }
}