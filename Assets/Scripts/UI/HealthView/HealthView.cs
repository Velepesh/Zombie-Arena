using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class HealthView : MonoBehaviour
{ 
    [SerializeField] private Slider _slider;
    [SerializeField] private Slider _damagedSlider;
    [SerializeField] private float _duration;
    [SerializeField] private float _delayBeforMoveDamaged;
    [SerializeField] private float _damagedDsuration;

    private int _currentHealth;

    protected int StartHealth { get; private set; }

    private void OnValidate()
    {
        _duration = Mathf.Clamp(_duration, 0f, float.MaxValue);
        _delayBeforMoveDamaged = Mathf.Clamp(_delayBeforMoveDamaged, 0f, float.MaxValue);
        _damagedDsuration = Mathf.Clamp(_damagedDsuration, 0f, float.MaxValue);
    }


    protected void Init(IDamageable damageable)
    {
        AssignStartValues(damageable);
    }

    protected void DisableSlider()
    {
        _slider.gameObject.SetActive(false);

        if(_damagedSlider != null)
            _damagedSlider.gameObject.SetActive(false);
    }

    protected void ChangeSliderValue()
    {
        _slider.DOValue(_currentHealth, _duration);

        if (_damagedSlider != null)
        {
            _damagedSlider.DOValue(_currentHealth, _damagedDsuration).SetDelay(_delayBeforMoveDamaged);
        }
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

        if (_damagedSlider != null)
        {
            _damagedSlider.value = StartHealth;
            _damagedSlider.value = StartHealth;
        }
    }
}