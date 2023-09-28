using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Zombie))]
[RequireComponent(typeof(ZombieAttacker))]
[RequireComponent(typeof(ZombieMover))]
[RequireComponent(typeof(Animator))]
public class ZombieAnimations : MonoBehaviour
{
    [SerializeField] private float _zombiePositionY;
    [SerializeField] private float _headKillimpactLength;
    [SerializeField] private float _bodyKillimpactLength;
    [SerializeField] private float _impactDuration;
    [SerializeField] private bool _isMultipleAttackStates;

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
        _attacker.AttackEventEnded += OnAttackEventEnded;
    }

    private void OnDisable()
    {
        _zombie.Died -= OnDied;
        _zombie.Spawned -= OnSpawned;
        _mover.Moved -= OnMoved;
        _attacker.Attacked -= OnAttacked;
        _attacker.AttackEventEnded -= OnAttackEventEnded;
    }

    private void OnAttackEventEnded()
    {
        if(_isMultipleAttackStates)
            ChangeAttackState();
    }

    private void OnDied(IDamageable damageable)
    {
        if (_zombie.IsHeadKill)
            SetDiedAnimation(ZombieAnimatorController.States.HeadDie, _headKillimpactLength);
        else
            SetDiedAnimation(ZombieAnimatorController.States.BodyDie, _bodyKillimpactLength);
    }

    private void SetDiedAnimation(string parametrName, float impactLength)
    {
        if (Random.Range(0, 2) == 1)
            _animator.SetBool(ZombieAnimatorController.States.IsMirror, true);

        Impact(_zombie.ContactPosition, impactLength);
        _animator.SetTrigger(parametrName);
    }

    private void OnAttacked() => Attack();

    private void OnMoved() => StopAttack();

    private void OnSpawned(Zombie zombie) => Run();

    private void Attack()
    {
        ChooseAttack();
    }

    private void Run()
    {
        _animator.SetTrigger(ZombieAnimatorController.States.Run);
    }

    private void StopAttack()
    {
        StopAttackAnimations();
        Run();
    }

    private void StopAttackAnimations()
    {
        if (_animator.GetBool(ZombieAnimatorController.States.IsHandAttack))
            _animator.SetBool(ZombieAnimatorController.States.IsHandAttack, false);

        if (_isMultipleAttackStates)
            if (_animator.GetBool(ZombieAnimatorController.States.IsLegAttack))
                _animator.SetBool(ZombieAnimatorController.States.IsLegAttack, false);
    }

    private void ChangeAttackState()
    {
        StopAttackAnimations();
        ChooseAttack();
    }

    private void ChooseAttack()
    {
        int number = 0;
        if (_isMultipleAttackStates)
            number = Random.Range(0, 2);

        if (number == 0)
            _animator.SetBool(ZombieAnimatorController.States.IsHandAttack, true);
        else
            _animator.SetBool(ZombieAnimatorController.States.IsLegAttack, true);
    }

    private void Impact(Vector3 direction, float length)
    {
        Vector3 target = transform.position + (-direction * length);
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