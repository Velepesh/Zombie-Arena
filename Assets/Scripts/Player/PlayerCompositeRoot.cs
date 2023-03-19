using UnityEngine;

public class PlayerCompositeRoot : Builder
{
    [SerializeField] private Player _player;
    [SerializeField] private PlayerEnabler _enabler;
    [SerializeField] private PlayerViewSetup _setup;

    public Player Player => _player;

    private void Start()
    {
        Deactivate();
    }

    public override void Compose()
    {
        _setup.enabled = true;
        _enabler.Activate();
    }

    public override void AddHealth()
    {
        _player.Health.AddHealth(10);
    }

    private void Deactivate()
    {
        _setup.enabled = false;
        _enabler.Deactivate();
    }
}