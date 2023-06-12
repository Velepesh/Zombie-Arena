using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class ZombieTargetsCompositeRoot : CompositeRoot
{
    [SerializeField] private PlayerCompositeRoot _playerCompositeRoot;
    [SerializeField] private TwinsCompositeRoot _twinsCompositeRoot;

    private List<ITarget> _targets = new List<ITarget>();

    public event UnityAction TargetDied;
    public event UnityAction<Twin, Twin> TwinDied;

    public Player Player => _playerCompositeRoot.Player;
    public Twin RightTwin => _twinsCompositeRoot.RightTwin;
    public Twin LeftTwin => _twinsCompositeRoot.LeftTwin;

    private void OnEnable()
    {
        _playerCompositeRoot.Died += OnDied;
        _twinsCompositeRoot.Died += OnDied;
        _twinsCompositeRoot.TwinDied += OnTwinDied;
    }

    private void OnDisable()
    {
        _playerCompositeRoot.Died -= OnDied;
        _twinsCompositeRoot.Died -= OnDied;
        _twinsCompositeRoot.TwinDied -= OnTwinDied;
    }

    public override void Compose()
    {
       InitTargets();
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

    private void InitTargets()
    {
        _targets.Add(Player);
        _targets.Add(RightTwin);
        _targets.Add(LeftTwin);
    }

    private void OnTwinDied(Twin twin)
    {
        Twin aliveTwin = _twinsCompositeRoot.GetAliveTwin(twin);
        _targets.Remove(twin);

        TwinDied?.Invoke(twin, aliveTwin);
    }

    private void OnDied()
    {
        TargetDied?.Invoke();
    }
}
