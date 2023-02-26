using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class HealthView : MonoBehaviour
{ 
    [SerializeField] private Slider _slider;
    [SerializeField] private float _duration;

    private int _currentHealth;

    protected int StartHealth { get; private set; }
    protected Slider Slider => _slider;

    private void OnValidate()
    {
        _duration = Mathf.Clamp(_duration, 0f, float.MaxValue);
    }


    protected void Init(IDamageable damageable)
    {
        AssignStartValues(damageable);
    }

    protected void DisableSlider()
    {
        _slider.gameObject.SetActive(false);
    }

    protected void ChangeSliderValue()
    {
        _slider.DOValue(_currentHealth, _duration);
    }

    protected void SetHealth(int health)
    {
        _currentHealth = health;

        ChangeSliderValue();
    }

    private void AssignStartValues(IDamageable damageable)
    {
        StartHealth = damageable.Health.Value;

        _currentHealth = StartHealth;
        _slider.maxValue = StartHealth;
        _slider.value = StartHealth;
    }
}