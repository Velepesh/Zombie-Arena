using System.Collections.Generic;
using UnityEngine;

public class State : MonoCache
{
    [SerializeField] private List<Transition> _transitions;

    protected ITarget Target { get; private set; }

    public void Enter(ITarget target)
    {
        if (enabled == false)
        {
            Target = target;
            enabled = true;

            foreach (var transition in _transitions)
            {
                transition.enabled = true;
                transition.Init(target);
            }
        }

        foreach (var transition in _transitions)
        {
            transition.enabled = true;
            transition.Init(target);
        }
    }

    public void Exit()
    {
        if (enabled == true)
        {
            foreach (var transition in _transitions)
                transition.enabled = false;

            enabled = false;
        }
    }

    public State GetNextState()
    {
        foreach (var transition in _transitions)
        {
            if (transition.NeedTransit)
                return transition.TargetState;
        }

        return null;
    }
}
