public class PlayerPresenter
{
    private DamageableHealthView _view;
    private Player _model;
    private DamagePanel _damagePanel;
    private PlayerSaver _saver;

    public PlayerPresenter(DamageableHealthView view, Player model, PlayerSaver saver, DamagePanel damagePanel)
    {
        _view = view;
        _model = model;
        _saver = saver;
        _damagePanel = damagePanel;
    }

    public void Enable()
    {
        _model.Attacked += OnAttacked;
        _model.Health.HealthChanged += OnHealthChanged;
        _model.Health.HealthAdded += OnHealthAdded;
        _model.Died += OnDied;
        _view.HealthChanged += OnHealthChanged;

        _view.SetIHealth(_model);
        _damagePanel.Init(_model);
    }

    public void Disable()
    {
        _model.Attacked -= OnAttacked;
        _model.Health.HealthChanged -= OnHealthChanged;
        _model.Health.HealthAdded -= OnHealthAdded;
        _model.Died -= OnDied;
        _view.HealthChanged -= OnHealthChanged;
    }

    private void OnAttacked(Zombie zombie)
    {
        _damagePanel.OnAttacked(zombie);
    }

    private void OnHealthAdded(int health)
    {
        _saver.Save(health);
    }

    private void OnHealthChanged(int health)
    {
        _view.UpdateView(health);
    }

    private void OnHealthChanged(int startHealth, int health)
    {
        _damagePanel.OnHealthChanged(startHealth, health);
    }

    private void OnDied(IDamageable damageable)
    {

    }
}