using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZombieTargets : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Tower _tower;

    private List<ITarget> _targets = new List<ITarget>();
    private List<IDamageable> _damageables = new List<IDamageable>();

    public event UnityAction<IDamageable> TargetDied;

    public Player Player => _player;
    public Tower Tower => _tower;


    private void OnEnable()
    {
        _player.Died += OnDied;
        _tower.Died += OnDied;
    }

    private void OnDisable()
    {
        _player.Died -= OnDied;
        _tower.Died -= OnDied;
    }

    private void Start()
    {
        SetupTargets();       
    }

    public ITarget GetRandomTarget()
    {
        return _targets[Random.Range(0, _targets.Count)];
    }

    public ITarget GetOtherTarget(ITarget currentTarget, Vector3 position)
    {
        float minDistance = 0;
        int index = 0;

        for (int i = 0; i < _targets.Count; i++)
        {
            if (_targets[i] != currentTarget)
            {
                float distanceToNewTarget = Vector3.Distance(position, _targets[i].Position);
                if (distanceToNewTarget < minDistance)
                {
                    minDistance = distanceToNewTarget;
                    index = i;
                }
            }
        }

        return _targets[index];
    }

    private void SetupTargets()
    {
        _targets.Add(_player);
        _targets.Add(_tower);

        for (int i = 0; i < _targets.Count; i++)
        {
            if (_targets[i] is IDamageable damageable)
                _damageables.Add(damageable);
            else
                throw new System.Exception($"{_targets[i]} must be IDamageable");
        }
    }

    private void OnDied(IDamageable damageable)
    {
        TargetDied?.Invoke(damageable);
    }
}
