public class TwinsPresenter
{
    private DamageableHealthView _view;
    private Twins _model;
    private TwinsHealthSaver _saver;

    public TwinsPresenter(DamageableHealthView view, Twins model, TwinsHealthSaver saver)
    {
        _view = view;
        _model = model;
        _saver = saver;
    }

    public void Enable()
    {
        _model.Health.HealthChanged += OnHealthChanged;
        _model.Health.HealthAdded += OnHealthAdded;
        _model.Died += OnDied;

        _view.SetIHealth(_model);
    }

    public void Disable()
    {
        _model.Health.HealthChanged -= OnHealthChanged;
        _model.Health.HealthAdded -= OnHealthAdded;
        _model.Died -= OnDied;
    } 

    private void OnHealthAdded(int health)
    {
        _saver.Save(health);
    }

    private void OnHealthChanged(int health)
    {
        _view.UpdateView(health);
    }

    private void OnDied(IDamageable damageable)
    {

    }
}