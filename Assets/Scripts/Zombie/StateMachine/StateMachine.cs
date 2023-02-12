using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Zombie))]
public class StateMachine : MonoCache
{
    [SerializeField] private State _firstState;

    private State _currentState;

    public State Current => _currentState;

    public event UnityAction Enabled;


    private void OnEnable()
    {
        StartCoroutine(SetStartState());
        AddUpdate();
    }

    private IEnumerator SetStartState()
    {
        Debug.Log("SetStartState");
        yield return new WaitForSeconds(0.1f);
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

    private void OnReseted()
    {
     //   Transit(_firstState);
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