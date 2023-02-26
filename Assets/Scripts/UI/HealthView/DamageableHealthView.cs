using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DamageableHealthView : HealthView
{
    [SerializeField] private TextScaleAnimation _textScaleAnimation;
    [SerializeField] private TMP_Text _healthText;

    public event UnityAction<int, int> HealthChanged;

    public void SetIDamageable(IDamageable damageable)
    {
        Init(damageable);
        UpdateView(damageable.Health.Value);
    }

    public void UpdateView(int health)
    {
        SetHealth(health);
        ChangeHealthText(health);
    }

    private void ChangeHealthText(int health)
    {
        _healthText.text = health.ToString();
        _textScaleAnimation.PlayScaleAnimation(_healthText);
        HealthChanged?.Invoke(StartHealth, health);
    }
}