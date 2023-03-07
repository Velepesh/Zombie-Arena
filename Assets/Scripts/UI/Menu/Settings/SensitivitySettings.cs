using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SensitivitySettings : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private const string SENSITIVITY = "Sensitivity";

    private float _sensitivity => PlayerPrefs.GetFloat(SENSITIVITY, 0);

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

    private void Start() => Load();

    public void UpdateSensitivityValue(float value)
    {
        SensitivityUpdated?.Invoke(value);

        SaveValue(value);
    }

    private void Load()
    {
        _slider.value = _sensitivity;
        UpdateSensitivityValue(_sensitivity);
    }

    private void SaveValue(float value)
    {
        PlayerPrefs.SetFloat(SENSITIVITY, value);
    }
}