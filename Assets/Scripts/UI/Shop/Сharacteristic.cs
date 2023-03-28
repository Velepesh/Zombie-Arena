using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Сharacteristic : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _value;
    [SerializeField] private Slider _slider;
    [SerializeField] private int _maxValue;

    private void OnValidate()
    {
        _maxValue = Mathf.Clamp(_maxValue, 0, int.MaxValue);
    }

    private void Awake()
    {
        _slider.maxValue = _maxValue;
    }

    public void UpdateСharacteristic(int value)
    {
        _value.text = value.ToString();
        _slider.value = value;
    }
}