using UnityEngine;

public class TwinsCompositeRoot : Builder
{
    [SerializeField] private Twins _twins;
    [SerializeField] private TwinsViewSetup _setup;

    public Twins Twins => _twins;

    private void Awake()
    {
        _setup.enabled = false;
    }

    private void OnEnable()
    {
        _twins.Died += OnTwinDied;
    }

    private void OnDisable()
    {
        _twins.Died -= OnTwinDied;
    }

    public override void Compose()
    {
        _setup.enabled = true;
    }

    public override void AddHealth(int value)
    {
        _twins.Health.AddHealth(value);
    }

    private void OnTwinDied(IDamageable damageable)
    {
        OnDied();
    }
}