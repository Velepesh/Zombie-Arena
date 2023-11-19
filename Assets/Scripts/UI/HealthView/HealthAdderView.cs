using TMPro;
using UnityEngine;

public class HealthAdderView : MonoBehaviour
{
   [SerializeField] private HealthAdder _healthAdder;
   [SerializeField] private TMP_Text _currentHealthText;
   [SerializeField] private TMP_Text _priceText;

    private void OnEnable()
    {
        _healthAdder.HealthChanged += OnHealthChanged;
        _healthAdder.PriceIncreased += OnPriceIncreased;
    }

    private void OnDisable()
    {
        _healthAdder.HealthChanged -= OnHealthChanged;
        _healthAdder.PriceIncreased -= OnPriceIncreased;
    }

    private void OnHealthChanged(int health)
    {
        UpdateText(_currentHealthText, health);
    }

    private void OnPriceIncreased(int price)
    {
        UpdateText(_priceText, price);
    }

    private void UpdateText(TMP_Text text, int value)
    {
        text.text = value.ToString();
    }
}