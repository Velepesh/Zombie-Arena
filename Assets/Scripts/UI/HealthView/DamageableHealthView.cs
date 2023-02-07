using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DamageableHealthView : HealthView
{
    [SerializeField] private MonoBehaviour _damageable;
    [SerializeField] private TextScaleAnimation _textScaleAnimation;
    [SerializeField] private TMP_Text _healthText;

    private IDamageable Damageable => (IDamageable)_damageable;
    private int _startHealth;

    public event UnityAction<int, int> HealthChanged;

    private void OnEnable()
    {
        Damageable.Health.HealthChanged += OnHealthChanged;
        _startHealth = Damageable.Health.Value;
    }

    private void OnDisable()
    {
        Damageable.Health.HealthChanged -= OnHealthChanged;
    }

    private void Start()
    {
        Init(Damageable);
        EnableSlider();
        ChangeHealthText(Damageable.Health.Value);
    }

    private void OnHealthChanged(int health)
    {
        SetHealth(health);
        ChangeHealthText(health);
    }

    private void ChangeHealthText(int health)
    {
        _healthText.text = health.ToString();

        _textScaleAnimation.PlayScaleAnimation(_healthText);
        HealthChanged?.Invoke(_startHealth, health);
    }
}