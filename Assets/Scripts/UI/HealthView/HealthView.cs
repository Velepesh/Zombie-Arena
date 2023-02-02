using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class HealthView : MonoBehaviour
{ 
    [SerializeField] private Slider _slider;
    [SerializeField] private float _speed;

    private int _currentHealth;

    protected Slider Slider => _slider;

    protected void Init(IDamageable damageable)
    {
        AssignStartValues(damageable);
    }

    protected void EnableSlider()
    {
        _slider.gameObject.SetActive(true);
    }

    protected void DisableSlider()
    {
        _slider.gameObject.SetActive(false);
    }

    protected void ChangeSliderValue()
    {
        _slider.DOValue(_currentHealth, _speed);
    }

    protected void SetHealth(int health)
    {
        _currentHealth = health;

        ChangeSliderValue();
    }

    private void AssignStartValues(IDamageable damageable)
    {
        _currentHealth = damageable.Health.Value;
        _slider.maxValue = _currentHealth;
        _slider.value = _currentHealth;
    }
}