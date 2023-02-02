using UnityEngine;
using UnityEngine.UI;

public class HealthViewColor : MonoBehaviour
{
    [SerializeField] private PlayerHealthView _view;
    [SerializeField] private Image _fillArea;
    [SerializeField] private Color _fullHealthColor;
    [SerializeField] private Color _averageHealthColor;
    [SerializeField] private Color _lowHealthColor;

    readonly private int _colorsNumber = 3;

    private void OnEnable()
    {
        _view.HealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        _view.HealthChanged -= OnHealthChanged;
    }

    public void ChangeColor(int startHealth, int currentHealth)
    {
        float step = (float)startHealth / _colorsNumber;

        if(currentHealth >= startHealth - step)
            _fillArea.color = _fullHealthColor;
        else if (currentHealth >= startHealth - (step * 2))
            _fillArea.color = _averageHealthColor;
        else
            _fillArea.color = _lowHealthColor;
    }

    private void OnHealthChanged(int startHealt, int targetHealth)
    {
        ChangeColor(startHealt, targetHealth);
    }
}