using System;
using UnityEngine;
using YG;

public class TwinsSetup : TargetSetup
{
    [SerializeField] private DamageableHealthView _view;

    private Twins _twins;
    private TwinsPresenter _presenter;
    private TwinsSaver _saver;

    public void Init(Twins twins)
    {
        _twins = twins;
        _saver = new TwinsSaver(_twins);
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

    public override void Reborn()
    {
        _twins.Health.Reborn();
    }
}

public class TwinsSaver
{
    private Twins _twins;

    public TwinsSaver(Twins twins)
    {
        _twins = twins;
        Load();
    }

    public void Save(int health)
    {
        if (health <= 0)
            throw new ArgumentException(nameof(health));

        if (health == YandexGame.savesData.TwinsHealth)
            return;

        YandexGame.savesData.TwinsHealth = health;
        YandexGame.SaveProgress();
    }

    private void Load()
    {
        int health = YandexGame.savesData.TwinsHealth;

        _twins.Health.SetHealth(health);
    }
}