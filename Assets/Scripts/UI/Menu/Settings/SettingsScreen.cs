using UnityEngine;
using UnityEngine.UI;
using System;

public class SettingsScreen : MonoBehaviour
{
    [SerializeField] private SensitivityView _sensitivityView;
    [SerializeField] private VolumeView _volumeView;
    [SerializeField] private Button _backButton;

    public event Action BackButtonClicked;

    private void OnEnable()
    {
        _backButton.onClick.AddListener(OnBackButtonClick);
    }

    private void OnDisable()
    {
        _backButton.onClick.RemoveListener(OnBackButtonClick);
    }
    
    public void UpdateSettingSlidersValue(float sensitivity, float music, float sfx)
    {
        _sensitivityView.Init(sensitivity);
        _volumeView.Init(sfx, music);
    }

    private void OnBackButtonClick()
    {
        BackButtonClicked?.Invoke();
    }
}