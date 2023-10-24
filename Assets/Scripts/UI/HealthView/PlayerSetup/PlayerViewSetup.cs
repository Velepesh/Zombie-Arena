using UnityEngine;
using YG;

public class PlayerViewSetup : Setup
{
    [SerializeField] private Player _player;
    [SerializeField] private DamageableHealthView _desktopView;
    [SerializeField] private DamageableHealthView _mobileView;
    [SerializeField] private HudDamageScreen _damageScreen;
    [SerializeField] private bl_IndicatorManager _indicator;

    private PlayerViewPresenter _presenter;
    private DamageableHealthView _view;

    protected override void OnEnable()
    {
        if (YandexGame.SDKEnabled == true)
        {
            if (YandexGame.EnvironmentData.isMobile)
                _view = _mobileView;
            else
                _view = _desktopView;

            _presenter = new PlayerViewPresenter(_view, _player, _indicator, _damageScreen);
            _presenter.Enable();
        }
    }

    protected override void OnDisable()
    {
        _presenter.Disable();
    }
}