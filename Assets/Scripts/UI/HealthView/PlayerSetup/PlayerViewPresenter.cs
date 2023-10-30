public class PlayerViewPresenter
{
    private DamageableHealthView _view;
    private Player _model;
    private bl_IndicatorManager _indicator;
    private HudDamageScreen _damageScreen;

    public PlayerViewPresenter(DamageableHealthView view, Player model, bl_IndicatorManager indicator, HudDamageScreen damageScreen)
    {
        _view = view;
        _model = model;
        _indicator = indicator;
        _damageScreen = damageScreen;
    }

    public void Enable()
    {
        _model.Attacked += OnAttacked;
        _model.Health.HealthChanged += OnHealthChanged;
        _view.HealthChanged += OnHealthChanged;

        _view.SetIHealth(_model);
        _indicator.SetPlayer(_model);
    }

    public void Disable()
    {
        _model.Attacked -= OnAttacked;
        _model.Health.HealthChanged -= OnHealthChanged;
        _view.HealthChanged -= OnHealthChanged;
    }

    private void OnAttacked(Zombie zombie)
    {
        _indicator.SetIndicator(zombie);
    }

    private void OnHealthChanged(int health)
    {
        _view.UpdateView(health);
    }

    private void OnHealthChanged(int startHealth, int health)
    {
        _damageScreen.UpdateDamageScreen(startHealth, health);
    }
}