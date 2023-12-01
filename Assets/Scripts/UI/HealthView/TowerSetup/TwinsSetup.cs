using UnityEngine;

public class TwinsSetup : TargetSetup
{
    [SerializeField] private DamageableHealthView _view;

    private Twins _twins;
    private TwinsPresenter _presenter;
    private TwinsHealthSaver _saver;

    public void Init(Twins twins)
    {
        _twins = twins;
        _saver = new TwinsHealthSaver(_twins);
        _presenter = new TwinsPresenter(_view, _twins, _saver);
        _presenter.Enable();
        OnHealthLoaded(_twins.Health);
    }

    private void OnDisable()
    {
        if (_presenter != null)
            _presenter.Disable();
    }

    public override void AddHealth(int value)
    {
        _twins.Health.AddHealth(value);
    }
}