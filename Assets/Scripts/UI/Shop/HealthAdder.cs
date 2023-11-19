using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class HealthAdder : MonoBehaviour
{
    [SerializeField] private int _startHealth;
    [SerializeField] private string _label;
    [SerializeField] private Builder _builderCompositeRoot;
    [SerializeField] private int _addedHealth = 10;
    [SerializeField] private int _price;
    [SerializeField] private int _addPrice;
    [SerializeField] private Button _buyButton;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private CanvasFade _canvasFade;

    private Health _health;

    public string Label => _label;

    public event UnityAction<int> HealthChanged;
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
        _builderCompositeRoot.HealthLoaded += OnHealthLoaded;
        _buyButton.onClick.AddListener(OnBuyHealthButtonClick);
    }

    private void OnDisable()
    {
        _buyButton.onClick.RemoveListener(OnBuyHealthButtonClick);

        if(_health != null)
            _health.HealthChanged -= OnHealthChanged;
    }

    public void AddHealth()
    {
        _builderCompositeRoot.AddHealth(_addedHealth);
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
        _health.HealthChanged += OnHealthChanged;
        SetPrice();
        OnHealthChanged(_health.Value);
    }

    private void IncreasePrice()
    {
        _price += _addPrice;
        PriceIncreased?.Invoke(_price);
    }

    private void OnHealthChanged(int value)
    {
        HealthChanged?.Invoke(value);
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