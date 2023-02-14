using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Zombie))]
[RequireComponent(typeof(ZombieAttacker))]
[RequireComponent(typeof(ZombieMover))]
[RequireComponent(typeof(Animator))]
public class ZombieAnimations : MonoBehaviour
{
    [SerializeField] private float _zombiePositionY;
    [SerializeField] private float _impactLength;
    [SerializeField] private float _impactDuration;

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
        _zombie.Spawned += OnSpawned;
        _mover.Moved += OnMoved;
        _attacker.Attacked += OnAttacked;
    }

    private void OnDisable()
    {
        _zombie.Died -= OnDied;
        _zombie.Spawned -= OnSpawned;
        _mover.Moved -= OnMoved;
        _attacker.Attacked -= OnAttacked;
    }

    private void OnDied(IDamageable damageable)
    {
        Impact(_zombie.ContactPosition);
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

    private void OnSpawned(Zombie zombie)
    {
        Run();
    }

    private void Attack()
    {
        _animator.SetBool(ZombieAnimatorController.States.IsAttack, true);
    }

    private void Run()
    {
        _animator.SetTrigger(ZombieAnimatorController.States.Run);
    }

    private void StopAttack()
    {
        if(_animator.GetBool(ZombieAnimatorController.States.IsAttack))
            _animator.SetBool(ZombieAnimatorController.States.IsAttack, false);
    }

    private void Impact(Vector3 direction)
    {
        Vector3 target = transform.position + (-direction * _impactLength);
        target = new Vector3(target.x, _zombiePositionY, target.z);

        StartCoroutine(ImpactByTime(target));
    }

    private IEnumerator ImpactByTime(Vector3 target)
    {
        float expiredSeconds = 0;
        float progress = 0;

        Vector3 startPosition = new Vector3(transform.position.x, _zombiePositionY, transform.position.z);

        while (progress < 1)
        {
            expiredSeconds += Time.deltaTime;
            progress = expiredSeconds / _impactDuration;

            transform.position = Vector3.Lerp(startPosition, target, progress);

            yield return null;
        }
    }
}