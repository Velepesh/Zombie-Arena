using UnityEngine;

public class PlayerSetup : TargetSetup
{
    [SerializeField] private DamageableHealthView _view;
    [SerializeField] private DamagePanel _damagePanel;
    
    private Player _player;
    private PlayerPresenter _presenter;
    private PlayerHealthSaver _saver;

    public void Init(Player player)
    {
        _player = player;
        _saver = new PlayerHealthSaver(player);
        _presenter = new PlayerPresenter(_view, _player, _saver, _damagePanel);
        _presenter.Enable();
        OnHealthLoaded(_player.Health);
    }

    private void OnDisable()
    {
        if(_presenter != null)
            _presenter.Disable();
    }

    public override void AddHealth(int value)
    {
        _player.Health.AddHealth(value);
    }
}