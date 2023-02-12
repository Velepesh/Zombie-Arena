using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoCache
{
    [SerializeField] private State _targetState;

    public State TargetState => _targetState;

    public bool NeedTransit { get; protected set; }

    private void OnEnable()
    {
        NeedTransit = false;
    }
}
