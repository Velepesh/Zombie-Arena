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
    }

    private void OnDisable()
    {
        _healthAdder.HealthChanged -= OnHealthChanged;
    }

    private void Start()
    {
        UpdateText(_priceText, _healthAdder.Price);
    }

    private void OnHealthChanged(int health)
    {
        UpdateText(_currentHealthText, health);
    }

    private void UpdateText(TMP_Text text, int value)
    {
        text.text = value.ToString();
    }
}