using UnityEngine;

public class PlayerViewSetup : Setup
{
    [SerializeField] private Player _player;
    [SerializeField] private DamageableHealthView _view;
    [SerializeField] private HudDamageScreen _damageScreen;
    [SerializeField] private bl_IndicatorManager _indicator;

    private PlayerViewPresenter _presenter;

    protected override void Awake()
    {
        _presenter = new PlayerViewPresenter(_view, _player, _indicator, _damageScreen);
    }

    protected override void OnEnable()
    {
        _presenter.Enable();
    }

    protected override void OnDisable()
    {
        _presenter.Disable();
    }
}