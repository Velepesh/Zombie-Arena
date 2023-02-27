using UnityEngine;

public class TowerBuilder : Builder
{
    [SerializeField] private Tower _tower;
    [SerializeField] private TowerViewSetup _setup;

    public Tower Tower => _tower;

    public override void Form()
    {
        _setup.enabled = true;
    }

    public override void Deactivate()
    {
        _setup.enabled = false;
    }

    public override void AddHealth()
    {
        _tower.Health.AddHealth(10);
    }
}