using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(Zombie))]
[RequireComponent(typeof(ZombieAttacker))]
[RequireComponent(typeof(ZombieMover))]
[RequireComponent(typeof(Animator))]
public class ZombieAnimations : MonoBehaviour
{
    private Zombie _zombie;
    private ZombieAttacker _attacker;
    private ZombieMover _mover;
    private Animator _animator;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
        _attacker = GetComponent<ZombieAttacker>();
        _mover = GetComponent<ZombieMover>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _zombie.Died += OnDied;
        _mover.Moved += OnMoved;
        _attacker.Attacked += OnAttacked;
    }

    private void OnDisable()
    {
        _zombie.Died -= OnDied;
        _mover.Moved -= OnMoved;
        _attacker.Attacked -= OnAttacked;
    }

    private void OnDied(IDamageable damageable)
    {
        _animator.SetTrigger(ZombieAnimatorController.States.Die);
    }

    private void OnAttacked()
    {
        Attack();
    }

    private void OnMoved()
    {
        StopAttack();
    }

    private void Attack()
    {
        _animator.SetBool(ZombieAnimatorController.States.IsAttack, true);
    }

    private void StopAttack()
    {
        if(_animator.GetBool(ZombieAnimatorController.States.IsAttack))
            _animator.SetBool(ZombieAnimatorController.States.IsAttack, false);
    }
}
