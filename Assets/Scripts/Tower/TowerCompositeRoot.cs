using UnityEngine;

public class TowerCompositeRoot : Builder
{
    [SerializeField] private Tower _tower;
    [SerializeField] private TowerViewSetup _setup;

    public Tower Tower => _tower;

    private void Awake()
    {
        _setup.enabled = false;
    }

    public override void Compose()
    {
        _setup.enabled = true;
    }

    public override void AddHealth(int value)
    {
        _tower.Health.AddHealth(value);
    }
}