using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SensitivitySettings : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    public event UnityAction<float> SensitivityUpdated;

    private void OnEnable()
    {
        _slider.onValueChanged.AddListener(delegate
        {
            UpdateSensitivityValue(_slider.value);
        });
    }

    private void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(delegate
        {
            UpdateSensitivityValue(_slider.value);
        });
    }

    public void Init(float value)
    {
        _slider.value = value;
        UpdateSensitivityValue(value);
    }

    private void UpdateSensitivityValue(float value)
    {
        SensitivityUpdated?.Invoke(value);
    }
}