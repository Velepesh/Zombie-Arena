using UnityEngine;

public class PlayerHealthSetup : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private DamageableHealthView _view;
    [SerializeField] private HudDamageScreen _damageScreen;
    [SerializeField] private bl_IndicatorManager _indicator;

    private PlayerViewPresenter _presenter;

    private void Awake()
    {
        _presenter = new PlayerViewPresenter(_view, _player, _indicator, _damageScreen);
    }

    private void OnEnable()
    {
        _presenter.Enable();
    }

    private void OnDisable()
    {
        _presenter.Disable();
    }
}
