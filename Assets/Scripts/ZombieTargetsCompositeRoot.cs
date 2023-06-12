using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZombieTargetsCompositeRoot : CompositeRoot
{
    [SerializeField] private PlayerCompositeRoot _playerCompositeRoot;
    [SerializeField] private TwinsCompositeRoot _twinsCompositeRoot;

    private List<ITarget> _targets = new List<ITarget>();

    public event UnityAction TargetDied;

    public Player Player => _playerCompositeRoot.Player;
    public Twins Twins => _twinsCompositeRoot.Twins;

    private void OnEnable()
    {
        _playerCompositeRoot.Died += OnDied;
        _twinsCompositeRoot.Died += OnDied;
    }

    private void OnDisable()
    {
        _playerCompositeRoot.Died -= OnDied;
        _twinsCompositeRoot.Died -= OnDied;
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
        _targets.Add(Player);
        _targets.Add(Twins);
    }

    private void OnDied()
    {
        TargetDied?.Invoke();
    }
}
