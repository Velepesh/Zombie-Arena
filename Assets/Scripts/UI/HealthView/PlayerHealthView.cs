using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealthView : HealthView
{
    [SerializeField] private Player _player;
    [SerializeField] private TextScaleAnimation _textScaleAnimation;
    [SerializeField] private TMP_Text _healthText;

    private int _startHealth;

    public event UnityAction<int, int> HealthChanged;

    private void OnEnable()
    {
        _player.Health.HealthChanged += OnHealthChanged;
        _startHealth = _player.Health.Value;
    }

    private void OnDisable()
    {
        _player.Health.HealthChanged -= OnHealthChanged;
    }

    private void Start()
    {
        Init(_player);
        EnableSlider();
        ChangeHealthText(_player.Health.Value);
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