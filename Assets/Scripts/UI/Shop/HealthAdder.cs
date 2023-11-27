using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class HealthAdder : MonoBehaviour
{
    [SerializeField] private int _startHealth;
    [SerializeField] private string _label;
    [SerializeField] private TargetSetup _targetSetup;
    [SerializeField] private int _addedHealth = 10;
    [SerializeField] private int _price;
    [SerializeField] private int _addPrice;
    [SerializeField] private Button _buyButton;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private CanvasFade _canvasFade;

    private Health _health;

    public string Label => _label;

    public event UnityAction<int> HealthAdded;
    public event UnityAction<int> PriceIncreased;
    public event UnityAction<HealthAdder, int> BuyHealthButtonClicked;

    private void OnValidate()
    {
        _addedHealth = Mathf.Clamp(_addedHealth, 0, int.MaxValue);
        _price = Mathf.Clamp(_price, 0, int.MaxValue);
        _addPrice = Mathf.Clamp(_addPrice, 0, int.MaxValue);
    }

    private void OnEnable()
    {
        _targetSetup.HealthLoaded += OnHealthLoaded;
        _buyButton.onClick.AddListener(OnBuyHealthButtonClick);
    }

    private void OnDisable()
    {
        _targetSetup.HealthLoaded -= OnHealthLoaded;
        _buyButton.onClick.RemoveListener(OnBuyHealthButtonClick);

        if(_health != null)
            _health.HealthChanged -= OnHealthAdded;
    }

    public void AddHealth()
    {
        _targetSetup.AddHealth(_addedHealth);
        IncreasePrice();
        MetricaSender.Reward("health_adder");
    }

    public void HideHealthAdderPanel()
    {
        if(_canvasGroup.alpha != 0)
            _canvasFade.Hide();
    }

    private void OnHealthLoaded(Health health)
    {
        _health = health;
        _health.HealthAdded += OnHealthAdded;
        SetPrice();
        OnHealthAdded(_health.Value);
    }

    private void IncreasePrice()
    {
        _price += _addPrice;
        PriceIncreased?.Invoke(_price);
    }

    private void OnHealthAdded(int value)
    {
        HealthAdded?.Invoke(value);
    }

    private void OnBuyHealthButtonClick()
    {
        BuyHealthButtonClicked?.Invoke(this, _price);
    }

    private void SetPrice()
    {
        int delta = _health.Value - _startHealth;

        if (delta < 0)
            throw new ArgumentException(nameof(delta));

        int tensDifference = delta / 10;
        _price = _price + (tensDifference * _addPrice);
        PriceIncreased?.Invoke(_price);
    }
}