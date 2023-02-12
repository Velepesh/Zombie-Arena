using UnityEngine;

[RequireComponent(typeof(ZombieDestroyer))]
public class ResetTransition : Transition
{
    private ZombieDestroyer _destroyer;

    private void Awake()
    {
        _destroyer = GetComponent<ZombieDestroyer>();
    }

    private void OnEnable()
    {
        NeedTransit = false;
        _destroyer.Destroyed += OnDestroyed;
    }

    private void OnDisable()
    {
        _destroyer.Destroyed -= OnDestroyed;
    }

    private void OnDestroyed()
    {
        NeedTransit = true;
    }
}