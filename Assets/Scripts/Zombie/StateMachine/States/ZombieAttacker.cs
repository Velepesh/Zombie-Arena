using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Zombie))]
[RequireComponent(typeof(NavAgentEnabler))]
public class ZombieAttacker : State
{
    [SerializeField] private List<AttackCollider> _attackColliders;

    private Zombie _zombie;
    private NavAgentEnabler _agent;

    public event UnityAction Attacked;
    public event UnityAction AttackEventEnded;

    private void Awake()
    {
        int countOfColliders = _attackColliders.Count;

        if (countOfColliders == 0 || countOfColliders > 2)
            throw new ArgumentOutOfRangeException(nameof(countOfColliders));

        _zombie = GetComponent<Zombie>();
        _agent = GetComponent<NavAgentEnabler>();
    }

    private void OnEnable()
    {
        DisableAttackColliders();
        _agent.StopAgent();
        Attack();
    }

    private void OnDisable()
    {
        for (int i = 0; i < _attackColliders.Count; i++)
            _attackColliders[i].DisableCollider();
    }

    private void OnStartAttackEvent()
    {
        _attackColliders[0].EnableCollider();
    }

    private void OnStartSecondAttackEvent()
    {
        if (_attackColliders[1] == null)
            throw new ArgumentNullException("Attack Collider is null");

        _attackColliders[1].EnableCollider();
    }

    private void OnEndAttackEvent()
    {
        DisableAttackColliders();
    }

    private void DisableAttackColliders()
    {
        for (int i = 0; i < _attackColliders.Count; i++)
            _attackColliders[i].DisableCollider();
    }

    private void OnEndAttackAnimationEvent()
    {
        _zombie.SetAttackingState(false);
        AttackEventEnded?.Invoke();
    }

    private void Attack()
    {
        _zombie.SetAttackingState(true);

        for (int i = 0; i < _attackColliders.Count; i++)
            _attackColliders[i].Init(_zombie);

        Attacked?.Invoke();

        if (_zombie.CurrentTarget is Twins)
            _zombie.SetAttackedTwins();
    }
}