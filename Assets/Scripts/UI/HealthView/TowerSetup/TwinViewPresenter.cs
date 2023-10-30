public class TwinViewPresenter
{
    private DamageableHealthView _view;
    private Twins _model;

    public TwinViewPresenter(DamageableHealthView view, Twins model)
    {
        _view = view;
        _model = model;
    }

    public void Enable()
    {
        _model.Health.HealthChanged += OnHealthChanged;

        _view.SetIHealth(_model);
    }

    public void Disable()
    {
        _model.Health.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int health)
    {
        _view.UpdateView(health);
    }
}