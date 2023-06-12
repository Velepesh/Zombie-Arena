public class TwinViewPresenter
{
    private DamageableHealthView _view;
    private Twin _model;

    public TwinViewPresenter(DamageableHealthView view, Twin model)
    {
        _view = view;
        _model = model;
    }

    public void Enable()
    {
        _model.Health.HealthChanged += OnHealthChanged;

        _view.SetIDamageable(_model);
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