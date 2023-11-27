using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZombieTargetsCompositeRoot : CompositeRoot
{
    private Player _player;
    private Twins _twins;

    private List<ITarget> _targets = new List<ITarget>();

    public event UnityAction TargetDied;

    public Player Player => _player;

    private void OnDisable()
    {
        if(_player != null)
            _player.Died -= OnDied;

        if (_twins != null)
            _twins.Died -= OnDied;
    }

    public void Init(Player player, Twins twins)
    {
        _player = player;
        _twins = twins;
    }

    public override void Compose()
    {
        InitTargets();
    }

    public ITarget GetRandomTarget()
    {
        return _targets[Random.Range(0, _targets.Count)];
    }

    private void InitTargets()
    {
        _targets.Add(_player);
        _targets.Add(_twins);

        _player.Died += OnDied;
        _twins.Died += OnDied;
    }

    private void OnDied(IDamageable damageable)
    {
        TargetDied?.Invoke();
    }
}
