using UnityEngine;
using YG;

public class TwinsCompositeRoot : Builder
{
    [SerializeField] private Twins _twins;
    [SerializeField] private TwinsViewSetup _setup;

    public Twins Twins => _twins;

    private void Awake()
    {
        _setup.enabled = false;

        if (YandexGame.SDKEnabled)
            Load();
    }

    private void OnEnable()
    {
        YandexGame.GetDataEvent += Load;
        _twins.Died += OnTwinDied;
    }

    private void OnDisable()
    {
        YandexGame.GetDataEvent -= Load;
        _twins.Died -= OnTwinDied;
    }

    public override void Compose()
    {
        _setup.enabled = true;
    }

    public override void AddHealth(int value)
    {
        _twins.Health.AddHealth(value);

        Save();
    }

    private void OnTwinDied(IDamageable damageable)
    {
        OnDied();
    }

    private void Load()
    {
        int health = YandexGame.savesData.TwinsHealth;

        _twins.Health.SetStartHealth(health);
    }

    private void Save()
    {
        if (_twins.Health.Value == YandexGame.savesData.TwinsHealth)
            return;

        YandexGame.savesData.TwinsHealth = _twins.Health.Value;
        YandexGame.SaveProgress();
    }
}